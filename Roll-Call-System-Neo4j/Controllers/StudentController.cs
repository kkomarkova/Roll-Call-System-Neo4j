﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using Roll_Call_System_Neo4j.Models;

namespace Roll_Call_System_Neo4j.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IGraphClient _client;
        public StudentController(IGraphClient client)
        {
            _client = client;
        }

        [HttpGet("Students")]
        public async Task<IActionResult> GetStudents()
        {

            var students = await _client.Cypher.Match("(Role{name:'Student'})<-[:HAS_ROLE]-(n:User)")
                                 .Return(n => n.As<User>()).ResultsAsync;

            return Ok(students);
        }

    }
}