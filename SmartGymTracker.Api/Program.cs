using SmartGymTracker.Api.Models;
using SmartGymTracker.Api.Serialization;
using SmartGymTracker.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// --- DI & config (must be BEFORE Build) ---
builder.Services.AddHttpClient<IExerciseClient, ExerciseClient>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();

// System.Text.Json source-gen context for minimal APIs
builder.Services.ConfigureHttpJsonOptions(o =>
{
    o.SerializerOptions.TypeInfoResolverChain.Insert(0, ExerciseJsonContext.Default);
});

// If you’re using controllers too, keep this and map them later:
// builder.Services.AddControllers().AddJsonOptions(o =>
// {
//     o.JsonSerializerOptions.TypeInfoResolverChain.Insert(0, ExerciseJsonContext.Default);
// });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// -------------------------------------------

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Minimal API endpoint
app.MapGet("/api/exercises", async (
    IExerciseService svc,
    string? q,
    string? muscle,
    string? equipment,
    string? category,
    CancellationToken ct) =>
{
    var data = await svc.SearchAsync(q, muscle, equipment, category, ct);
    return Results.Ok(new ExercisesResponse(data.Count, data));
});

// If you enabled controllers above, also map them here:
// app.MapControllers();

// Optional: warm cache on startup
_ = Task.Run(async () =>
{
    using var scope = app.Services.CreateScope();
    var client = scope.ServiceProvider.GetRequiredService<IExerciseClient>();
    try { await client.GetAllAsync(true); } catch { /* ignore */ }
});

app.Run();