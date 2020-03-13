using Engine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace bot.Services
{
    public class HandHistoryService : IHandHistoryService
    {
        public void GetLatestHistory(string path)
        {
            List<string> lines = File.ReadLines(path).ToList();

            Player[] players = GetPlayers();

            int index = lines.FindLastIndex(t => t == "*** SUMMARY ***");

            var stackSection = lines.Skip(lines.FindLastIndex(t => t == "PokerStars Hand"))
                                    .Take(lines.FindLastIndex(t => t == "*** HOLE CARDS ***")); // TODO is hole cards always present?

            var betsSection = lines.Skip(lines.FindLastIndex(t => t == "*** HOLE CARDS ***")) 
                                    .Take(lines.FindLastIndex(t => t == "*** SUMMARY ***"));

            var winningsSection = lines.Skip(lines.FindLastIndex(t => t == "*** SUMMARY ***"))
                                    .Take(lines.Count - 1);

            Console.WriteLine("/// STACKS ///");
            stackSection.ToList().ForEach(line => Console.WriteLine(line));
            Console.WriteLine("/// BETS ///");
            betsSection.ToList().ForEach(line => Console.WriteLine(line));
            Console.WriteLine("/// SUMMARY ///");
            winningsSection.ToList().ForEach(line => Console.WriteLine(line));



            //lines.RemoveAll(line => !line.StartsWith("Seat"));

            //int heroIndex = lines.FindIndex(line => line.Contains("CannonballJim"));
            //int truePosition = 1;
            //int oldPosition = GetPosition(lines.ElementAt(heroIndex));

            //// First count down from hero and assign players
            //for (int i = heroIndex; i >= 0; i--)
            //{
            //    string line = lines.ElementAt(i);
            //    int seatPosition = GetPosition(line);
            //    int positionDifference = oldPosition - seatPosition;
            //    truePosition += positionDifference;

            //    if (players.FirstOrDefault(p => p.Position == truePosition) != null)
            //    {
            //        players.FirstOrDefault(p => p.Position == truePosition).Stack += GetStackChange(line);
            //    }

            //    oldPosition = seatPosition;
            //}

            //oldPosition = GetPosition(lines.ElementAt(heroIndex));
            //truePosition = 10;

            //// Then count anybody beyond hero
            //for (int i = heroIndex + 1; i < lines.Count(); i++)
            //{
            //    string line = lines.ElementAt(i);
            //    int seatPosition = GetPosition(line);
            //    int positionDifference = seatPosition - oldPosition;
            //    truePosition -= positionDifference;

            //    if (players.FirstOrDefault(p => p.Position == truePosition) != null)
            //    {
            //        players.FirstOrDefault(p => p.Position == truePosition).Stack += GetStackChange(line);
            //    }

            //    oldPosition = seatPosition;
            //}

            //foreach (Player player in players)
            //{
            //    Console.WriteLine($"Player: {player.Position} Stack: {player.Stack}");
            //}
        }

        private int GetStackChange(string line)
        {
            if (line.Contains("collected") || line.Contains("wins") || line.Contains("won"))
            {
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
            else
            {
                return 0;
            }
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
