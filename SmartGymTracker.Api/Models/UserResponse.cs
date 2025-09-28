namespace SmartGymTracker.Api.Models
{
    public sealed record UserResponse(int Count, IReadOnlyList<UserLogin> Data)
    {
    }
}
