using System.Net.Http.Json;
using SmartGymTracker.Metrics.API.Models;

namespace SmartGymTracker.Metrics.API.Services;

public interface IExerciseBiometricsClient
{
    Task<IReadOnlyList<ExerciseBiometrics>> GetAllAsync(bool forceReload = false, CancellationToken ct = default);
    Task<ExerciseBiometrics?> GetByIdAsync(int id, CancellationToken ct = default);
}

public sealed class ExerciseBiometricsClient(HttpClient http) : IExerciseBiometricsClient
{
    private const string DataUrl = "/api/exercisebiometrics";

    private IReadOnlyList<ExerciseBiometrics>? _cache;
    private DateTimeOffset _cachedAt = DateTimeOffset.MinValue;
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(30);
    public async Task<IReadOnlyList<ExerciseBiometrics>> GetAllAsync(bool forceReload = false, CancellationToken ct = default)
    {
        if (!forceReload && _cache is not null && DateTimeOffset.UtcNow - _cachedAt < CacheTtl)
        {
            return _cache;
        }

        var data = await http.GetFromJsonAsync<List<ExerciseBiometrics>>(DataUrl, ct)
                   ?? throw new InvalidOperationException("Failed to retrieve exercise biometrics data from the API.");

        _cache = data;
        _cachedAt = DateTimeOffset.UtcNow;
        return _cache;
    }

    public async Task<ExerciseBiometrics?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await http.GetFromJsonAsync<ExerciseBiometrics>($"{DataUrl}/{id}", ct);
    }
}