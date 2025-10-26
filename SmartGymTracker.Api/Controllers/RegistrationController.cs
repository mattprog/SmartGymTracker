using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static SmartGymTracker.Api.Controllers.UserController;
using System.Threading.Tasks;
using SmartGymTracker.Api.Models;
using Library.SmartGymTracker.Models;

namespace SmartGymTracker.Api.Controllers
{

    public class RegistrationController
    {
        [Route("api/[controller]")]
        [ApiController]
        public class RegisterController : ControllerBase
        {
            private readonly UserManager<ApplicationUser> _userManager;

            public RegisterController(UserManager<ApplicationUser> userManager)
            {
                _userManager = userManager;
            }

            [HttpPost]
            public async Task<IActionResult> Register(User model)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Phone = model.PhoneNumber,
                    BirthDate = model.DateOfBirth.ToString("yyyy-MM-dd") // Convert DateOnly to string  
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return Ok("Registration successful");
                }
                return BadRequest(result.Errors);
            }
        }
    }

}

