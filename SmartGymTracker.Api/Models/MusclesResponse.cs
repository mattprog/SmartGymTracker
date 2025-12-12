using Library.SmartGymTracker.Models;

namespace SmartGymTracker.Api.Models;

public record MusclesResponse(int Count, IReadOnlyList<Muscle> Data);
