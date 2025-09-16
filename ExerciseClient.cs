using System.Net.Http.Json;
using SmartGymTracker.Api.Models;

namespace SmartGymTracker.Api.Services;

public interface IExerciseClient
{
    Task<IReadOnlyList<Exercise>> GetAllAsync(bool forceReload = false, CancellationToken ct = default);
    static string ImageUrl(string rel) =>
        "https://raw.githubusercontent.com/yuhonas/free-exercise-db/main/exercises/" + rel.TrimStart('/');
}

public sealed class ExerciseClient(HttpClient http) : IExerciseClient
{
    private const string DataUrl =
        "https://raw.githubusercontent.com/yuhonas/free-exercise-db/main/dist/exercises.json";

    private IReadOnlyList<Exercise>? _cache;
    private DateTimeOffset _cachedAt = DateTimeOffset.MinValue;
    private static readonly TimeSpan CacheTtl = TimeSpan.FromHours(12);

    public async Task<IReadOnlyList<Exercise>> GetAllAsync(bool forceReload = false, CancellationToken ct = default)
    {
        if (!forceReload && _cache is not null && DateTimeOffset.UtcNow - _cachedAt < CacheTtl)
            return _cache;

        using var req = new HttpRequestMessage(HttpMethod.Get, DataUrl);
        var res = await http.SendAsync(req, ct);
        res.EnsureSuccessStatusCode();

        var data = await res.Content.ReadFromJsonAsync<List<Exercise>>(cancellationToken: ct)
                   ?? throw new InvalidOperationException("No data");

        _cache = data;
        _cachedAt = DateTimeOffset.UtcNow;
        return _cache;
    }
}
