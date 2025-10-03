using System.Text.Json.Serialization;
using SmartGymTracker.Metrics.API.Models;

namespace SmartGymTracker.Serialization;

[JsonSourceGenerationOptions(
    GenerationMode = JsonSourceGenerationMode.Metadata,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase
)]
[JsonSerializable(typeof(List<Biometrics>))]
[JsonSerializable(typeof(BiometricsResponse))]
[JsonSerializable(typeof(List<ExerciseBiometrics>))]
[JsonSerializable(typeof(ExerciseBiometricsResponse))]
internal partial class BiometricsJsonContext : JsonSerializerContext
{
}