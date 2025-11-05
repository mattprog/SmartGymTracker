using System.Net.Sockets;
using SmartGymTracker.Api.Models;
using Library.SmartGymTracker.Models;
using MySQL.SmartGymTracker;
namespace SmartGymTracker.Api.Services;

    public interface IUserService
    {
        Task<IReadOnlyList<User>> SearchAsync(string? UserId, string? username, string? password, string? email, string firstname, string? lastname, 
            string phone_number, string? dateofbirth, string? gender, CancellationToken ct = default);
        Task<User> GetUserAsync(int UserId, CancellationToken ct = default);

        Task<User> AddUserAsync(string? username, string? password, string? email, string firstname, string? lastname,
            string phone_number, string? dateofbirth, string? gender, CancellationToken ct = default);
        Task<User> UpdateUserAsync(int UserId, string? username, string? password, string? email, string firstname, string? lastname,
            string phone_number, string? dateofbirth, string? gender, CancellationToken ct = default);

        Task<User> UpdatePassword(string? email, string? newPassword, CancellationToken ct = default);

        Task<User> DeleteUserAsync(int UserId, CancellationToken ct = default);
}
    public sealed class UserService(IUserClient client) : IUserService
    {
        public async Task<IReadOnlyList<User>> SearchAsync(
            string? UserId, string? username, string? password, string? email, string firstname, string? lastname, string phone_number, string? dateofbirth, 
           string? gender, CancellationToken ct = default)
        {
            var all = await client.GetAllAsync(false, ct);

          return all.Where(u =>
          (string.IsNullOrWhiteSpace(username) || string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase)) &&
          (string.IsNullOrWhiteSpace(email) || string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase)) &&
          (string.IsNullOrWhiteSpace(firstname) || string.Equals(u.FirstName, firstname, StringComparison.OrdinalIgnoreCase)) &&
          (string.IsNullOrWhiteSpace(lastname) || string.Equals(u.LastName, lastname, StringComparison.OrdinalIgnoreCase))
          ).ToList();
        }

        public async Task<User> GetUserAsync(
            int UserId, CancellationToken ct = default)
        {
            var user = await client.GetUserAsync(UserId, false, ct);

            return user;
        }
        public async Task<User> AddUserAsync(
          string? username, string? password, string? email, string firstname, string? lastname, string phone_number, string? dateofbirth,
          string? gender, CancellationToken ct = default)
       {
          var user = await client.AddUserAsync(username, password, email, firstname, lastname, phone_number, dateofbirth,
          gender, false, ct);

          return user;
       }
       public async Task<User> UpdateUserAsync(
         int UserId, string? username, string? password, string? email, string firstname, string? lastname, string phone_number, string? dateofbirth,
         string? gender, CancellationToken ct = default)
      {
        var user = await client.UpdateUserAsync(UserId, username, password, email, firstname, lastname, phone_number, dateofbirth,
        gender, false, ct);

        return user;
      }

    public async Task<User> UpdatePassword(
     string? email, string? newPassword, CancellationToken ct = default)
      {
        var user = await client.UpdatePassword(email, newPassword, ct);

        return user;
      }

    public async Task<User> DeleteUserAsync(
        int UserId, CancellationToken ct = default)
      {
        var user = await client.DeleteUserAsync(UserId, false, ct);

        return user;
      }
}


