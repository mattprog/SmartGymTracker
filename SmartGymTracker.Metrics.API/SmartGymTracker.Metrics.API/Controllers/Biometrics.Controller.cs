using Microsoft.AspNetCore.Mvc;
using SmartGymTracker.Metrics.API.Models;
using SmartGymTracker.Metrics.API.Services;

namespace SmartGymTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BiometricsController : ControllerBase
{
    private readonly IBiometricsService _svc;

    public BiometricsController(IBiometricsService svc)
    {
        _svc = svc; 
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] int? userId,
        [FromQuery] DateTime? date,
        CancellationToken ct)

    {
        var data = await _svc.SearchAsync(userId, date, ct);
        return Ok(new {count = data.Count, data});
    }
}