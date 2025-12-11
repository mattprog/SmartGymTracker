using Library.SmartGymTracker.Models;

namespace SmartGymTracker.Api.Models;

public record WorkoutsResponse(int Count, IReadOnlyList<Workout> Data);
