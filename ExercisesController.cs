[ApiController]
[Route("api/[controller]")]
public class ExercisesController(IExerciseService svc) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string? q, [FromQuery] string? muscle,
        [FromQuery] string? equipment, [FromQuery] string? category, CancellationToken ct)
        => Ok(new { data = await svc.SearchAsync(q, muscle, equipment, category, ct) });
}
