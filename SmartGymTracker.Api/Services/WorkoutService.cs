using Library.SmartGymTracker.Models;
using MySQL.SmartGymTracker;

namespace SmartGymTracker.Api.Services;

public interface IWorkoutService
{
    Task<IReadOnlyList<Workout>> GetAllAsync(int? userId = null, CancellationToken ct = default);
    Task<Workout?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Workout?> AddAsync(Workout workout, CancellationToken ct = default);
    Task<Workout?> UpdateAsync(Workout workout, CancellationToken ct = default);
    Task<Workout?> DeleteAsync(int id, CancellationToken ct = default);
}

public sealed class WorkoutService : IWorkoutService
{
    private readonly Workout_DB _db = new();

    public Task<IReadOnlyList<Workout>> GetAllAsync(int? userId = null, CancellationToken ct = default)
    {
        List<Workout>? data = userId.HasValue
            ? _db.GetByUserId(userId.Value)
            : _db.GetAll();

        IReadOnlyList<Workout> result = data ?? new List<Workout>();
        return Task.FromResult(result);
    }

    public Task<Workout?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        if (id <= 0)
            return Task.FromResult<Workout?>(null);

        var workout = _db.GetById(id);
        return Task.FromResult(workout);
    }

    public Task<Workout?> AddAsync(Workout workout, CancellationToken ct = default)
    {
        if (workout is null)
            return Task.FromResult<Workout?>(null);

        var added = _db.Add(workout);
        return Task.FromResult(added);
    }

    public Task<Workout?> UpdateAsync(Workout workout, CancellationToken ct = default)
    {
        if (workout is null || workout.WorkoutId <= 0)
            return Task.FromResult<Workout?>(null);

        var updated = _db.Update(workout);
        return Task.FromResult(updated);
    }

    public Task<Workout?> DeleteAsync(int id, CancellationToken ct = default)
    {
        if (id <= 0)
            return Task.FromResult<Workout?>(null);

        var deleted = _db.Delete(id);
        return Task.FromResult(deleted);
    }
}
