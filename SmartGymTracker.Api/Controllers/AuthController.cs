using System.Text;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Options;
//using SmartGymTracker.Api.Models;
//using SmartGymTracker.Api.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.Data;

namespace SmartGymTracker.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Replace with your user validation logic (e.g., check against a database)
        if (request.Email == "user@gmail.com" && request.Password == "password")
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, request.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yourSecretKey"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yourIssuer",
                audience: "yourAudience",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // Return the token as JSON
            return Ok(new { token = tokenString });
        }

        return Unauthorized();
    }
};


