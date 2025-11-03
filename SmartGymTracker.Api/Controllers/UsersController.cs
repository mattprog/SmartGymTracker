using Microsoft.AspNetCore.Mvc;
using SmartGymTracker.Api.Models;
using SmartGymTracker.Api.Services;

namespace SmartGymTracker.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _users;

    public UsersController(IUserService users)
    {
        _users = users;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] UserRegistrationRequest request, CancellationToken ct)
    {
        try
        {
            var user = await _users.RegisterAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ApiMessage(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new ApiMessage(ex.Message));
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var user = await _users.AuthenticateAsync(request, ct);
        if (user is null)
        {
            return Unauthorized(new ApiMessage("Invalid username or password."));
        }

        return Ok(user);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetById(int id, CancellationToken ct)
    {
        var user = await _users.GetAsync(id, ct);
        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDto>> Update(int id, [FromBody] UserUpdateRequest request, CancellationToken ct)
    {
        try
        {
            var updated = await _users.UpdateAsync(id, request, ct);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new ApiMessage(ex.Message));
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var removed = await _users.DeleteAsync(id, ct);
        if (!removed)
        {
            return NotFound();
        }

        return NoContent();
    }
}
