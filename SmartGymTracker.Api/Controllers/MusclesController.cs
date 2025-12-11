using Library.SmartGymTracker.Models;
using Microsoft.AspNetCore.Mvc;
using SmartGymTracker.Api.Models;
using SmartGymTracker.Api.Services;

namespace SmartGymTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MusclesController : ControllerBase
{
    private readonly IMuscleService _svc;

    public MusclesController(IMuscleService svc)
    {
        _svc = svc;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var data = await _svc.GetAllAsync(ct);
        return Ok(new MusclesResponse(data.Count, data));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var muscle = await _svc.GetByIdAsync(id, ct);
        return muscle is not null ? Ok(muscle) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Muscle muscle, CancellationToken ct)
    {
        var added = await _svc.AddAsync(muscle, ct);
        if (added is null)
            return BadRequest("Unable to create muscle.");

        return CreatedAtAction(nameof(GetById), new { id = added.MuscleId }, added);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Muscle muscle, CancellationToken ct)
    {
        if (id != muscle.MuscleId)
            return BadRequest("ID mismatch.");

        var updated = await _svc.UpdateAsync(muscle, ct);
        return updated is not null ? Ok(updated) : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var deleted = await _svc.DeleteAsync(id, ct);
        return deleted is not null ? Ok(deleted) : NotFound();
    }
}
