using Microsoft.AspNetCore.Mvc;
using SmartGymTracker.Api.Models;
using SmartGymTracker.Api.Services;

namespace SmartGymTracker.Api.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IUserService _users;

    public AdminController(IUserService users)
    {
        _users = users;
    }

    [HttpGet("users")]
    public async Task<ActionResult<UsersResponse>> GetUsers(CancellationToken ct)
    {
        var users = await _users.ListAsync(ct);
        return Ok(new UsersResponse(users.Count, users));
    }

    [HttpPost("users/{id:int}/reset-password")]
    public async Task<IActionResult> ResetPassword(int id, [FromBody] ResetPasswordRequest request, CancellationToken ct)
    {
        var success = await _users.ResetPasswordAsync(id, request, ct);
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}
