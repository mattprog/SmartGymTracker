using System.Text.Json.Serialization;
using SmartGymTracker.Api.Models;

namespace SmartGymTracker.Api.Serialization;

[JsonSourceGenerationOptions(
    GenerationMode = JsonSourceGenerationMode.Metadata,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase
)]
[JsonSerializable(typeof(List<Exercise>))]
[JsonSerializable(typeof(ExercisesResponse))]
[JsonSerializable(typeof(UserDto))]
[JsonSerializable(typeof(List<UserDto>))]
[JsonSerializable(typeof(UsersResponse))]
[JsonSerializable(typeof(ApiMessage))]
[JsonSerializable(typeof(UserRegistrationRequest))]
[JsonSerializable(typeof(UserUpdateRequest))]
[JsonSerializable(typeof(LoginRequest))]
[JsonSerializable(typeof(ResetPasswordRequest))]
[JsonSerializable(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails))]
[JsonSerializable(typeof(Microsoft.AspNetCore.Mvc.ValidationProblemDetails))]
internal partial class ExerciseJsonContext : JsonSerializerContext
{
}
