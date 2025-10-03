using Microsoft.AspNetCore.Mvc;
using SmartGymTracker.Metrics.API.Models;
using SmartGymTracker.Metrics.API.Services;
using SmartGymTracker.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IBiometricsClient, BiometricsClient>();
builder.Services.AddHttpClient<IExerciseBiometricsClient, ExerciseBiometricsClient>();

builder.Services.AddSingleton<IBiometricsService, BiometricsService>();
builder.Services.AddSingleton<IExerciseBiometricsService, ExerciseBiometricsService>();

builder.Services.ConfigureHttpJsonOptions(o =>
{
    o.SerializerOptions.TypeInfoResolverChain.Insert(0, BiometricsJsonContext.Default);

});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Allowall", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5153")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/api/biometrics", async (
    IBiometricsService svc,
    [FromQuery] int? userId,
    [FromQuery] DateTime? date,
    CancellationToken ct) =>
{
    var data = await svc.SearchAsync(userId, date, ct);
    return Results.Ok(data);
});

app.MapPost("/api/biometrics", async (
    IBiometricsService svc,
    [FromBody]Biometrics newBio,
    CancellationToken ct) =>
{
    Console.WriteLine($"Received POST: {System.Text.Json.JsonSerializer.Serialize(newBio)}");
    await svc.AddAsync(newBio, ct);
    return Results.Created($"/api/biometrics/{newBio.biometricsId}", newBio);
});

app.MapGet("/api/exercise-biometrics", async (
    IExerciseBiometricsService svc,
    [FromQuery] int? workoutId,
    CancellationToken ct) =>
{
    var data = await svc.SearchAsync(workoutId, ct);
    return Results.Ok(new ExerciseBiometricsResponse(data.Count, data));
});

app.MapPost("/api/exercise-biometrics", async (
    IExerciseBiometricsService svc,
    [FromBody] ExerciseBiometrics newEntry,
    CancellationToken ct) =>
{
    await svc.AddAsync(newEntry, ct);
    return Results.Created($"/api/exercise-biometrics/{newEntry.exerciseBiometricId}", newEntry);
});

app.Run();