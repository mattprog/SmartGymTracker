namespace SmartGymTracker.Api.Models;

public sealed class Exercise
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Force { get; set; }
    public string Level { get; set; } = default!;
    public string? Mechanic { get; set; }
    public string? Equipment { get; set; }
    public List<string> PrimaryMuscles { get; set; } = new();
    public List<string> SecondaryMuscles { get; set; } = new();
    public List<string> Instructions { get; set; } = new();
    public string Category { get; set; } = default!;
    public List<string> Images { get; set; } = new();
}
