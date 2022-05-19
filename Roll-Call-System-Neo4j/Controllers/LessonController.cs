using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using Roll_Call_System_Neo4j.Models;

namespace Roll_Call_System_Neo4j.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly IGraphClient _client;
        public LessonController(IGraphClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var lessons = await _client.Cypher.Match("(n: Lesson)")
                            .Return(n => n.As<Lesson>()).ResultsAsync;
            return Ok(lessons);
        }
        [HttpPost]
        public async Task<IActionResult> CreateLesson([FromBody] Lesson lesson)
        {
            await _client.Cypher.Create("(n:Lesson{id: $id,code: $code, codeTime: $codeTime, startTime: $startTime})")
                                
                                .WithParam("code", lesson.code)
                                .WithParam("codeTime", lesson.codeTime)
                                .WithParam("startTime", lesson.startTime)
                                .WithParam("id", lesson.id)
                                .ExecuteWithoutResultsAsync();
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubject(int id, [FromBody] Lesson lesson)
        {
            await _client.Cypher.Match("(l:Lesson)")
                                .Where((Lesson l) => l.id == id)
                                .Set("l = $lesson")
                                .WithParam("lesson", lesson)
                                .WithParam("startTime", lesson.startTime)
                                .ExecuteWithoutResultsAsync();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            await _client.Cypher.Match("(l:Lesson)")
                                 .Where((Lesson l) => l.id == id)
                                 .Delete("l")
                                 .ExecuteWithoutResultsAsync();
            return Ok();

        }
    }
}
