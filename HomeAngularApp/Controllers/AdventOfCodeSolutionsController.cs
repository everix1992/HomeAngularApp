using HomeAngularApp.Services.AdventOfCode.Intf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
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
        // TODO: Need to garbage collect 
        private static readonly ConcurrentDictionary<string, string> _input = new ConcurrentDictionary<string, string>();
        private readonly IDictionary<int, IAdventOfCodeSolution> _solutions;

        public AdventOfCodeSolutionsController(IEnumerable<IAdventOfCodeSolution> adventOfCodeSolutions)
        {
            _solutions = new Dictionary<int, IAdventOfCodeSolution>();

            var id = 0;
            foreach (var solution in adventOfCodeSolutions)
            {
                _solutions[id++] = solution;
            }
        }

        [HttpGet]
        public IEnumerable<SolutionViewModel> Get()
        {
            return _solutions.Select(solution => new SolutionViewModel {Id = solution.Key, Name = solution.Value.Name});
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<string>> GetSolutionAsync(int id, string inputId)
        {
            var found = _solutions.TryGetValue(id, out var solution);

            if (!found)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(inputId))
            {
                return BadRequest("Input ID is required");
            }

            var foundInput = _input.TryGetValue(inputId, out var input);

            if (!foundInput)
            {
                return BadRequest("Invalid Input ID");
            }
            
            var inputLines = input.Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.RemoveEmptyEntries);
            return (await solution.ExecuteAsync(inputLines));
        }

        [HttpPost("input")]
        public ActionResult<string> UploadInput([FromBody]string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return BadRequest("Input is empty.");
            }

            var fileIdentifier = GetInputIdentifier();
            _input[fileIdentifier] = input;

            return fileIdentifier;
        }

        [HttpPost("input-file")]
        public async Task<ActionResult<string>> UploadInputFile(IFormFile file)
        {
            var fileIdentifier = GetInputIdentifier();
            var text = await file.ReadAsStringAsync();

            _input[fileIdentifier] = text;

            return fileIdentifier;
        }

        private static string GetInputIdentifier()
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }
    }

    // TODO: Better names for this class?
    public class SolutionViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}