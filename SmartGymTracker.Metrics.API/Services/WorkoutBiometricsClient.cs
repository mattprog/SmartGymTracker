using System.Net.Http.Json;
using Library.SmartGymTracker.Models;


namespace SmartGymTracker.Metrics.API.Services;

public interface IWorkoutBiometricsClient
{
    Task<IReadOnlyList<WorkoutBiometrics>> GetAllAsync(bool forceReload = false, CancellationToken ct = default);
    Task<WorkoutBiometrics?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<WorkoutBiometrics?> AddAsync(WorkoutBiometrics workoutBiometrics, CancellationToken ct = default);
    Task<WorkoutBiometrics?> UpdateAsync(WorkoutBiometrics workoutBiometrics, CancellationToken ct = default);
    Task<WorkoutBiometrics?> DeleteAsync(int id, CancellationToken ct = default);
}

public sealed class WorkoutBiometricsClient(HttpClient http) : IWorkoutBiometricsClient
{
    private const string DataUrl = "/api/workoutbiometrics";

    private IReadOnlyList<WorkoutBiometrics>? _cache;
    private DateTimeOffset _cachedAt = DateTimeOffset.MinValue;
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(30);

    public async Task<IReadOnlyList<WorkoutBiometrics>> GetAllAsync(bool forceReload = false, CancellationToken ct = default)
    {
        if (!forceReload && _cache is not null && DateTimeOffset.UtcNow - _cachedAt < CacheTtl)
            return _cache;

        var data = await http.GetFromJsonAsync<List<WorkoutBiometrics>>(DataUrl, ct)
                   ?? throw new InvalidOperationException("Failed to retrieve workout biometrics data from the API.");

        _cache = data;
        _cachedAt = DateTimeOffset.UtcNow;
        return _cache;
    }

    public async Task<WorkoutBiometrics?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        if (id <= 0) return null;

        return await http.GetFromJsonAsync<WorkoutBiometrics>($"{DataUrl}/{id}", ct);
    }

    public async Task<WorkoutBiometrics?> AddAsync(WorkoutBiometrics workoutBiometrics, CancellationToken ct = default)
    {
        var res = await http.PostAsJsonAsync(DataUrl, workoutBiometrics, ct);
        if (!res.IsSuccessStatusCode) return null;
        return await res.Content.ReadFromJsonAsync<WorkoutBiometrics>(cancellationToken: ct);
    }

    public async Task<WorkoutBiometrics?> UpdateAsync(WorkoutBiometrics workoutBiometrics, CancellationToken ct = default)
    {
        var res = await http.PutAsJsonAsync($"{DataUrl}/{workoutBiometrics.WorkoutId}", workoutBiometrics, ct);
        if (!res.IsSuccessStatusCode) return null;
        return await res.Content.ReadFromJsonAsync<WorkoutBiometrics>(cancellationToken: ct);
    }

    public async Task<WorkoutBiometrics?> DeleteAsync(int id, CancellationToken ct = default)
    {
        var res = await http.DeleteAsync($"{DataUrl}/{id}", ct);
        if (!res.IsSuccessStatusCode) return null;
        return await res.Content.ReadFromJsonAsync<WorkoutBiometrics>(cancellationToken: ct);
    }
}