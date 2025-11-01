using System.Text;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Options;
//using SmartGymTracker.Api.Models;
//using SmartGymTracker.Api.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;


using System.Threading.Tasks;
using SmartGymTracker.Api.Models;
using Library.SmartGymTracker.Models;
using SmartGymTracker.Api.Services;

namespace SmartGymTracker.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    //[HttpPost("login")]
    //public IActionResult Login([FromBody] LoginRequest request)
    //{

    public class ApplicationUser : IdentityUser
    {
        // Additional properties can be added here if needed
        public string? Phone { get; internal set; }
        public string? BirthDate { get; internal set; }
        public int Weight { get; internal set; }
        public int Height { get; internal set; }
        public string? FirstName { get; internal set; }
        public string? LastName { get; internal set; }
    }

    private readonly IUserService _svc;

    private readonly SignInManager<ApplicationUser> _LoginManager;

    public UserController(SignInManager<ApplicationUser> LoginManager,IUserService svc)
    {
        _LoginManager = LoginManager;
        _svc = svc;
    }


    [HttpPost]
    //[HttpGet]
    public async Task<IActionResult> Post(
        string? UserId,
        string? username,
        string? password,
        string? email,
        string firstname,
        string? lastname,
        string? phone_number,
        string? dateofbirth,
        string? weight,
        string? height,
        string? gender,
        CancellationToken ct)
    {
        var data = await _svc.SearchAsync(UserId, username, password, email, firstname, lastname, phone_number, dateofbirth,
            gender, ct);
        return Ok(new { count = data.Count, data });
    }
    public async Task<IActionResult> Login(User model)
    {
        var result = await _LoginManager.PasswordSignInAsync(model.Username, model.Password, false, false);
        if (result.Succeeded)
        {
            return Ok("Login successful");
        }
        return Unauthorized("Invalid email or password");
        // Replace with your user validation logic (e.g., check against a database)
        //if (request.Email == "user@gmail.com" && request.Password == "password")
        //{
        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.Name, request.Email)
        //    };

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yourSecretKey"));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        issuer: "yourIssuer",
        //        audience: "yourAudience",
        //        claims: claims,
        //        expires: DateTime.Now.AddHours(1),
        //        signingCredentials: creds);
        //    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        //    // Return the token as JSON
        //    return Ok(new { token = tokenString });
        //}

        //return Unauthorized();
    }
};


