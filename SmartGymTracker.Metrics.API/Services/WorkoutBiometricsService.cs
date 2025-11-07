using Library.SmartGymTracker.Models;
using MySQL.SmartGymTracker;

namespace SmartGymTracker.Metrics.API.Services;

public interface IWorkoutBiometricsService
{
    Task<IReadOnlyList<WorkoutBiometrics>> SearchAsync(int? workoutId, CancellationToken ct = default);
    Task<WorkoutBiometrics?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<WorkoutBiometrics?> AddAsync(WorkoutBiometrics bio, CancellationToken ct = default);
    Task<WorkoutBiometrics?> UpdateAsync(WorkoutBiometrics bio, CancellationToken ct = default);
    Task<WorkoutBiometrics?> DeleteAsync(int id, CancellationToken ct = default);
}

public sealed class WorkoutBiometricsService : IWorkoutBiometricsService
{
    private readonly WorkoutBiometrics_DB _db = new();

    public async Task<IReadOnlyList<WorkoutBiometrics>> SearchAsync(int? workoutId, CancellationToken ct = default)
    {
        var all = _db.GetAll() ?? new List<WorkoutBiometrics>();

        var filtered = all
            .Where(b =>
                (!workoutId.HasValue || b.WorkoutId == workoutId)
            ).ToList();

        return await Task.FromResult(filtered);
    }

    public async Task<WorkoutBiometrics?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        if (id <= 0) return null;
        var bio = _db.GetById(id);
        return await Task.FromResult(bio);
    }

    public async Task<WorkoutBiometrics?> AddAsync(WorkoutBiometrics bio, CancellationToken ct = default)
    {
        if (bio == null) return null;
        var added = _db.Add(bio);
        return await Task.FromResult(added);
    }

    public async Task<WorkoutBiometrics?> UpdateAsync(WorkoutBiometrics bio, CancellationToken ct = default)
    {
        if (bio == null || bio.WorkoutId <= 0) return null;
        var updated = _db.Update(bio);
        return await Task.FromResult(updated);
    }

    public async Task<WorkoutBiometrics?> DeleteAsync(int id, CancellationToken ct = default)
    {
        if (id <= 0) return null;
        var deleted = _db.Delete(id);
        return await Task.FromResult(deleted);
    }
}