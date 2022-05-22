using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using Neo4j.Driver;
using Neo4jClient;
using Roll_Call_System_Neo4j.Models;

namespace Roll_Call_System_Neo4j.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGraphClient _client;
        public UserController(IGraphClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _client.Cypher.Match("(n: User)")
                            .Return(n => n.As<User>()).ResultsAsync;
            return Ok(users);
        }
        [HttpGet("{email}")]
        public async Task<IActionResult> GetbyEmail(string email)
        {
            var users = await _client.Cypher.Match("(n: User)")
                            .Where((User n) => n.email == email)
                            .Return(n => n.As<User>()).ResultsAsync;
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            await _client.Cypher.Create("(n:User{email: $email, firstName: $firstName, lastName: $lastName, password: $password, salt: $salt})")
                                .WithParam("email", user.email)
                                .WithParam("firstName", user.firstName)
                                .WithParam("lastName", user.lastName)
                                .WithParam("password", user.password)
                                .WithParam("salt", user.salt)
                                .ExecuteWithoutResultsAsync();
            return Ok();
        }
        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            await _client.Cypher.Match("(u:User)")
                                 .Where((User u) => u.email == email)
                                 .Delete("u")
                                 .ExecuteWithoutResultsAsync();
            return Ok();

        }
        [HttpPut("{email}")]
        public async Task<IActionResult> UpdateUser(string email, [FromBody] User user)
        {
            await _client.Cypher.Match("(u:User)")
                                .Where((User u) => u.email == email)
                                .Set("u = $user")
                                .WithParam("user", user)
                                .WithParam("email", user.email)
                                .WithParam("firstName", user.firstName)
                                .WithParam("lastName", user.lastName)
                                .WithParam("password", user.password)
                                .WithParam("salt", user.salt)
                                .ExecuteWithoutResultsAsync();
            return Ok();
        }
    }
}
