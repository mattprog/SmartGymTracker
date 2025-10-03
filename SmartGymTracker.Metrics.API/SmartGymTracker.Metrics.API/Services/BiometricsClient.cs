using System.Net.Http.Json;
using SmartGymTracker.Metrics.API.Models;

namespace SmartGymTracker.Metrics.API.Services;

public interface IBiometricsClient
{
    Task<IReadOnlyList<Biometrics>> GetAllAsync(bool forceReload = false, CancellationToken ct = default);
    Task<Biometrics?> GetByIdAsync(int id, CancellationToken ct = default);
}

public sealed class BiometricsClient(HttpClient http) : IBiometricsClient
{
    private const string DataUrl = "/api/biometrics";

    private IReadOnlyList<Biometrics>? _cache;
    private DateTimeOffset _cachedAt = DateTimeOffset.MinValue;
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(30);
    public async Task<IReadOnlyList<Biometrics>> GetAllAsync(bool forceReload = false, CancellationToken ct = default)
    {
        if (!forceReload && _cache is not null && DateTimeOffset.UtcNow - _cachedAt < CacheTtl)
        {
            return _cache;
        }

        var data = await http.GetFromJsonAsync<List<Biometrics>>(DataUrl, ct)
                   ?? throw new InvalidOperationException("Failed to retrieve biometrics data from the API.");

        _cache = data;
        _cachedAt = DateTimeOffset.UtcNow;
        return _cache;
    }

    public async Task<Biometrics?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await http.GetFromJsonAsync<Biometrics>($"{DataUrl}/{id}", ct);
    }
}
