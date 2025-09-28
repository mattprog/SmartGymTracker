using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static SmartGymTracker.Api.Controllers.UserController;
using System.Threading.Tasks;
using SmartGymTracker.Api.Models;

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
            public async Task<IActionResult> Register(UserLogin model)
            {
                var user = new ApplicationUser { UserName = model.username, Email = model.email, FirstName = model.firstname, LastName = model.lastname,
                    Phone = model.phone_number, BirthDate = model.dateofbirth, Weight = model.weight, Height = model.height };
                var result = await _userManager.CreateAsync(user, model.password);
                if (result.Succeeded)
                {
                    return Ok("Registration successful");
                }
                return BadRequest(result.Errors);
            }
        }
    }

}

