using System.Xml.Serialization;
using System.Net.Mail;
using System.Net;
using AppTDS.Services;

namespace AppTDS.Models
{
    public class RegistrationSystem
    {
        private readonly UserService _userService;
        private Dictionary<string, string>? _verificationCode { get; set; }

        public RegistrationSystem(UserService userService)
        {
            _userService = userService;
            _verificationCode = new Dictionary<string, string>();
        }

        public async Task RegisterUserAsync(User user)
        {
            await _userService.AddUserAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userService.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(int userId)
        {
            await _userService.DeleteUserAsync(userId);
        }

        public async Task<User> AuthenticateUserAsync(string email, string password)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            return user != null && user.AuthenticatePassword(password) ? user : null;
        }

        public async Task<bool> RequestPasswordUpdateAsync(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null) return false;

            var verificationCode = GenerateVerificationCode();
            _verificationCode[email] = verificationCode;

            await SendVerificationCodeAsync(email, verificationCode);
            return true;
        }

        public async Task<bool> RegistrationUpdatePasswordAsync(string email, string oldPassword, string newPassword, string verificationCode)
        {
            var user = await AuthenticateUserAsync(email, oldPassword);
            if (user == null) return false;

            if (_verificationCode.ContainsKey(email) && _verificationCode[email] == verificationCode)
            {
                user.UpdatePassword(newPassword);
                await _userService.UpdateUserAsync(user);
                _verificationCode.Remove(email);
                return true;
            }
            return false;
        }

        private string GenerateVerificationCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public async Task<bool> RequestPasswordRecoveryAsync(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null) return false;

            var verificationCode = GenerateVerificationCode();
            _verificationCode[email] = verificationCode;

            await SendVerificationCodeAsync(email, verificationCode);
            return true;
        }

        public async Task<bool> RecoverPasswordAsync(string email, string newPassword, string verificationCode)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null) return false;

            if (_verificationCode.ContainsKey(email) && _verificationCode[email] == verificationCode)
            {
                user.UpdatePassword(newPassword);
                await _userService.UpdateUserAsync(user);
                _verificationCode.Remove(email);
                return true;
            }
            return false;
        }

        private async Task SendVerificationCodeAsync(string email, string verificationCode)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("apptds9@gmail.com", "AppTDS@10"),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("apptds9@gmail.com"),
                Subject = "Código de Verificação",
                Body = $"Seu código de verificação é: {verificationCode}",
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
