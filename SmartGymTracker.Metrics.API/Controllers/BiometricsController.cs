using Microsoft.AspNetCore.Mvc;
using Library.SmartGymTracker.Models;
using SmartGymTracker.Metrics.API.Services;
using SmartGymTracker.Metrics.API.Models;

namespace SmartGymTracker.Metrics.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BiometricsController : ControllerBase
{
    private readonly IBiometricsService _svc;

    public BiometricsController(IBiometricsService svc)
    {
        _svc = svc;
    }

    // GET /api/biometrics?userId=1&date=2025-11-01
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] int? userId,
        [FromQuery] DateTime? date,
        CancellationToken ct)
    {
        var data = await _svc.SearchAsync(userId, date, ct);
        return Ok(new BiometricsResponse(data.Count, data));
    }

    // GET /api/biometrics/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var data = await _svc.GetByIdAsync(id, ct);
        if (data == null) return NotFound();
        return Ok(data);
    }

    // POST /api/biometrics
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Biometrics bio, CancellationToken ct)
    {
        var result = await _svc.AddAsync(bio, ct);
        if (result == null) return BadRequest("Failed to add biometrics.");
        return Ok(result);
    }

    // PUT /api/biometrics/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Biometrics bio, CancellationToken ct)
    {
        bio.BiometricsId = id;
        var result = await _svc.UpdateAsync(bio, ct);
        if (result == null) return NotFound();
        return Ok(result);
    }

    // DELETE /api/biometrics/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await _svc.DeleteAsync(id, ct);
        if (result == null) return NotFound();
        return Ok(result);
    }
}