using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using SmartGymTracker.Api.Controllers;

namespace SmartGymTracker.Api.Serialization;

[JsonSourceGenerationOptions(
    GenerationMode = JsonSourceGenerationMode.Metadata,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(ProblemDetails))]
[JsonSerializable(typeof(ValidationProblemDetails))]
[JsonSerializable(typeof(LoginRequest))]
[JsonSerializable(typeof(RegisterUserRequest))]
[JsonSerializable(typeof(PasswordResetRequest))]
internal partial class AuthJsonContext : JsonSerializerContext
{
}