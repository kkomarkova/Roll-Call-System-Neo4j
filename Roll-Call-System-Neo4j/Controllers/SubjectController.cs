using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using Roll_Call_System_Neo4j.Models;

namespace Roll_Call_System_Neo4j.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly IGraphClient _client;
        public SubjectController(IGraphClient client)
        {
            _client = client;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var subjects = await _client.Cypher.Match("(n: Subject)")
                            .Return(n => n.As<Subject>()).ResultsAsync;
            return Ok(subjects);
        }
        [HttpGet("{name}")]
        public async Task<IActionResult> GetbyName(string name)
        {
            var trophies = await _client.Cypher.Match("(s:Subject)")
                            .Where((Subject s) => s.name == name)
                            .Return(s => s.As<Subject>()).ResultsAsync;
            return Ok(trophies);
        }
        [HttpPost]
        public async Task<IActionResult> CreateSubject([FromBody] Subject subject)
        {
            await _client.Cypher.Create("(n:Subject{name: $name})")
                                .WithParam("name", subject.name)
                                .ExecuteWithoutResultsAsync();
            return Ok();
        }
        [HttpPut("{name}")]
        public async Task<IActionResult> UpdateSubject(string name, [FromBody] Subject subject)
        {
            await _client.Cypher.Match("(s:Subject)")
                                .Where((Subject s) => s.name == name)
                                .Set("s = $subject")
                                .WithParam("subject", subject)
                                .WithParam("name", subject.name)
                                .ExecuteWithoutResultsAsync();
            return Ok();
        }
        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteSubject(string name)
        {
            await _client.Cypher.Match("(s:Subject)")
                                 .Where((Subject s) => s.name == name)
                                 .Delete("s")
                                 .ExecuteWithoutResultsAsync();
            return Ok();

        }

        //[HttpGet]
        //public async Task<IActionResult> GetSubjectswithlessons()
        //{
        //    var subjects = await _client.Cypher.Match("(Lesson)-[r: IS_PART_OF_SUBJECT]->(n:Subject))")
        //     .ReturnDistinct(n => new
        //     {
        //         Subject = n.As<Subject>(),
        //         Lesson = n.As<Lesson>()}).ResultsAsync;
        //    return Ok(subjects);

        //}

    }
    }

