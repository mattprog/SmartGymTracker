using System.Text.Json.Serialization;
using SmartGymTracker.Api.Models;

namespace SmartGymTracker.Api.Serialization;

[JsonSourceGenerationOptions(
    GenerationMode = JsonSourceGenerationMode.Metadata,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase
)]
[JsonSerializable(typeof(List<User>))]
[JsonSerializable(typeof(UserResponse))]
internal partial class UserJsonContext : JsonSerializerContext
{
}
