using SmartGymTracker.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IExerciseClient, ExerciseClient>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// GET /api/exercises?q=press&muscle=chest&equipment=barbell&category=strength
app.MapGet("/api/exercises", async (IExerciseService svc, string? q, string? muscle, string? equipment, string? category, CancellationToken ct) =>
{
    var data = await svc.SearchAsync(q, muscle, equipment, category, ct);
    return Results.Ok(new { count = data.Count, data });
});

// Optional: warm cache on startup
_ = Task.Run(async () => {
    using var scope = app.Services.CreateScope();
    var client = scope.ServiceProvider.GetRequiredService<IExerciseClient>();
    try { await client.GetAllAsync(true); } catch { /* ignore */ }
});

app.Run();
