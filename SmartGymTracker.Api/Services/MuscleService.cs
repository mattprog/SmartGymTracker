using Library.SmartGymTracker.Models;
using MySQL.SmartGymTracker;

namespace SmartGymTracker.Api.Services;

public interface IMuscleService
{
    Task<IReadOnlyList<Muscle>> GetAllAsync(CancellationToken ct = default);
    Task<Muscle?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Muscle?> AddAsync(Muscle muscle, CancellationToken ct = default);
    Task<Muscle?> UpdateAsync(Muscle muscle, CancellationToken ct = default);
    Task<Muscle?> DeleteAsync(int id, CancellationToken ct = default);
}

public sealed class MuscleService : IMuscleService
{
    private readonly Muscle_DB _db = new();

    public Task<IReadOnlyList<Muscle>> GetAllAsync(CancellationToken ct = default)
    {
        IReadOnlyList<Muscle> result = _db.GetAll() ?? new List<Muscle>();
        return Task.FromResult(result);
    }

    public Task<Muscle?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        if (id <= 0)
            return Task.FromResult<Muscle?>(null);

        var muscle = _db.GetById(id);
        return Task.FromResult(muscle);
    }

    public Task<Muscle?> AddAsync(Muscle muscle, CancellationToken ct = default)
    {
        if (muscle is null)
            return Task.FromResult<Muscle?>(null);

        var added = _db.Add(muscle);
        return Task.FromResult(added);
    }

    public Task<Muscle?> UpdateAsync(Muscle muscle, CancellationToken ct = default)
    {
        if (muscle is null || muscle.MuscleId <= 0)
            return Task.FromResult<Muscle?>(null);

        var updated = _db.Update(muscle);
        return Task.FromResult(updated);
    }

    public Task<Muscle?> DeleteAsync(int id, CancellationToken ct = default)
    {
        if (id <= 0)
            return Task.FromResult<Muscle?>(null);

        var deleted = _db.Delete(id);
        return Task.FromResult(deleted);
    }
}
