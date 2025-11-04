using Library.SmartGymTracker.Models;
using System.Collections.Generic;

namespace SmartGymTracker.Metrics.API.Models;
public sealed record WorkoutBiometricsResponse(int Count, IReadOnlyList<WorkoutBiometrics> Data);