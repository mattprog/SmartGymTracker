//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;
//using Library.SmartGymTracker.Models; // For ApplicationUser
//using static global::SmartGymTracker.Api.Controllers.UserController;

//    namespace SmartGymTracker.Api.Controllers
//    {
//        //[Route("api/[controller]")]
//        //[ApiController]
//        public class PasswordResetController : ControllerBase
//        {
//            private readonly UserManager<ApplicationUser> _userManager;

//            public PasswordResetController(UserManager<ApplicationUser> userManager)
//            {
//                _userManager = userManager;
//            }

//            // 1. Request password reset (send token)
//            //[HttpPost("request")]
//            public async Task<IActionResult> RequestReset([FromBody] string email)
//            {
//                var user = await _userManager.FindByEmailAsync(email);
//                if (user == null)
//                    return BadRequest("User not found.");

//                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
//                // TODO: Send token via email (not shown here)
//                return Ok(new { Token = token }); // For demo purposes only
//            }

//            // 2. Reset password
//            [HttpPost("reset")]
//            public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
//            {
//                var user = await _userManager.FindByEmailAsync(model.Email);
//                if (user == null)
//                    return BadRequest("User not found.");

//                var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
//                if (result.Succeeded)
//                    return Ok("Password reset successful.");

//                return BadRequest(result.Errors);
//            }
//        }

//        public class ResetPasswordModel
//        {
//            public string Email { get; set; }
//            public string Token { get; set; }
//            public string NewPassword { get; set; }
//        }
//    }


