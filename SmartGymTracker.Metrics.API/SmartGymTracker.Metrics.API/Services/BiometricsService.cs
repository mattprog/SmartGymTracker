using SmartGymTracker.Metrics.API.Models;

namespace SmartGymTracker.Metrics.API.Services
{
    public interface IBiometricsService
    {
        Task<IReadOnlyList<Biometrics>> SearchAsync(int? userId = null, DateTime? date = null, CancellationToken ct = default);
        Task AddAsync(Biometrics biometrics, CancellationToken ct = default);
    }

    public sealed class BiometricsService : IBiometricsService
    {
        private readonly List<Biometrics> _storage = new();

        public async Task<IReadOnlyList<Biometrics>> SearchAsync(int? userId = null, DateTime? date = null, CancellationToken ct = default)
        {
            await Task.Yield();
            return _storage.Where(b =>
                (!userId.HasValue || b.userId == userId.Value) &&
                (!date.HasValue || b.dateEntered.Date == date.Value.Date)).ToList();
        }

        public async Task AddAsync(Biometrics biometrics, CancellationToken ct = default)
        {
            await Task.Yield();
            biometrics.updatebmi();
            biometrics.biometricsId = _storage.Count + 1;
            _storage.Add(biometrics);
        }
    }
}
