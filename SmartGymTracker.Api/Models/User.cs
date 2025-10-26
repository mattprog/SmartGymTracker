namespace SmartGymTracker.Api.Models
{
    public class User
    {
        
        public string? UserId { get; set; } = default!;

        public record LoginRequest(string username, string password);
        public string? username { get; set; } 
        public string? password { get; set; }
        public string? firstname { get; set; }
        public string? lastname { get; set; }
        public string? email { get; set; }
        public string? phone_number { get; set; }
        public string? dateofbirth { get; set; }
        public int weight { get; set; }
        public int height { get; set; }
        public string? gender {get; set; }

        
    }
}
