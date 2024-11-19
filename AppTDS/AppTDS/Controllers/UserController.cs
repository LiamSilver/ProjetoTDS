using AppTDS.Models;
using AppTDS.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AppTDS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly RegistrationSystem _registrationSystem;

        public UserController(UserService userService, RegistrationSystem registrationSystem)
        {
            _userService = userService;
            _registrationSystem = registrationSystem;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _userService.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _userService.UpdateUserAsync(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateUser([FromBody] LoginModel loginModel)
        {
            var user = await _userService.GetUserByEmailAsync(loginModel.Email);
            if (user == null || !user.AuthenticatePassword(loginModel.Password))
            {
                return Unauthorized();
            }

            return Ok(user);
        }

        [HttpPost("request-password-update")]
        public async Task<IActionResult> RequestPasswordUpdate([FromBody] EmailModel emailModel)
        {
            var result = await _registrationSystem.RequestPasswordUpdateAsync(emailModel.Email);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordModel updatePasswordModel)
        {
            var result = await _registrationSystem.RegistrationUpdatePasswordAsync(
                updatePasswordModel.Email,
                updatePasswordModel.OldPassword,
                updatePasswordModel.NewPassword,
                updatePasswordModel.VerificationCode);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("request-password-recovery")]
        public async Task<IActionResult> RequestPasswordRecovery([FromBody] EmailModel emailModel)
        {
            var result = await _registrationSystem.RequestPasswordRecoveryAsync(emailModel.Email);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPost("recover-password")]
        public async Task<IActionResult> RecoverPassword([FromBody] RecoverPasswordModel recoverPasswordModel)
        {
            var result = await _registrationSystem.RecoverPasswordAsync(
                recoverPasswordModel.Email,
                recoverPasswordModel.NewPassword,
                recoverPasswordModel.VerificationCode);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class EmailModel
    {
        public string Email { get; set; }
    }

    public class UpdatePasswordModel
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string VerificationCode { get; set; }
    }

    public class RecoverPasswordModel
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string VerificationCode { get; set; }
    }

}
