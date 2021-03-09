using HomeAngularApp.Services.AdventOfCode.Intf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeAngularApp.Services.AdventOfCode.Impl
{
    public class ReportRepair : IAdventOfCodeSolution
    {
        private const int DesiredSum = 2020;

        public string Name => "Day 1: Report Repair";

        public async Task<string> ExecuteAsync(IEnumerable<string> inputLines)
        {
            var values = new List<int>();

            foreach (var line in inputLines)
            {
                var success = int.TryParse(line, out var value);

                if (success)
                {
                    values.Add(value);
                }
            }

            // Could improve performance at the cost of complexity if you use a HashSet/Dictionary to only loop the array a single time
            for (var i = 0; i < values.Count; i++)
            {
                for (var j = i + 1; j < values.Count; j++)
                {
                    var sum = values[i] + values[j];

                    if (sum == DesiredSum)
                    {
                        return (values[i] * values[j]).ToString();
                    }
                }
            }

            throw new InvalidOperationException("No matching result found from provided input.");
        }
    }
}
