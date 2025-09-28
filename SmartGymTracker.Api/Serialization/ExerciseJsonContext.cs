using System.Text.Json.Serialization;
using SmartGymTracker.Api.Models;

namespace SmartGymTracker.Api.Serialization;

[JsonSourceGenerationOptions(
    GenerationMode = JsonSourceGenerationMode.Metadata,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase
)]
[JsonSerializable(typeof(List<Exercise>))]
[JsonSerializable(typeof(ExercisesResponse))]
internal partial class ExerciseJsonContext : JsonSerializerContext
{
}
