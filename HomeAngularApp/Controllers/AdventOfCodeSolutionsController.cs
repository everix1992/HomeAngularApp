using System;
using System.Collections.Concurrent;
using HomeAngularApp.Services.AdventOfCode.Intf;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [HttpPost("GetSolution")]
        public async Task<ActionResult<string>> GetSolutionAsync(GetSolutionModel model)
        {
            // TODO: Consider splitting the input out into a POST call before this call
            var found = _solutions.TryGetValue(model.SolutionName, out var solution);

            if (!found)
            {
                return NotFound();
            }
            
            var inputLines = model.Input.Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.RemoveEmptyEntries);
            return (await solution.ExecuteAsync(inputLines));
        }
    }

    public class GetSolutionModel
    {
        [Required]
        public string Input { get; set; }

        [Required]
        public string SolutionName { get; set; }
    }
}