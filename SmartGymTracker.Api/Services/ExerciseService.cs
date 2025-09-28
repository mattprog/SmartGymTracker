using SmartGymTracker.Api.Models;

namespace SmartGymTracker.Api.Services;

public interface IExerciseService
{
    Task<IReadOnlyList<Exercise>> SearchAsync(string? q = null, string? muscle = null,
        string? equipment = null, string? category = null, CancellationToken ct = default);
}

public sealed class ExerciseService(IExerciseClient client) : IExerciseService
{
    public async Task<IReadOnlyList<Exercise>> SearchAsync(
        string? q = null, string? muscle = null, string? equipment = null, string? category = null, CancellationToken ct = default)
    {
        var all = await client.GetAllAsync(false, ct);

        return all.Where(e =>
        {
            bool qok = string.IsNullOrWhiteSpace(q)
                       || e.Name.Contains(q!, StringComparison.OrdinalIgnoreCase)
                       || string.Join(' ', e.Instructions).Contains(q!, StringComparison.OrdinalIgnoreCase);

            bool mok = string.IsNullOrWhiteSpace(muscle)
                       || e.PrimaryMuscles.Contains(muscle!, StringComparer.OrdinalIgnoreCase)
                       || e.SecondaryMuscles.Contains(muscle!, StringComparer.OrdinalIgnoreCase);

            bool eok = string.IsNullOrWhiteSpace(equipment)
                       || string.Equals(e.Equipment, equipment, StringComparison.OrdinalIgnoreCase);

            bool cok = string.IsNullOrWhiteSpace(category)
                       || string.Equals(e.Category, category, StringComparison.OrdinalIgnoreCase);

            return qok && mok && eok && cok;
        })
        .Select(e => {
            e.Images = e.Images.Select(IExerciseClient.ImageUrl).ToList();
            return e;
        })
        .ToList();

    }
}
