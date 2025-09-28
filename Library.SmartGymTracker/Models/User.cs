using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SmartGymTracker.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; }

        // Default Constructor
        public User()
        {
            UserId = -1;
            Username = string.Empty;
            Password = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            DateOfBirth = DateOnly.MinValue;
            Gender = string.Empty;
        }

        // Copy Constructor
        public User(User u)
        {
            UserId = u.UserId;
            Username = u.Username;
            Password = u.Password;
            FirstName = u.FirstName;
            LastName = u.LastName;
            Email = u.Email;
            PhoneNumber = u.PhoneNumber;
            DateOfBirth = u.DateOfBirth;
            Gender = u.Gender;
        }
    }
}