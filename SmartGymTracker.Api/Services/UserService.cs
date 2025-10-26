using System.Net.Sockets;
using SmartGymTracker.Api.Models;
namespace SmartGymTracker.Api.Services;

    public interface IUserService
    {
        Task<IReadOnlyList<User>> SearchAsync(string? UserId, string? username, string? password, string? email, string firstname, string? lastname, 
            string phone_number, string? dateofbirth, string? weight, string? height, string? gender, CancellationToken ct = default);
    }
    public sealed class UserService(IUserClient client) : IUserService
    {
        public async Task<IReadOnlyList<User>> SearchAsync(
            string? UserId, string? username, string? password, string? email, string firstname, string? lastname, string phone_number, string? dateofbirth, 
            string? weight, string? height, string? gender, CancellationToken ct = default)
        {
            var all = await client.GetAllAsync(false, ct);

        return all.Where(u =>
          (string.IsNullOrWhiteSpace(username) || string.Equals(u.username, username, StringComparison.OrdinalIgnoreCase)) &&
          (string.IsNullOrWhiteSpace(email) || string.Equals(u.email, email, StringComparison.OrdinalIgnoreCase)) &&
          (string.IsNullOrWhiteSpace(firstname) || string.Equals(u.firstname, firstname, StringComparison.OrdinalIgnoreCase)) &&
          (string.IsNullOrWhiteSpace(lastname) || string.Equals(u.lastname, lastname, StringComparison.OrdinalIgnoreCase))
      ).ToList();


        }
    }

