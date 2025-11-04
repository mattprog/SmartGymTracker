using Library.SmartGymTracker.Models;
using System.Collections.Generic;

namespace SmartGymTracker.Metrics.API.Models;
public sealed record BiometricsResponse(int Count, IReadOnlyList<Biometrics> Data);