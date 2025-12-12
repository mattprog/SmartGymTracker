using System.Text.Json.Serialization;
using Library.SmartGymTracker.Models;
using SmartGymTracker.Api.Models;

namespace SmartGymTracker.Api.Serialization;

[JsonSourceGenerationOptions(
    GenerationMode = JsonSourceGenerationMode.Metadata,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase
)]
[JsonSerializable(typeof(List<User>))]
[JsonSerializable(typeof(UserResponse))]
[JsonSerializable(typeof(SmartGymTracker.Api.Controllers.RegisterUserRequest))]
[JsonSerializable(typeof(SmartGymTracker.Api.Controllers.LoginRequest))]
[JsonSerializable(typeof(SmartGymTracker.Api.Controllers.UpdateUserRequest))]
[JsonSerializable(typeof(SmartGymTracker.Api.Controllers.PasswordResetRequest))]
[JsonSerializable(typeof(SmartGymTracker.Api.Controllers.AuthResponse))]
[JsonSerializable(typeof(WorkoutsResponse))]
[JsonSerializable(typeof(MusclesResponse))]
[JsonSerializable(typeof(Library.SmartGymTracker.Models.Workout))]
[JsonSerializable(typeof(List<Library.SmartGymTracker.Models.Workout>))]
[JsonSerializable(typeof(Library.SmartGymTracker.Models.Muscle))]
[JsonSerializable(typeof(List<Library.SmartGymTracker.Models.Muscle>))]
internal partial class UserJsonContext : JsonSerializerContext
{
}
