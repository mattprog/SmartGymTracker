using Library.SmartGymTracker.Models;
using Microsoft.AspNetCore.Mvc;
using SmartGymTracker.Api.Models;
using SmartGymTracker.Api.Services;

namespace SmartGymTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkoutsController : ControllerBase
{
    private readonly IWorkoutService _svc;

    public WorkoutsController(IWorkoutService svc)
    {
        _svc = svc;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int? userId, CancellationToken ct)
    {
        var data = await _svc.GetAllAsync(userId, ct);
        return Ok(new WorkoutsResponse(data.Count, data));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var workout = await _svc.GetByIdAsync(id, ct);
        return workout is not null ? Ok(workout) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Workout workout, CancellationToken ct)
    {
        var added = await _svc.AddAsync(workout, ct);
        if (added is null)
            return BadRequest("Unable to create workout.");

        return CreatedAtAction(nameof(GetById), new { id = added.WorkoutId }, added);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Workout workout, CancellationToken ct)
    {
        if (id != workout.WorkoutId)
            return BadRequest("ID mismatch.");

        var updated = await _svc.UpdateAsync(workout, ct);
        return updated is not null ? Ok(updated) : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var deleted = await _svc.DeleteAsync(id, ct);
        return deleted is not null ? Ok(deleted) : NotFound();
    }
}
