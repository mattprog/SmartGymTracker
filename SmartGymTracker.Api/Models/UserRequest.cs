namespace SmartGymTracker.Api.Models;

public record UserRequest(
    string Username,
    string Password,
    string Email,
    string FirstName,
    string? LastName,
    string PhoneNumber,
    string DateOfBirth,
    string? Gender
);

public record UpdateUserRequest(
    string? Username,
    string? Password,
    string? Email,
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string? DateOfBirth,
    string? Gender
);


