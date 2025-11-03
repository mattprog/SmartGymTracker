using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Library.SmartGymTracker.Models;
using MySql.Data.MySqlClient;
using MySQL.SmartGymTracker;
using SmartGymTracker.Api.Models;

namespace SmartGymTracker.Api.Services;

public interface IUserService
{
    Task<UserDto> RegisterAsync(UserRegistrationRequest request, CancellationToken ct = default);
    Task<UserDto?> AuthenticateAsync(LoginRequest request, CancellationToken ct = default);
    Task<UserDto?> GetAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<UserDto>> ListAsync(CancellationToken ct = default);
    Task<UserDto?> UpdateAsync(int id, UserUpdateRequest request, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    Task<bool> ResetPasswordAsync(int id, ResetPasswordRequest request, CancellationToken ct = default);
}

public sealed class UserService : IUserService
{
    private const char PasswordSeparator = '.';

    private readonly User_DB _usersDb;
    private readonly Dictionary<int, UserRecord> _records = new();
    private readonly object _syncRoot = new();
    private int _nextLocalId = 1;

    public UserService()
    {
        _usersDb = new User_DB();
        TryLoadFromDatabase();

        lock (_syncRoot)
        {
            ResetNextLocalId_NoLock();

            if (!_records.Values.Any(r => r.IsAdmin))
            {
                var admin = CreateAdminRecord_NoLock();
                _records[admin.Id] = admin;
            }
        }
    }

    public Task<UserDto> RegisterAsync(UserRegistrationRequest request, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        var username = request.Username?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username is required.", nameof(request));
        }

        var password = request.Password?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password is required.", nameof(request));
        }

        var firstName = request.FirstName?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("First name is required.", nameof(request));
        }

        var lastName = request.LastName?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Last name is required.", nameof(request));
        }

        var email = request.Email?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email is required.", nameof(request));
        }

        var record = new UserRecord
        {
            Username = username,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PhoneNumber = string.IsNullOrWhiteSpace(request.PhoneNumber) ? null : request.PhoneNumber!.Trim(),
            DateOfBirth = request.DateOfBirth?.Date,
            Gender = string.IsNullOrWhiteSpace(request.Gender) ? null : request.Gender!.Trim(),
            Height = request.Height,
            Weight = request.Weight,
            IsAdmin = request.IsAdmin,
            StoredPassword = CreateStoredPassword(password)
        };

        lock (_syncRoot)
        {
            EnsureUniqueUsername_NoLock(record.Username);
            EnsureUniqueEmail_NoLock(record.Email);
        }

        int? dbId = TryPersistCreate(record);

        lock (_syncRoot)
        {
            EnsureUniqueUsername_NoLock(record.Username, dbId);
            EnsureUniqueEmail_NoLock(record.Email, dbId);

            if (dbId.HasValue)
            {
                record.Id = dbId.Value;
                EnsureNextLocalIdAbove_NoLock(record.Id);
            }
            else
            {
                record.Id = GetNextLocalId_NoLock();
            }

            _records[record.Id] = record;

            return Task.FromResult(ToDto(record));
        }
    }

    public Task<UserDto?> AuthenticateAsync(LoginRequest request, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        var username = request.Username?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(username))
        {
            return Task.FromResult<UserDto?>(null);
        }

        UserRecord? record;
        lock (_syncRoot)
        {
            record = _records.Values.FirstOrDefault(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        if (record is null)
        {
            record = TryFetchFromDatabaseByUsername(username);
            if (record is not null)
            {
                lock (_syncRoot)
                {
                    _records[record.Id] = record;
                    EnsureNextLocalIdAbove_NoLock(record.Id);
                }
            }
        }

        if (record is null)
        {
            return Task.FromResult<UserDto?>(null);
        }

        var password = request.Password?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(password))
        {
            return Task.FromResult<UserDto?>(null);
        }

        return VerifyPassword(password, record.StoredPassword)
            ? Task.FromResult<UserDto?>(ToDto(record))
            : Task.FromResult<UserDto?>(null);
    }

    public Task<UserDto?> GetAsync(int id, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        UserRecord? record;
        lock (_syncRoot)
        {
            _records.TryGetValue(id, out record);
        }

        if (record is null)
        {
            record = TryFetchFromDatabaseById(id);
            if (record is not null)
            {
                lock (_syncRoot)
                {
                    if (record.Id != id)
                    {
                        _records.Remove(id);
                    }

                    _records[record.Id] = record;
                    EnsureNextLocalIdAbove_NoLock(record.Id);
                }
            }
        }

        return Task.FromResult(record is null ? null : ToDto(record));
    }

    public Task<IReadOnlyList<UserDto>> ListAsync(CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        lock (_syncRoot)
        {
            var items = _records.Values
                .OrderBy(u => u.Username, StringComparer.OrdinalIgnoreCase)
                .Select(ToDto)
                .ToList()
                .AsReadOnly();

            return Task.FromResult<IReadOnlyList<UserDto>>(items);
        }
    }

    public Task<UserDto?> UpdateAsync(int id, UserUpdateRequest request, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        UserRecord? updated;
        UserDto? dto;

        lock (_syncRoot)
        {
            if (!_records.TryGetValue(id, out var current))
            {
                return Task.FromResult<UserDto?>(null);
            }

            var working = current.Clone();

            if (!string.IsNullOrWhiteSpace(request.Email) &&
                !request.Email!.Trim().Equals(current.Email, StringComparison.OrdinalIgnoreCase))
            {
                EnsureUniqueEmail_NoLock(request.Email.Trim(), id);
                working.Email = request.Email.Trim();
            }

            if (!string.IsNullOrWhiteSpace(request.FirstName))
            {
                working.FirstName = request.FirstName!.Trim();
            }

            if (!string.IsNullOrWhiteSpace(request.LastName))
            {
                working.LastName = request.LastName!.Trim();
            }

            if (request.PhoneNumber is not null)
            {
                working.PhoneNumber = string.IsNullOrWhiteSpace(request.PhoneNumber)
                    ? null
                    : request.PhoneNumber.Trim();
            }

            if (request.DateOfBirth.HasValue)
            {
                working.DateOfBirth = request.DateOfBirth.Value.Date;
            }

            if (request.Gender is not null)
            {
                working.Gender = string.IsNullOrWhiteSpace(request.Gender)
                    ? null
                    : request.Gender.Trim();
            }

            if (request.Height.HasValue)
            {
                working.Height = request.Height;
            }

            if (request.Weight.HasValue)
            {
                working.Weight = request.Weight;
            }

            if (request.IsAdmin.HasValue)
            {
                working.IsAdmin = request.IsAdmin.Value;
            }

            _records[id] = working;
            updated = working;
            dto = ToDto(working);
        }

        TryPersistUpdate(updated!);
        return Task.FromResult(dto);
    }

    public Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        UserRecord? removed;
        lock (_syncRoot)
        {
            if (!_records.TryGetValue(id, out removed))
            {
                return Task.FromResult(false);
            }

            _records.Remove(id);
        }

        TryPersistDelete(id);
        return Task.FromResult(true);
    }

    public Task<bool> ResetPasswordAsync(int id, ResetPasswordRequest request, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(request.NewPassword))
        {
            throw new ArgumentException("New password is required.", nameof(request));
        }

        UserRecord? updated;
        lock (_syncRoot)
        {
            if (!_records.TryGetValue(id, out var current))
            {
                return Task.FromResult(false);
            }

            var working = current.Clone();
            working.StoredPassword = CreateStoredPassword(request.NewPassword);
            _records[id] = working;
            updated = working;
        }

        TryPersistUpdate(updated!);
        return Task.FromResult(true);
    }

    private void TryLoadFromDatabase()
    {
        try
        {
            var users = _usersDb.GetAll();
            if (users is null)
            {
                return;
            }

            lock (_syncRoot)
            {
                foreach (var user in users)
                {
                    var record = FromUser(user);
                    _records[record.Id] = record;
                    EnsureNextLocalIdAbove_NoLock(record.Id);
                }
            }
        }
        catch (MySqlException)
        {
            // Database is unavailable; fallback to in-memory storage.
        }
        catch (InvalidOperationException)
        {
            // Database returned an unexpected result; ignore and use in-memory storage.
        }
    }

    private int? TryPersistCreate(UserRecord record)
    {
        try
        {
            var created = _usersDb.Add(ToUser(record));
            return created?.UserId;
        }
        catch (MySqlException)
        {
            return null;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }

    private void TryPersistUpdate(UserRecord record)
    {
        if (record.Id <= 0)
        {
            return;
        }

        try
        {
            _ = _usersDb.Update(ToUser(record));
        }
        catch (MySqlException)
        {
            // Ignore and keep in-memory state.
        }
        catch (InvalidOperationException)
        {
            // Ignore and keep in-memory state.
        }
    }

    private void TryPersistDelete(int id)
    {
        if (id <= 0)
        {
            return;
        }

        try
        {
            _ = _usersDb.Delete(id);
        }
        catch (MySqlException)
        {
            // Ignore and keep in-memory deletion.
        }
        catch (InvalidOperationException)
        {
            // Ignore and keep in-memory deletion.
        }
    }

    private UserRecord? TryFetchFromDatabaseById(int id)
    {
        if (id <= 0)
        {
            return null;
        }

        try
        {
            var user = _usersDb.GetById(id);
            return user is null ? null : FromUser(user);
        }
        catch (MySqlException)
        {
            return null;
        }
    }

    private UserRecord? TryFetchFromDatabaseByUsername(string username)
    {
        try
        {
            var users = _usersDb.GetAll();
            if (users is null)
            {
                return null;
            }

            var match = users.FirstOrDefault(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            return match is null ? null : FromUser(match);
        }
        catch (MySqlException)
        {
            return null;
        }
    }

    private User ToUser(UserRecord record)
    {
        var birthDate = record.DateOfBirth.HasValue
            ? DateOnly.FromDateTime(record.DateOfBirth.Value)
            : new DateOnly(2000, 1, 1);

        return new User
        {
            UserId = record.Id > 0 ? record.Id : 0,
            Username = record.Username,
            Password = record.StoredPassword,
            FirstName = record.FirstName,
            LastName = record.LastName,
            Email = record.Email,
            PhoneNumber = record.PhoneNumber ?? string.Empty,
            DateOfBirth = birthDate,
            Gender = record.Gender ?? string.Empty,
            Privilege = record.IsAdmin ? PrivilegeLevel.Admin : PrivilegeLevel.User
        };
    }

    private static UserRecord FromUser(User user)
    {
        DateTime? birthDate = null;
        if (user.DateOfBirth != DateOnly.MinValue)
        {
            birthDate = user.DateOfBirth.ToDateTime(TimeOnly.MinValue);
        }

        string? phone = string.IsNullOrWhiteSpace(user.PhoneNumber) ? null : user.PhoneNumber;
        string? gender = string.IsNullOrWhiteSpace(user.Gender) ? null : user.Gender;

        return new UserRecord
        {
            Id = user.UserId,
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = phone,
            DateOfBirth = birthDate,
            Gender = gender,
            Height = null,
            Weight = null,
            IsAdmin = user.Privilege == PrivilegeLevel.Admin,
            StoredPassword = user.Password
        };
    }

    private static UserDto ToDto(UserRecord record)
    {
        return new UserDto
        {
            Id = record.Id,
            Username = record.Username,
            Email = record.Email,
            FirstName = record.FirstName,
            LastName = record.LastName,
            PhoneNumber = record.PhoneNumber,
            DateOfBirth = record.DateOfBirth,
            Gender = record.Gender,
            Height = record.Height,
            Weight = record.Weight,
            IsAdmin = record.IsAdmin
        };
    }

    private void EnsureUniqueUsername_NoLock(string username, int? ignoreId = null)
    {
        if (_records.Values.Any(u =>
                (!ignoreId.HasValue || u.Id != ignoreId.Value) &&
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("Username already exists.");
        }
    }

    private void EnsureUniqueEmail_NoLock(string email, int? ignoreId = null)
    {
        if (_records.Values.Any(u =>
                (!ignoreId.HasValue || u.Id != ignoreId.Value) &&
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("Email already exists.");
        }
    }

    private int GetNextLocalId_NoLock() => _nextLocalId++;

    private void ResetNextLocalId_NoLock()
    {
        var maxId = _records.Keys.DefaultIfEmpty(0).Max();
        _nextLocalId = maxId >= 1 ? maxId + 1 : 1;
    }

    private void EnsureNextLocalIdAbove_NoLock(int id)
    {
        if (id >= _nextLocalId)
        {
            _nextLocalId = id + 1;
        }
    }

    private UserRecord CreateAdminRecord_NoLock()
    {
        return new UserRecord
        {
            Id = GetNextLocalId_NoLock(),
            Username = "admin",
            FirstName = "Admin",
            LastName = "User",
            Email = "admin@example.com",
            PhoneNumber = null,
            DateOfBirth = null,
            Gender = null,
            Height = null,
            Weight = null,
            IsAdmin = true,
            StoredPassword = CreateStoredPassword("ChangeMe123!")
        };
    }

    private static string CreateStoredPassword(string password)
    {
        var (hash, salt) = HashPassword(password);
        return FormatPassword(hash, salt);
    }

    private static string FormatPassword(string hash, string salt) =>
        string.Join(PasswordSeparator, salt, hash);

    private static bool VerifyPassword(string password, string stored)
    {
        if (TryParseStoredPassword(stored, out var salt, out var hash))
        {
            var attempt = ComputeHash(password, salt);
            return string.Equals(hash, attempt, StringComparison.Ordinal);
        }

        return string.Equals(stored, password, StringComparison.Ordinal);
    }

    private static bool TryParseStoredPassword(string stored, out string salt, out string hash)
    {
        var parts = stored.Split(PasswordSeparator, 2, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 2)
        {
            salt = parts[0];
            hash = parts[1];
            return true;
        }

        salt = string.Empty;
        hash = stored;
        return false;
    }

    private static (string Hash, string Salt) HashPassword(string password)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(16);
        var salt = Convert.ToBase64String(saltBytes);
        var hash = ComputeHash(password, salt);
        return (hash, salt);
    }

    private static string ComputeHash(string password, string salt)
    {
        var bytes = Encoding.UTF8.GetBytes($"{salt}:{password}");
        var hashBytes = SHA256.HashData(bytes);
        return Convert.ToBase64String(hashBytes);
    }

    private sealed class UserRecord
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public bool IsAdmin { get; set; }
        public string StoredPassword { get; set; } = string.Empty;

        public UserRecord Clone()
        {
            return (UserRecord)MemberwiseClone();
        }
    }
}
