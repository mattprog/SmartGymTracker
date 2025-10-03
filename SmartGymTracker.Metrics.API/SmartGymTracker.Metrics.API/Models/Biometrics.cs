using System;
using System.Text.Json.Serialization;


namespace SmartGymTracker.Metrics.API.Models;

public sealed class Biometrics
{
    [JsonPropertyName("BiometricsId")]
    public int biometricsId { get; set; }

    [JsonPropertyName("UserId")]
    public int userId { get; set; }

    [JsonPropertyName("DateEntered")]
    public DateTime dateEntered { get; set; }

    [JsonPropertyName("Weight")]
    public float weight { get; set; }

    [JsonPropertyName("Height")]
    public float height { get; set; }

    [JsonPropertyName("BodyFatPercentage")]
    public float bodyFatPercentage { get; set; }

    [JsonPropertyName("BMI")]
    public float bmi { get; set; }

    [JsonPropertyName("RestingHeartRate")]
    public float restingHeartRate { get; set; }

    public void updatebmi()
    {
        if (height > 0)
        {
            bmi = weight / (height * height);
        }
    }
}