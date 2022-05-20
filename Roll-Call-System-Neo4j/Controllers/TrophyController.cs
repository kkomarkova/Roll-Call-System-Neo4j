using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using Roll_Call_System_Neo4j.Models;

namespace Roll_Call_System_Neo4j.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrophyController : ControllerBase
    {
        private readonly IGraphClient _client;
        public TrophyController(IGraphClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _client.Cypher.Match("(n: Trophy)")
                            .Return(n => n.As<Trophy>()).ResultsAsync;
            return Ok(users);
        }
        [HttpGet("{name}")]
        public async Task<IActionResult> GetbyName(string name)
        {
            var trophies = await _client.Cypher.Match("(t:Trophy)")
                            .Where((Trophy t) => t.name == name)
                            .Return(t => t.As<Trophy>()).ResultsAsync;
            return Ok(trophies);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTrophy([FromBody] Trophy trophy)
        {
            await _client.Cypher.Create("(n:Trophy{automatic: $automatic,name: $name})")
                                .WithParam("automatic", trophy.automatic)
                                .WithParam("name", trophy.name)
                                .ExecuteWithoutResultsAsync();
            return Ok();
        }
        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteTrophy(string name)
        {
            await _client.Cypher.Match("(t:Trophy)")
                                 .Where((Trophy t) => t.name == name)
                                 .Delete("t")
                                 .ExecuteWithoutResultsAsync();
            return Ok();

        }
        [HttpPut("{name}")]
        public async Task<IActionResult> UpdateTrophy(string name, [FromBody] Trophy trophy)
        {
            await _client.Cypher.Match("(t:Trophy)")
                                .Where((Trophy t) => t.name == name)
                                .Set("t = $trophy")
                                .WithParam("trophy", trophy)
                                .WithParam("name", trophy.name)
                                .ExecuteWithoutResultsAsync();
            return Ok();
        }
    }
}
