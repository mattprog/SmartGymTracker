using System.Data;
using System.Net.Http.Json;
using SmartGymTracker.Api.Models;
using Library.SmartGymTracker.Models;
using SmartGymTracker.Api.Serialization;
using MySQL.SmartGymTracker;
using System.Reflection;
namespace SmartGymTracker.Api.Services;


    public interface IUserClient
    {
        Task<IReadOnlyList<User>> GetAllAsync(bool forceReload = false, CancellationToken ct = default);
        Task<User> GetUserAsync(int UserId, bool forceReload = false, CancellationToken ct = default);

        Task<User> AddUserAsync(string? username, string? password, string? email, string? firstname, string? lastname, string? phone_number, string? dateofbirth,
          string? gender, bool forceReload = false, CancellationToken ct = default);

        Task<User> UpdateUserAsync(int UserId, string? username, string? password, string? email, string? firstname, string? lastname, string? phone_number, string? dateofbirth,
          string? gender, bool forceReload = false, CancellationToken ct = default);

        Task<User> DeleteUserAsync(int UserId, bool forceReload = false, CancellationToken ct = default);
}
public sealed class UserClient(HttpClient http) : IUserClient
{

    private IReadOnlyList<User>? _cache;
    private DateTimeOffset _cachedAt = DateTimeOffset.MinValue;
    private static readonly TimeSpan CacheTtl = TimeSpan.FromHours(12);
    
    public async Task<IReadOnlyList<User>> GetAllAsync(bool forceReload = false, CancellationToken ct = default)
    {
        if (!forceReload && _cache is not null && DateTimeOffset.UtcNow - _cachedAt < CacheTtl)
            return _cache;
        var db = new User_DB();
        var data = db.GetAll();
        _cache = data;
        _cachedAt = DateTimeOffset.UtcNow;
        return _cache;
    }
    public async Task<User> GetUserAsync(int UserId, bool forceReload = false, CancellationToken ct = default)
    {
        var db = new User_DB();
        var data = db.GetById(UserId);
        return data;
    }
    public async Task<User> AddUserAsync(string? username, string? password, string? email, string? firstname, string? lastname, string? phone_number, string? dateofbirth,
          string? gender, bool forceReload = false, CancellationToken ct = default)
    {
        var user = new User();
        user.Username = username ?? string.Empty;
        user.Password = password ?? string.Empty;
        user.Email = email ?? string.Empty;
        user.FirstName = firstname ?? string.Empty; 
        user.LastName = lastname ?? string.Empty;
        user.PhoneNumber = phone_number ?? string.Empty;
        user.DateOfBirth = dateofbirth is not null && DateOnly.TryParse(dateofbirth, out var dob) ? dob : DateOnly.MinValue;
        user.Gender = gender ?? string.Empty;
        var db = new User_DB();
        var data = db.Add(user);
        return data;
    }

    public async Task<User> UpdateUserAsync(int UserId, string? username, string? password, string? email, string? firstname, string? lastname, string? phone_number, string? dateofbirth,
         string? gender, bool forceReload = false, CancellationToken ct = default)
    {
        var user = new User();
        user.UserId = UserId;
        user.Username = username ?? string.Empty;
        user.Password = password ?? string.Empty;
        user.Email = email ?? string.Empty;
        user.FirstName = firstname ?? string.Empty;
        user.LastName = lastname ?? string.Empty;
        user.PhoneNumber = phone_number ?? string.Empty;
        user.DateOfBirth = dateofbirth is not null && DateOnly.TryParse(dateofbirth, out var dob) ? dob : DateOnly.MinValue;
        user.Gender = gender ?? string.Empty;
        var db = new User_DB();
        var data = db.Update(user);
        return data;
    }

    public async Task<User> DeleteUserAsync(int UserId, bool forceReload = false, CancellationToken ct = default)
    {
        var db = new User_DB();
        var data = db.Delete(UserId);
        return data;
    }
}
