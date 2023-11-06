using System;
using System.Collections.Generic;

namespace CoCStatsTracker
{
    public class Program
    {
        static void Main(string[] args)
        {
            var clanWars = new List<string>() { "1", "2", "3", "4", "5", "6" };

            var countToSave = 5;

            for (int i = countToSave; i < clanWars.Count; i++)
            {
                clanWars.Remove(clanWars[i]);
            }

            foreach (var cw in clanWars)
            {
                Console.WriteLine(cw);
            }
        }
    }
}
