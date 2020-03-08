using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace bot.Services
{
    public class HandHistoryService : IHandHistoryService
    {
        public void GetLatestHistory(string path)
        {
            List<string> lines = File.ReadLines(path).ToList();

            var index = lines.FindLastIndex(t => t == "*** SUMMARY ***");

            lines.RemoveRange(0, index);

            lines.RemoveAll(line => !line.StartsWith("Seat"));

            //lines = lines.Where(line => line.Contains("collected") || line.Contains("wins")).ToList();

            int heroIndex = lines.FindIndex(line => line.Contains("CannonballJim"));
            int numberOfPlayers = 9;
            int truePosition = 1;
            int oldPosition = GetPosition(lines.ElementAt(heroIndex));

            // First count down from hero and assign players
            for (var i = heroIndex; i >= 0; i--)
            {
                var line = lines.ElementAt(i);
                int seatPosition = GetPosition(line);
                int positionDifference = oldPosition - seatPosition;
                truePosition += positionDifference;
                Console.WriteLine($"Position {truePosition} {line}");
            
                oldPosition = seatPosition;
            }

            // Then count anybody beyond hero
            for (var i = lines.Count() - 1; i > heroIndex; i--)
            {
                var line = lines.ElementAt(i);
                int seatPosition = GetPosition(line);
                truePosition++; // TODO work out position difference here
                Console.WriteLine($"Position {truePosition} {line}");
            }
        }

        private int GetPosition(string line)
        {
            int.TryParse(line.Split(" ")[1].Replace(":", ""), out int position);
            return position;
        }
    }
}
