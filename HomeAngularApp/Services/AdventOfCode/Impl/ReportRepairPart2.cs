using HomeAngularApp.Services.AdventOfCode.Intf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeAngularApp.Services.AdventOfCode.Impl
{
    public class ReportRepairPart2 : IAdventOfCodeSolution
    {
        private const int DesiredSum = 2020;

        public string Name => "Day 1: Report Repair (Part 2)";

        // TODO: Might be able to share code with the original report repair problem
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

            for (var i = 0; i < values.Count; i++)
            {
                for (var j = i + 1; j < values.Count; j++)
                {
                    for (var k = j + 1; k < values.Count; k++)
                    {
                        var sum = values[i] + values[j] + values[k];

                        if (sum == DesiredSum)
                        {
                            return (values[i] * values[j] * values[k]).ToString();
                        }
                    }
                }
            }

            throw new InvalidOperationException("No matching result found from provided input.");
        }
    }
}
