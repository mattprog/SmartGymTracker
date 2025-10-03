namespace SmartGymTracker.Metrics.API.Models;

public sealed record ExerciseBiometricsResponse(int Count, IReadOnlyList<ExerciseBiometrics> Data);