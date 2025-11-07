using Microsoft.AspNetCore.Mvc;
using SmartGymTracker.Metrics.API.Services;
using Library.SmartGymTracker.Models;
using SmartGymTracker.Serialization;
using SmartGymTracker.Metrics.API.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IBiometricsClient, BiometricsClient>();
builder.Services.AddHttpClient<IWorkoutBiometricsClient, WorkoutBiometricsClient>();

builder.Services.AddSingleton<IBiometricsService, BiometricsService>();
builder.Services.AddSingleton<IWorkoutBiometricsService, WorkoutBiometricsService>();

builder.Services.ConfigureHttpJsonOptions(o =>
{
    o.SerializerOptions.TypeInfoResolverChain.Insert(0, BiometricsJsonContext.Default);

});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
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

#region Biometrics Endpoints

app.MapGet("/api/biometrics", async (
    IBiometricsService svc,
    [FromQuery] int? userId,
    [FromQuery] DateTime? date,
    CancellationToken ct) =>
{
    var data = await svc.SearchAsync(userId, date, ct);
    return Results.Ok(data);
});

app.MapGet("/api/biometrics/{id:int}", async (
    IBiometricsService svc,
    int id,
    CancellationToken ct) =>
{
    var bio = await svc.GetByIdAsync(id, ct);
    return bio is not null ? Results.Ok(bio) : Results.NotFound();
});

app.MapPost("/api/biometrics", async (
    IBiometricsService svc,
    [FromBody] Biometrics newBio,
    CancellationToken ct) =>
{
    var added = await svc.AddAsync(newBio, ct);
    return added is not null 
        ? Results.Created($"/api/biometrics/{added.BiometricsId}", added) 
        : Results.BadRequest();
});

app.MapPut("/api/biometrics/{id:int}", async (
    IBiometricsService svc,
    int id,
    [FromBody] Biometrics updatedBio,
    CancellationToken ct) =>
{
    if (id != updatedBio.BiometricsId)
        return Results.BadRequest("ID mismatch");

    var updated = await svc.UpdateAsync(updatedBio, ct);
    return updated is not null ? Results.Ok(updated) : Results.NotFound();
});

app.MapDelete("/api/biometrics/{id:int}", async (
    IBiometricsService svc,
    int id,
    CancellationToken ct) =>
{
    var deleted = await svc.DeleteAsync(id, ct);
    return deleted is not null ? Results.Ok(deleted) : Results.NotFound();
});
#endregion

#region WorkoutBiometrics Endpoints

app.MapGet("/api/workout-biometrics", async (
    IWorkoutBiometricsService svc,
    [FromQuery] int? workoutId,
    CancellationToken ct) =>
{
    var data = await svc.SearchAsync(workoutId, ct);
    return Results.Ok(new WorkoutBiometricsResponse(data.Count, data));
});

app.MapGet("/api/workout-biometrics/{id:int}", async (
    IWorkoutBiometricsService svc,
    int id,
    CancellationToken ct) =>
{
    var entry = await svc.GetByIdAsync(id, ct);
    return entry is not null ? Results.Ok(entry) : Results.NotFound();
});

app.MapPost("/api/workout-biometrics", async (
    IWorkoutBiometricsService svc,
    [FromBody] WorkoutBiometrics newEntry,
    CancellationToken ct) =>
{
    var added = await svc.AddAsync(newEntry, ct);
    return added is not null
        ? Results.Created($"/api/workout-biometrics/{added.WorkoutId}", added)
        : Results.BadRequest();
});

app.MapPut("/api/workout-biometrics/{id:int}", async (
    IWorkoutBiometricsService svc,
    int id,
    [FromBody] WorkoutBiometrics updatedEntry,
    CancellationToken ct) =>
{
    if (id != updatedEntry.WorkoutId)
        return Results.BadRequest("ID mismatch");

    var updated = await svc.UpdateAsync(updatedEntry, ct);
    return updated is not null ? Results.Ok(updated) : Results.NotFound();
});

app.MapDelete("/api/workout-biometrics/{id:int}", async (
    IWorkoutBiometricsService svc,
    int id,
    CancellationToken ct) =>
{
    var deleted = await svc.DeleteAsync(id, ct);
    return deleted is not null ? Results.Ok(deleted) : Results.NotFound();
});

#endregion

app.Run();