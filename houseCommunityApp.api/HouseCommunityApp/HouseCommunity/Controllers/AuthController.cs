using HouseCommunity.Data;
using HouseCommunity.DTOs;
using HouseCommunity.Helpers;
using HouseCommunity.Request;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HouseCommunity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly NotificationMetadata _notificationMetadata;

        public AuthController(IAuthRepository repo, IConfiguration config, NotificationMetadata notificationMetadata)
        {
            this._repo = repo;
            this._config = config;
            this._notificationMetadata = notificationMetadata;
        }


        [HttpPost("login")]
        public async Task<IActionResult> LogIn(UserForLoginDTO userForLoginDTO)
        {

            var userFromRepo = await _repo.LogIn(userForLoginDTO.UserName.ToLower(), userForLoginDTO.Password);

            if (userFromRepo == null)
                return Unauthorized();

            SecurityTokenDescriptor tokenDescriptor = GetTokenDescriptor(userFromRepo);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(
                new
                {
                    token = tokenHandler.WriteToken(token),
                    user = userFromRepo
                });

        }

        private SecurityTokenDescriptor GetTokenDescriptor(Model.User userFromRepo)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.UserName)            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            return tokenDescriptor;
        }

        [HttpPost("req-reset-password")]
        public async Task<IActionResult> ResetPasswordRequest(UserForPasswordResetRequest usernameuserForLoginDTO)
        {

            var userFromRepo = await _repo.GetUserForReset(usernameuserForLoginDTO.Email.ToLower());
            SecurityTokenDescriptor tokenDescriptor = GetTokenDescriptor(userFromRepo);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);


            Get(usernameuserForLoginDTO.Email, tokenHandler.WriteToken(token));
            return Ok();
        }

        [HttpPost("valid-password-token")]
        public async Task<IActionResult> ValidPasswordToken(TokenForUserVerify token)
        {

            return Ok();
        }

        [HttpPost("new-password")]
        public async Task<IActionResult> SetNewPassword(PasswordForReset token)
        {
            if (token.NewPassword == token.ConfirmPassword)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var id = tokenHandler.ReadJwtToken(token.ResetToken).Claims.FirstOrDefault(p => p.Type == "nameid").Value;
                var username = await _repo.GetUserNameById(Convert.ToInt32(id));
                var user = await _repo.ResetPassword(username, token.NewPassword);
                return Ok(user);
            }
            else
            {
                return Unauthorized("Passwords are different");
            }

        }

        public string Get(string receiver, string token)
        {
            EmailMessage message = new EmailMessage();
            message.Sender = new MailboxAddress("Self", _notificationMetadata.Sender);
            message.Reciever = new MailboxAddress("Self", _notificationMetadata.Sender);
            message.Subject = "Password Reset";
            message.Content = "Otrzymałeś tą wiądomość w związku z prośbą o ustawienie nowego hasła do Twojego konta.\n\n" +
                              "Kliknij w poniższy link lub przeklej go do paska adresu Twojej przeglądarki w celu dokonczenia procesu zmiany hasła:\n\n" +
                              "http://localhost:4200/response-reset-password/" + token + "\n\n" +
                              "Jeżeli nie wysyłałeś prośby o zmianę hasła lub nie chcesz go zmieniać - nie musisz nic robić. Po prostu pozostaw tę wiadomość bez odpowiedzi.\n";
            var mimeMessage = CreateMimeMessageFromEmailMessage(message);
            using (SmtpClient smtpClient = new SmtpClient())
            {
                smtpClient.Connect(_notificationMetadata.SmtpServer,
                _notificationMetadata.Port, true);
                smtpClient.Authenticate(_notificationMetadata.UserName,
                _notificationMetadata.Password);
                smtpClient.Send(mimeMessage);
                smtpClient.Disconnect(true);
            }
            return "Email sent successfully";
        }

        private MimeMessage CreateMimeMessageFromEmailMessage(EmailMessage message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(message.Sender);
            mimeMessage.To.Add(message.Reciever);
            mimeMessage.Subject = message.Subject;
            mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            { Text = message.Content };
            return mimeMessage;
        }
    }
}