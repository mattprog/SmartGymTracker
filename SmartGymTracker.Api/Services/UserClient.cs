using System.Net.Http.Json;
using SmartGymTracker.Api.Models;
using SmartGymTracker.Api.Serialization;
namespace SmartGymTracker.Api.Services;

    public interface IUserClient
    {
        Task<IReadOnlyList<User>> GetAllAsync(bool forceReload = false, CancellationToken ct = default);
        
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
        // using var req = new HttpRequestMessage(HttpMethod.Get,);
        //var res = await http.SendAsync(req, ct);
        //res.EnsureSuccessStatusCode();

        //var data = await res.Content.ReadFromJsonAsync(
        //  UserJsonContext.Default.ListUser,
        //cancellationToken: ct
        //) ?? throw new InvalidOperationException("No data");
        var data = new User[0];
        _cache = data;
        _cachedAt = DateTimeOffset.UtcNow;
        return _cache;
    }
}
