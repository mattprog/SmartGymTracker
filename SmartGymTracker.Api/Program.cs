using System.Reflection;
using Library.SmartGymTracker.Models;
using Microsoft.AspNetCore.Mvc;
using SmartGymTracker.Api.Models;
using SmartGymTracker.Api.Serialization;
using SmartGymTracker.Api.Services;
using SmartGymTracker.Api.Controllers; // needed for AddApplicationPart

AppContext.SetSwitch("Microsoft.AspNetCore.Mvc.ApiExplorer.IsEnhancedModelMetadataSupported", true);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IExerciseClient, ExerciseClient>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();

builder.Services.AddHttpClient<IUserClient, UserClient>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IWorkoutService, WorkoutService>();
builder.Services.AddSingleton<IMuscleService, MuscleService>();

builder.Services.ConfigureHttpJsonOptions(o =>
{
    o.SerializerOptions.TypeInfoResolverChain.Insert(0, ExerciseJsonContext.Default);
    o.SerializerOptions.TypeInfoResolverChain.Insert(0, UserJsonContext.Default);
});

builder.Services
    .AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.TypeInfoResolverChain.Insert(0, ExerciseJsonContext.Default);
        o.JsonSerializerOptions.TypeInfoResolverChain.Insert(0, UserJsonContext.Default);
    })
    .AddApplicationPart(typeof(AuthController).Assembly)
    .AddControllersAsServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5153", "http://localhost:5074")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();

app.MapControllerRoute(
    name: "auth",
    pattern: "api/auth/{action=Index}/{id?}");

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

app.MapGet("/api/user", async (
    IUserService svc,
    string? UserId,
    string? username,
    string? password,
    string? email,
    string firstname,
    string? lastname,
    string phone_number,
    string? dateofbirth,
    string? gender,
    CancellationToken ct) =>
{
    var data = await svc.SearchAsync(UserId, username, password, email, firstname, lastname, phone_number, dateofbirth,
             gender, ct);
    return Results.Ok(new UserResponse(data.Count, data));
});

app.MapGet("/api/user/{UserId}", async (
    IUserService svc,
    int UserId,
    CancellationToken ct) =>
{
    var data = await svc.GetUserAsync(UserId, ct);
    return Results.Ok(data);
});

app.MapPost("/api/user", async (
    IUserService svc,
    string? username,
    string? password,
    string? email,
    string firstname,
    string? lastname,
    string phone_number,
    string? dateofbirth,
    string? gender,
    CancellationToken ct) =>
{
    var data = await svc.AddUserAsync(username, password, email, firstname, lastname, phone_number, dateofbirth, gender, ct);
    return Results.Ok(data);
});

app.MapPut("/api/user/{UserId}", async (
    IUserService svc,
    int UserId,
    [FromBody] UpdateUserRequest model,
    CancellationToken ct) =>
{
    var data = await svc.UpdateUserAsync(UserId, model.Username, model.Password, model.Email, model.FirstName ?? string.Empty, model.LastName, model.PhoneNumber ?? string.Empty, model.DateOfBirth, model.Gender, ct);
    return Results.Ok(data);
});

app.MapDelete("/api/user/{UserId}", async (
    IUserService svc,
    int UserId,
    CancellationToken ct) =>
{
    var data = await svc.DeleteUserAsync(UserId, ct);
    return Results.Ok(data);
});

_ = Task.Run(async () =>
{
    using var scope = app.Services.CreateScope();

    var exerciseClient = scope.ServiceProvider.GetRequiredService<IExerciseClient>();
    var userClient = scope.ServiceProvider.GetRequiredService<IUserClient>();

    try
    {
        await exerciseClient.GetAllAsync(true);
        await userClient.GetAllAsync(true);
    }
    catch { /* ignore */ }
});

await SeedMusclesAsync(app.Services);

app.Run();

static async Task SeedMusclesAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var muscleService = scope.ServiceProvider.GetRequiredService<IMuscleService>();
    var existing = await muscleService.GetAllAsync();
    if (existing.Count > 0)
        return;

    var defaults = new[]
    {
        new Muscle { Name = "Chest", Description = "desc" },
        new Muscle { Name = "Back", Description = "desc" },
        new Muscle { Name = "Legs", Description = "desc" },
        new Muscle { Name = "Arms", Description = "desc" },
        new Muscle { Name = "Shoulders", Description = "desc" },
        new Muscle { Name = "Core", Description = "desc" },
        new Muscle { Name = "Glutes", Description = "desc" },
        new Muscle { Name = "Calves", Description = "desc" },
        new Muscle { Name = "Traps", Description = "desc" },
        new Muscle { Name = "Forearms", Description = "desc" }
    };

    foreach (var muscle in defaults)
    {
        await muscleService.AddAsync(muscle);
    }
}
