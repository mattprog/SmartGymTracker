using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartGymTracker.Api.Models
{
    public sealed class UserDto
    {
        public int Id { get; init; }
        public string Username { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string? PhoneNumber { get; init; }
        public DateTime? DateOfBirth { get; init; }
        public string? Gender { get; init; }
        public int? Height { get; init; }
        public int? Weight { get; init; }
        public bool IsAdmin { get; init; }
    }

    public sealed class UserRegistrationRequest
    {
        [Required]
        public string Username { get; init; } = string.Empty;

        [Required]
        public string Password { get; init; } = string.Empty;

        [Required]
        public string FirstName { get; init; } = string.Empty;

        [Required]
        public string LastName { get; init; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; init; } = string.Empty;

        public string? PhoneNumber { get; init; }
        public DateTime? DateOfBirth { get; init; }
        public string? Gender { get; init; }
        public int? Height { get; init; }
        public int? Weight { get; init; }
        public bool IsAdmin { get; init; }
    }

    public sealed class UserUpdateRequest
    {
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? Email { get; init; }
        public string? PhoneNumber { get; init; }
        public DateTime? DateOfBirth { get; init; }
        public string? Gender { get; init; }
        public int? Height { get; init; }
        public int? Weight { get; init; }
        public bool? IsAdmin { get; init; }
    }

    public sealed class LoginRequest
    {
        [Required]
        public string Username { get; init; } = string.Empty;

        [Required]
        public string Password { get; init; } = string.Empty;
    }

    public sealed class ResetPasswordRequest
    {
        [Required]
        public string NewPassword { get; init; } = string.Empty;
    }

    public sealed class UsersResponse
    {
        public UsersResponse(int count, IReadOnlyList<UserDto> data)
        {
            Count = count;
            Data = data;
        }

        public int Count { get; }
        public IReadOnlyList<UserDto> Data { get; }
    }
}
