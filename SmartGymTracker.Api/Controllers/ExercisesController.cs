using Microsoft.AspNetCore.Mvc;
using SmartGymTracker.Api.Models;
using SmartGymTracker.Api.Services;

namespace SmartGymTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExercisesController : ControllerBase
{
    private readonly IExerciseService _svc;

    public ExercisesController(IExerciseService svc)
    {
        _svc = svc; // if this shows "out of scope", check the using above + DI registration in Program.cs
    }

    // GET api/exercises?q=press&muscle=chest&equipment=barbell&category=strength
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] string? q,
        [FromQuery] string? muscle,
        [FromQuery] string? equipment,
        [FromQuery] string? category,
        CancellationToken ct)
    {
        var data = await _svc.SearchAsync(q, muscle, equipment, category, ct);
        return Ok(new { count = data.Count, data });
    }
}
