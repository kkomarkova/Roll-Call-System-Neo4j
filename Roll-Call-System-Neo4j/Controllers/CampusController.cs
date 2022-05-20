using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using Roll_Call_System_Neo4j.Models;

namespace Roll_Call_System_Neo4j.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampusController : ControllerBase
    {
        private readonly IGraphClient _client;
        public CampusController(IGraphClient client)
        {
            _client = client;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var trophies = await _client.Cypher.Match("(n: Campus)")
                            .Return(n => n.As<Campus>()).ResultsAsync;
            return Ok(trophies);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCampus([FromBody] Campus campus)
        {
            await _client.Cypher.Create("(n:Campus{name: $name, location: $location, ssid: $ssid})")
                                .WithParam("name", campus.name)
                                .WithParam("location", campus.location)
                                .WithParam("ssid", campus.ssid)
                                .ExecuteWithoutResultsAsync();
            return Ok();
        }
        [HttpPut("{ssid}")]
        public async Task<IActionResult> UpdateCampus(string ssid, [FromBody] Campus campus)
        {
            await _client.Cypher.Match("(c:Campus)")
                                .Where((Campus c) => c.ssid == ssid)
                                .Set("c = $campus")
                                .WithParam("campus", campus)
                                .WithParam("name", campus.name)
                                .WithParam("location", campus.location)
                                .WithParam("ssid", campus.ssid)
                                .ExecuteWithoutResultsAsync();
            return Ok();
        }
        [HttpDelete("{ssid}")]
        public async Task<IActionResult> DeleteCampus(string ssid)
        {
            await _client.Cypher.Match("(c:Campus)")
                                 .Where((Campus c) => c.ssid == ssid)
                                 .Delete("c")
                                 .ExecuteWithoutResultsAsync();
            return Ok();

        }
    }
}
