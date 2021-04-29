using HomeAngularApp.Services.AdventOfCode.Intf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HomeAngularApp.Services.AdventOfCode.Impl
{
    public class TobogganTrajectory : IAdventOfCodeSolution
    {
        public string Name => "Day 3: Toboggan Trajectory";

        public async Task<string> ExecuteAsync(IEnumerable<string> input)
        {
            const int columnTraversal = 3;
            const int rowTraversal = 1;

            var inputArray = input.ToArray();
            var inputColumns = inputArray[0].Length; // This assumes all rows have the same length
            var inputRows = inputArray.Length;

            var currentColumn = 0;
            var treeCount = 0;
            var nonTreeCount = 0;
            for (var currentRow = 1; currentRow < inputRows; currentRow += rowTraversal)
            {
                for (var column = 0; column < columnTraversal; column++)
                {
                    currentColumn++;

                    if (currentColumn == inputColumns)
                    {
                        currentColumn = 0;
                    }
                }

                var endingSpot = inputArray[currentRow][currentColumn];

                if (endingSpot == '#')
                {
                    treeCount++;
                }
            }

            return treeCount.ToString();
        }
    }
}
