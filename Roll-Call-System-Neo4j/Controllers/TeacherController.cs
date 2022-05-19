using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using Roll_Call_System_Neo4j.Models;

namespace Roll_Call_System_Neo4j.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly IGraphClient _client;
        public TeacherController(IGraphClient client)
        {
            _client = client;
        }
        [HttpGet("Teachers")]
        public async Task<IActionResult> GetTeachers()
        {

            var teachers = await _client.Cypher.Match("(Role{name:'Teacher'})<-[:HAS_ROLE]-(n:User)")
                                 .Return(n => n.As<User>()).ResultsAsync;

            return Ok(teachers);
        }
    }
}
