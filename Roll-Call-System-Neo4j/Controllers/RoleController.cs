using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using Roll_Call_System_Neo4j.Models;

namespace Roll_Call_System_Neo4j.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IGraphClient _client;
        public RoleController(IGraphClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var roles = await _client.Cypher.Match("(n: Role)")
                            .Return(n => n.As<Role>()).ResultsAsync;
            return Ok(roles);
        }
        [HttpGet("{name}")]
        public async Task<IActionResult> GetbyName(string name)
        {
            var roles = await _client.Cypher.Match("(r:Role)")
                            .Where((Role r) => r.name == name)
                            .Return(r => r.As<Role>()).ResultsAsync;
            return Ok(roles);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] Role role)
        {
            await _client.Cypher.Create("(n:Role{name: $name})")
                                .WithParam("name", role.name)
                                .ExecuteWithoutResultsAsync();
            return Ok();
        }
        [HttpPut("{name}")]
        public async Task<IActionResult> UpdateRole(string name, [FromBody] Role role)
        {
            await _client.Cypher.Match("(r:Role)")
                                .Where((Role r) => r.name == name)
                                .Set("r = $role")
                                .WithParam("role", role)
                                .WithParam("name", role.name)
                                .ExecuteWithoutResultsAsync();
            return Ok();
        }
        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteRole(string name)
        {
            await _client.Cypher.Match("(r:Role)")
                                 .Where((Role r) => r.name == name)
                                 .Delete("r")
                                 .ExecuteWithoutResultsAsync();
            return Ok();

        }
    }
}

