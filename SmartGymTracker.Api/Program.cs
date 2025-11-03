using SmartGymTracker.Api.Models;
using SmartGymTracker.Api.Serialization;
using SmartGymTracker.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IExerciseClient, ExerciseClient>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.TypeInfoResolverChain.Insert(0, ExerciseJsonContext.Default);
});

builder.Services.ConfigureHttpJsonOptions(o =>
{
    o.SerializerOptions.TypeInfoResolverChain.Insert(0, ExerciseJsonContext.Default);
});

// later for inserting to objects
// builder.Services.AddControllers().AddJsonOptions(o =>
// {
//     o.JsonSerializerOptions.TypeInfoResolverChain.Insert(0, ExerciseJsonContext.Default);
// });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

_ = Task.Run(async () =>
{
    using var scope = app.Services.CreateScope();
    var client = scope.ServiceProvider.GetRequiredService<IExerciseClient>();
    try { await client.GetAllAsync(true); } catch { /* ignore */ }
});

app.Run();
