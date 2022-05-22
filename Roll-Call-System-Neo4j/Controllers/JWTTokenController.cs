using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Neo4jClient;
using Roll_Call_System_Neo4j.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Roll_Call_System_Neo4j.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JWTTokenController : ControllerBase
{
    public IConfiguration _configuration;
    private readonly IGraphClient _client;

    public JWTTokenController(IConfiguration configuration, IGraphClient client)
    {
        _configuration = configuration;
        _client = client;
    }

    [HttpPost]
    public async Task<IActionResult> Post(LoginUser loginUser)
    {
        if (loginUser != null && loginUser.Email != null && loginUser.Password != null)
        {
            User user = new User()
            {
                email = loginUser.Email,
                password = loginUser.Password,
            };

            string? salt = await GetSalt(user.email);
            if (salt == null) return NotFound();
            user.password = HashPasswordWithSalt(salt, loginUser.Password);

            var userData = await GetUser(user.email, user.password);
            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();
            if (userData != null)
            {
                var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("Id", userData.id.ToString()),
                new Claim("Email", userData.email),
                new Claim("Password", userData.password)
            };

                //Get all roles from the database (i.e. Teacher, Student)
                var roles = new List<string>()
            {
                "Student", "Teacher", "Admin"
            };
                if (roles == null) return NoContent();

                try
                {
                    ////Get the first role from my role list which I got above, that matches the roleId of the user that has logged in
                    //claims.Add(new Claim(ClaimTypes.Role, roles.FirstOrDefault(x => x == userData.Role)));

                    ////For ease of access (I don't know how to access the named ones set above lol), I store the user Id in the name type since we're not using that one
                    ////If anyone figures out how to access the Claim("Id") later, please let me know <3 <3
                    //claims.Add(new Claim(ClaimTypes.Name, userData.Id.ToString()));
                }
                catch
                {
                    return BadRequest("Invalid Credentials");
                }


                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.key));
                var signin = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    jwt.Issuer,
                    jwt.Audience,
                    claims.ToArray(),
                    expires: DateTime.Now.AddMinutes(20),
                    signingCredentials: signin
                );
                return Ok((new JwtSecurityTokenHandler().WriteToken(token)));
            }
            else
            {
                return BadRequest("Invalid Credentials");
            }
        }

        else
        {
            return BadRequest("Invalid Credentials");
        }
    }

    private async Task<User?> GetUser(string userEmail, string userPassword)
    {
        var result = await _client.Cypher.Match("(n: User)")
                            .Where((User n) => n.email == userEmail && n.password == userPassword)
                            .Return(n => n.As<User>()).ResultsAsync;

        return result.FirstOrDefault();
    }

    private async Task<string?> GetSalt(string userEmail)
    {
        var result = await _client.Cypher.Match("(n: User)")
                            .Where((User n) => n.email == userEmail)
                            .Return(n => n.As<User>().salt).ResultsAsync;

        return result.FirstOrDefault();
    }

    private string HashPasswordWithSalt(string salt, string password)
    {
        byte[] saltByte = Encoding.UTF8.GetBytes(salt);
        password += "pepper123";
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: saltByte,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 1000,
            numBytesRequested: 32));
        Console.WriteLine(hashed);
        return hashed;
    }
}


