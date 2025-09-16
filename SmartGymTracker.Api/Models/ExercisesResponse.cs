namespace SmartGymTracker.Api.Models;

public sealed record ExercisesResponse(int Count, IReadOnlyList<Exercise> Data);
