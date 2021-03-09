using System.Collections.Concurrent;
using HomeAngularApp.Services.AdventOfCode.Intf;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAngularApp.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AdventOfCodeSolutionsController : ControllerBase
    {
        private readonly IDictionary<string, IAdventOfCodeSolution> _solutions;

        public AdventOfCodeSolutionsController(IEnumerable<IAdventOfCodeSolution> adventOfCodeSolutions)
        {
            _solutions = adventOfCodeSolutions.ToDictionary(s => s.Name, s => s);
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _solutions.Keys;
        }

        [HttpGet("{problemName}")]
        public async Task<ActionResult<string>> GetSolutionAsync(string problemName)
        {
            var found = _solutions.TryGetValue(problemName, out var solution);

            if (!found)
            {
                return NotFound();
            }

            return (await solution.ExecuteAsync(null));
        }
    }
}