using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HomeAngularApp.Services.AdventOfCode.Intf;

namespace HomeAngularApp.Services.AdventOfCode.Impl
{
    public class PasswordPhilosophy : IAdventOfCodeSolution
    {
        public string Name => "Day 2: Password Philosophy";

        public async Task<string> ExecuteAsync(IEnumerable<string> input)
        {
            var regex = new Regex(@"(\d+)\s*-\s*(\d+)\s+([a-zA-Z])\s*:\s*(\w+)");

            var validPasswordCount = 0;
            foreach (var line in input)
            {
                var match = regex.Match(line);

                if (!match.Success)
                {
                    throw new NotImplementedException("Missing regex support for this line of input: " + line);
                }

                // TODO: Handle bad input
                var rangeStart = int.Parse(match.Groups[1].Value);
                var rangeEnd = int.Parse(match.Groups[2].Value);
                var character = match.Groups[3].Value.Trim()[0];
                var password = match.Groups[4].Value;

                var charCount = password.Count(c => c == character);

                if (charCount >= rangeStart && charCount <= rangeEnd)
                {
                    validPasswordCount++;
                }
            }

            return validPasswordCount.ToString();
        }
    }
}
