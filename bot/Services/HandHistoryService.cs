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

            int index = lines.FindLastIndex(t => t == "*** SUMMARY ***");

            lines.RemoveRange(0, index);

            lines.RemoveAll(line => !line.StartsWith("Seat"));

            Player[] players = GetPlayers();

            //lines = lines.Where(line => line.Contains("collected") || line.Contains("wins")).ToList();

            int heroIndex = lines.FindIndex(line => line.Contains("CannonballJim"));
            int truePosition = 1;
            int oldPosition = GetPosition(lines.ElementAt(heroIndex));

            // First count down from hero and assign players
            for (int i = heroIndex; i >= 0; i--)
            {
                string line = lines.ElementAt(i);
                int seatPosition = GetPosition(line);
                int positionDifference = oldPosition - seatPosition;
                truePosition += positionDifference;
                //Console.WriteLine($"Position {truePosition} {line}");

                if (players.FirstOrDefault(p => p.Position == truePosition) != null)
                {
                    players.FirstOrDefault(p => p.Position == truePosition).Stack += GetStackChange(line);
                }

                oldPosition = seatPosition;
            }

            oldPosition = GetPosition(lines.ElementAt(heroIndex));

            // Then count anybody beyond hero
            for (int i = lines.Count() - 1; i > heroIndex; i--)
            {
                string line = lines.ElementAt(i);
                int seatPosition = GetPosition(line);
                int positionDifference = seatPosition - oldPosition;
                truePosition += positionDifference;
                //Console.WriteLine($"Position {truePosition} {line}");
                if (players.FirstOrDefault(p => p.Position == truePosition) != null)
                {
                    players.FirstOrDefault(p => p.Position == truePosition).Stack += GetStackChange(line);
                }
            }

            foreach (Player player in players)
            {
                Console.WriteLine($"Player: {player.Position} Stack: {player.Stack}");
            }
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
