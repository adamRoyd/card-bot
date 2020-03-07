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

            lines.RemoveRange(0, index + 2);

            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }
    }
}
