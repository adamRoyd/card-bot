using Engine.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace bot.Services
{
    public class HandHistoryService : IHandHistoryService
    {
        public Player[] GetPlayersFromHistory(string path)
        {
            var directory = new DirectoryInfo(path);
            var latestHistoryFile = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First();

            List<string> lines = File.ReadLines(latestHistoryFile.FullName).ToList();

            Player[] players = GetPlayers();

            int pokerStarsHandIndex = lines.FindLastIndex(t => t.Contains("PokerStars Hand"));
            int flopIndex = lines.FindLastIndex(t => t.Contains("*** FLOP ***"));
            int summaryIndex = lines.FindLastIndex(t => t.Contains("*** SUMMARY ***"));


            List<string> fullHistory = lines.Skip(pokerStarsHandIndex)
                                    .Take(summaryIndex - pokerStarsHandIndex)
                                    .ToList();

            List<string> preFlop = lines.Skip(pokerStarsHandIndex)
                                    .Take(flopIndex - pokerStarsHandIndex)
                                    .ToList();

            List<string> postFlop = lines.Skip(flopIndex)
                        .Take(summaryIndex - flopIndex)
                        .ToList();

            LogHistoryId(fullHistory.First());

            SetNames(fullHistory, players);

            SetInitialStacks(fullHistory, players);

            SetBlinds(fullHistory, players);

            AddOrDeduct(fullHistory, players, "small blind ", false);
            AddOrDeduct(fullHistory, players, "big blind ", false);
            AddOrDeduct(fullHistory, players, "ante ", false);
            AddOrDeduct(fullHistory, players, "bets ", false);
            AddOrDeduct(fullHistory, players, "calls ", false);
            AddOrDeduct(fullHistory, players, "collected ", true);
            AddOrDeduct(fullHistory, players, "Uncalled bet ", true);

            // Check if flop dealt. If not, treat full history as pre flop.
            if (flopIndex > pokerStarsHandIndex)
            {
                DeductPreFlopRaise(preFlop, players);
                AddOrDeduct(postFlop, players, "raises \\d+ to ", false);
            }
            else
            {
                DeductPreFlopRaise(fullHistory, players);
            }

            foreach (Player player in players)
            {
                Debug.WriteLine($"Player: {player.Position} {player.Name} Stack: {player.Stack}");
            }

            return players;
        }

        private void DeductPreFlopRaise(List<string> lines, Player[] players)
        {
            lines
            .Where(line => Regex.IsMatch(line, "raises "))
            .ToList()
            .ForEach(line =>
            {
                string name = GetName(line);

                if (IsMatch(players, name))
                {
                    string firstRaise = Regex.Split(line, "raises ")[1].Split(" ")[0];
                    string secondRaise = Regex.Split(line, " to ")[1].Split(" ")[0];

                    int.TryParse(firstRaise, out int first);
                    int.TryParse(secondRaise, out int second);

                    if (players.FirstOrDefault(p => p.Name == name).IsBlind)
                    {
                        Debug.WriteLine($"Deduct firstRaise, {name}, -{first}");
                        players.FirstOrDefault(p => p.Name == name).Stack -= first;
                    }
                    else
                    {
                        Debug.WriteLine($"Deduct secondRaise, {name}, -{second}");
                        players.FirstOrDefault(p => p.Name == name).Stack -= second;
                    }

                }
            }
            );
        }

        private void LogHistoryId(string line)
        {
            var expression = "#\\d+:";
            var id = Regex.Match(line, expression);
            Debug.WriteLine($"History: {id}");
        }

        private void AddOrDeduct(List<string> lines, Player[] players, string expression, bool isAddition)
        {
            lines
                .Where(line => Regex.IsMatch(line, expression))
                .ToList()
                .ForEach(line =>
                {
                    string name = GetName(line);

                    if (IsMatch(players, name))
                    {
                        string numberInText = Regex.Split(line, expression)[1].Split(" ")[0].Replace("(", "").Replace(")", "");

                        int.TryParse(numberInText, out int value);

                        if (isAddition)
                        {
                            Debug.WriteLine($"Add {expression}, {name}, +{value}");
                            players.FirstOrDefault(p => p.Name == name).Stack += value;
                        }
                        else
                        {
                            Debug.WriteLine($"Deduct {expression}, {name}, -{value}");
                            players.FirstOrDefault(p => p.Name == name).Stack -= value;
                        }
                    }
                }
            );
        }

        private static bool IsMatch(Player[] players, string name)
        {
            return !string.IsNullOrEmpty(name) && players.FirstOrDefault(p => p.Name == name) != null;
        }

        private void SetInitialStacks(List<string> lines, Player[] players)
        {
            lines.Where(line => line.StartsWith("Seat"))
                .ToList()
                .ForEach(line =>
            {
                string name = GetName(line);

                if (IsMatch(players, name))
                {
                    players.FirstOrDefault(p => p.Name == name).Stack = GetStack(line);
                }
            });
        }

        private void SetBlinds(List<string> lines, Player[] players)
        {
            lines.Where(line => line.Contains("posts small blind") || line.Contains("posts big blind"))
                .ToList()
                .ForEach(line =>
                {
                    string name = GetName(line);

                    if (IsMatch(players, name))
                    {
                        players.FirstOrDefault(p => p.Name == name).IsBlind = true;
                    }
                });
        }

        private void SetNames(List<string> lines, Player[] players)
        {
            List<string> playerList = lines.Where(line => line.StartsWith("Seat")).ToList();

            int heroIndex = playerList.FindIndex(line => line.Contains("CannonballJim"));
            int truePosition = 1;
            int oldPosition = GetPosition(playerList.ElementAt(heroIndex));

            // First count down from hero and assign players
            for (int i = heroIndex; i >= 0; i--)
            {
                string line = playerList.ElementAt(i);
                int seatPosition = GetPosition(line);
                int positionDifference = oldPosition - seatPosition;
                truePosition += positionDifference;

                if (players.FirstOrDefault(p => p.Position == truePosition) != null)
                {
                    players.FirstOrDefault(p => p.Position == truePosition).Name = GetName(line);
                }

                oldPosition = seatPosition;
            }

            oldPosition = GetPosition(playerList.ElementAt(heroIndex));
            truePosition = 10;

            // Then count anybody beyond hero
            for (int i = heroIndex + 1; i < playerList.Count(); i++)
            {
                string line = playerList.ElementAt(i);
                int seatPosition = GetPosition(line);
                int positionDifference = seatPosition - oldPosition;
                truePosition -= positionDifference;

                if (players.FirstOrDefault(p => p.Position == truePosition) != null)
                {
                    players.FirstOrDefault(p => p.Position == truePosition).Name += GetName(line);
                }

                oldPosition = seatPosition;
            }
        }

        private string GetName(string line)
        {
            if (line.Contains("Seat "))
            {
                string[] output = line.Split('(', ')')[0].Split(':');

                return output[1].Trim();
            }
            else if (line.Contains(" returned to "))
            {
                string[] output = line.Split(" ");

                return output[^1].Trim();
            }
            else if (line.Contains(":"))
            {
                return line.Split(":")[0];
            }
            else if (line.Contains(" collected "))
            {
                return line.Split(" collected ")[0];
            }
            else
            {
                Console.WriteLine($"Get name not accurate! line: {line}");
                return line.Split(" ")[0];
            }
        }

        private int GetStack(string line)
        {
            line = line.Replace("in chips", "");

            string[] output = line.Split('(', ')');

            foreach (string section in output)
            {
                if (int.TryParse(section, out int value))
                {
                    return value;
                }
            }

            return 0;
        }

        private int GetPosition(string line)
        {
            int.TryParse(line.Split(" ")[1].Replace(":", ""), out int position);
            return position;
        }

        private Player[] GetPlayers()
        {
            return new Player[]
                {
                    new Player{
                        Position = 1
                    },
                    new Player{
                        Position = 2
                    },
                    new Player{
                        Position = 3
                    },
                    new Player{
                        Position = 4
                    },
                    new Player{
                        Position = 5
                    },
                    new Player{
                        Position = 6
                    },
                    new Player{
                        Position = 7
                    },
                    new Player{
                        Position = 8
                    },
                    new Player{
                        Position = 9
                    }
                };
        }
    }
}
