using Library.SmartGymTracker.Models;
namespace SmartGymTracker.Api.Models
{
    public sealed record UserResponse(int Count, IReadOnlyList<User> Data)
    {
    }
   
}
