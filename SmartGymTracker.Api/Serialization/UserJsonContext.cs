using System.Text.Json.Serialization;
using Library.SmartGymTracker.Models;
using SmartGymTracker.Api.Models;

namespace SmartGymTracker.Api.Serialization;

[JsonSourceGenerationOptions(
    GenerationMode = JsonSourceGenerationMode.Metadata,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase
)]
[JsonSerializable(typeof(List<User>))]
[JsonSerializable(typeof(User))]
[JsonSerializable(typeof(UserResponse))]
[JsonSerializable(typeof(UserRequest))]
[JsonSerializable(typeof(UpdateUserRequest))]

internal partial class UserJsonContext : JsonSerializerContext
{
}
