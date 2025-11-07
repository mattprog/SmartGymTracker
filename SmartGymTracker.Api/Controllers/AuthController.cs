using Microsoft.AspNetCore.Mvc;
using SmartGymTracker.Api.Services;
using SmartGymTracker.Api.Models;
using Library.SmartGymTracker.Models;

namespace SmartGymTracker.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserService _svc;

    public AuthController(IUserService svc)
    {
        _svc = svc;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest model, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(model.Username) ||
            string.IsNullOrWhiteSpace(model.Password))
        {
            return BadRequest("Username and password are required.");
        }

        var users = await _svc.SearchAsync(
            null,
            model.Username,
            model.Password,
            null,
            null,
            null,
            null,
            null,
            null,
            ct
        );

        if (users.Count == 0)
            return Unauthorized("Invalid username or password.");

        return Ok(new { message = "Login successful", user = users.First() });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest model, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(model.Username) ||
            string.IsNullOrWhiteSpace(model.Password) ||
            string.IsNullOrWhiteSpace(model.FirstName) ||
            string.IsNullOrWhiteSpace(model.Email))
        {
            return BadRequest("Missing required fields.");
        }

        var result = await _svc.AddUserAsync(
            model.Username,
            model.Password,
            model.Email,
            model.FirstName,
            model.LastName,
            model.PhoneNumber,
            model.DateOfBirth,
            model.Gender,
            ct
        );

        return Ok(result);
    }

    [HttpPost("password-reset")]
    public async Task<IActionResult> PasswordReset([FromBody] PasswordResetRequest model, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            return BadRequest("Email and new password are required.");

        var users = await _svc.SearchAsync(
                    null,
                    null,
                    null,
                    model.Email,
                    null,
                    null,
                    null,
                    null,
                    null,
                    ct
        );

        if (users.Count != 1)
            return Unauthorized("Invalid email address.");
        
        var result = await _svc.UpdatePassword(users[0], model.Password, ct);

        return Ok(new { message = "Password successfully reset." });
    }
}

public class LoginRequest
{
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
}

public class RegisterUserRequest
{
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? DateOfBirth { get; set; }
    public string? Gender { get; set; }
}

public class PasswordResetRequest
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}
