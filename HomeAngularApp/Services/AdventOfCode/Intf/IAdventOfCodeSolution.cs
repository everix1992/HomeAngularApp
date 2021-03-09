using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeAngularApp.Services.AdventOfCode.Intf
{
    // This could potentially be a more generic problem solving interface and then have a separate service that maps names to them
    public interface IAdventOfCodeSolution
    {
        string Name { get; }

        Task<string> ExecuteAsync(IEnumerable<string> input);
    }
}
