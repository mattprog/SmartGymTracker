using System.Text.Json.Serialization;
using SmartGymTracker.Metrics.API.Models;
using Library.SmartGymTracker.Models;

namespace SmartGymTracker.Serialization;

[JsonSourceGenerationOptions(
    GenerationMode = JsonSourceGenerationMode.Metadata,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase
)]

[JsonSerializable(typeof(List<Biometrics>))]
[JsonSerializable(typeof(BiometricsResponse))]
[JsonSerializable(typeof(List<WorkoutBiometrics>))]
[JsonSerializable(typeof(WorkoutBiometricsResponse))]
internal partial class BiometricsJsonContext : JsonSerializerContext
{
}