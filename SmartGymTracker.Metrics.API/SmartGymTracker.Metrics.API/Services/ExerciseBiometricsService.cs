using SmartGymTracker.Metrics.API.Models;

namespace SmartGymTracker.Metrics.API.Services
{
    public interface IExerciseBiometricsService
    {
        Task<IReadOnlyList<ExerciseBiometrics>> SearchAsync(int? workoutId = null, CancellationToken ct = default);
        Task AddAsync(ExerciseBiometrics entry, CancellationToken ct = default);
    }

    public sealed class ExerciseBiometricsService : IExerciseBiometricsService
    {
        private readonly List<ExerciseBiometrics> _storage = new();

        public async Task<IReadOnlyList<ExerciseBiometrics>> SearchAsync(int? workoutId = null, CancellationToken ct = default)
        {
            await Task.Yield();
            return _storage.Where(e =>
                !workoutId.HasValue || e.workoutId == workoutId.Value)
                .ToList();
        }

        public async Task AddAsync(ExerciseBiometrics entry, CancellationToken ct = default)
        {
            await Task.Yield();
            entry.exerciseBiometricId = _storage.Count + 1;
            _storage.Add(entry);
        }
    }
}