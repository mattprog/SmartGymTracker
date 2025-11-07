using Library.SmartGymTracker.Models;
using MySQL.SmartGymTracker;

namespace SmartGymTracker.Metrics.API.Services;

public interface IBiometricsService
{
    Task<IReadOnlyList<Biometrics>> SearchAsync(int? userId, DateTime? date, CancellationToken ct = default);
    Task<Biometrics?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Biometrics?> AddAsync(Biometrics bio, CancellationToken ct = default);
    Task<Biometrics?> UpdateAsync(Biometrics bio, CancellationToken ct = default);
    Task<Biometrics?> DeleteAsync(int id, CancellationToken ct = default);
}

public sealed class BiometricsService : IBiometricsService
{
    private readonly Biometrics_DB _db = new();

    public async Task<IReadOnlyList<Biometrics>> SearchAsync(int? userId, DateTime? date, CancellationToken ct = default)
    {
        var all = _db.GetAll();
        if (all == null)
            all = new List<Biometrics>();

        var filtered = all
            .Where(b =>
                (!userId.HasValue || b.UserId == userId) &&
                (!date.HasValue || b.DateEntered == DateOnly.FromDateTime(date.Value))
            ).ToList();

        return await Task.FromResult(filtered);
    }

    public async Task<Biometrics?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        if (id <= 0) return null;
        var bio = _db.GetById(id);
        return await Task.FromResult(bio);
    }

    public async Task<Biometrics?> AddAsync(Biometrics bio, CancellationToken ct = default)
    {
        if (bio == null) return null;
        var added = _db.Add(bio);
        return await Task.FromResult(added);
    }

    public async Task<Biometrics?> UpdateAsync(Biometrics bio, CancellationToken ct = default)
    {
        if (bio == null || bio.BiometricsId <= 0) return null;
        var updated = _db.Update(bio);
        return await Task.FromResult(updated);
    }

    public async Task<Biometrics?> DeleteAsync(int id, CancellationToken ct = default)
    {
        if (id <= 0) return null;
        var deleted = _db.Delete(id);
        return await Task.FromResult(deleted);
    }
}
