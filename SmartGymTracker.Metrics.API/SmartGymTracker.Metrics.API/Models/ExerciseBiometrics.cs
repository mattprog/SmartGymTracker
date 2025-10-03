using System;
using System.Text.Json.Serialization;

namespace SmartGymTracker.Metrics.API.Models;

public sealed class ExerciseBiometrics
{
    [JsonPropertyName("ExerciseBiometricId")]
    public int exerciseBiometricId { get; set; }

    [JsonPropertyName("WorkoutId")]
    public int workoutId { get; set; }

    [JsonPropertyName("Steps")]
    public int steps { get; set; }

    [JsonPropertyName("AverageHeartRate")]    
    public int averageHeartRate { get; set; }

    [JsonPropertyName("MaxHeartRate")]
    public int maxHeartRate { get; set; }

    [JsonPropertyName("CaloriesBurned")]
    public int caloriesBurned { get; set; }

    [JsonPropertyName("Feeling")]
    public string? feeling { get; set; }

    [JsonPropertyName("SleepScore")]
    public int sleepScore { get; set; }
}