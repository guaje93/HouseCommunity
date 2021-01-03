using HouseCommunity.Data;
using HouseCommunity.Data.Interfaces;
using HouseCommunity.DTOs;
using HouseCommunity.Helpers;
using HouseCommunity.Request;
using HouseCommunity.Services;
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
        #region Fields

        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private readonly IBuildingRepository _buildingRepository;
        private readonly IMailService _mailService;
        private readonly NotificationMetadata _notificationMetadata;

        #endregion

        #region Constructors

        public AuthController(IAuthRepository repo,
                              IConfiguration config,
                              IUserRepository userRepository,
                              IBuildingRepository buildingRepository,
                              IMailService mailService,
                              NotificationMetadata notificationMetadata)
        {
            this._repo = repo;
            this._config = config;
            this._userRepository = userRepository;
            this._buildingRepository = buildingRepository;
            this._mailService = mailService;
            this._notificationMetadata = notificationMetadata;
        }

        #endregion //Constructors

        #region Methods


        [HttpPost("login")]
        public async Task<IActionResult> LogIn(UserForLoginDTO userForLoginDTO)
        {
            var userFromRepo = await _repo.LogIn(userForLoginDTO.UserName.ToLower(), userForLoginDTO.Password);

            if (userFromRepo == null)
                return Unauthorized("Hasło lub uzytkownik jest niepoprawne!");

            SecurityTokenDescriptor tokenDescriptor = GetTokenDescriptor(userFromRepo);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(
                new
                {
                    token = tokenHandler.WriteToken(token),
                    user = new UserForLogInDueToPermissions() { Id = userFromRepo.Id, UserRole = userFromRepo.UserRole }
                });

        }

        [HttpPost("register-new")]
        public async Task<IActionResult> Register(UserForRegisterDTO userForRegisterDTO)
        {
            var creator = await _userRepository.GetUserById(userForRegisterDTO.UserId);
            var users = await _userRepository.GetUsersByRole(Model.UserRole.Resident);
            if(users.Any(p => p.Email == userForRegisterDTO.Email))
            {
                return Unauthorized("Podany email jest już w bazie");
            }
            var userFromRepo = await _repo.RegisterUser(userForRegisterDTO);

            if (userFromRepo == null)
                return Forbid("Wystąpił błąd. Użytkownik nie został dodany.");

            else
            {
                SecurityTokenDescriptor tokenDescriptor = GetTokenDescriptor(userFromRepo);
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenToSend = tokenHandler.WriteToken(token);

                var messageSubject = "Home Community App - Konto zostało stworzone";
                var messageContent = $"Użytkownik {creator.FirstName} {creator.LastName} stworzył konto powiązane z danym adresem mailowym." +
                                      "Kliknij w poniższy link lub przeklej go do paska adresu Twojej przeglądarki w celu dokonczenia procesu rejestracji:\n\n" +
                                      "https://housecommunityappfront.azurewebsites.net/response-reset-password/" + tokenToSend + "\n\n" +
                                      "Jeżeli nie wysyłałeś prośby o zmianę hasła lub nie chcesz go zmieniać - nie musisz nic robić. Po prostu pozostaw tę wiadomość bez odpowiedzi.\n";
                _mailService.SendMail(messageSubject, messageContent, "", "Home Community App");
            }

            return Ok(
                new
                {
                    Result = "Użytkownik został dodany"
                });

        }

        [HttpPost("register-existing")]
        public async Task<IActionResult> RegisterAddExistig(UserForRegisterExistingDTO userForRegisterDTO)
        {
            var creator = await _userRepository.GetUserById(userForRegisterDTO.UserId);
            var users = await _userRepository.GetUsersByRole(Model.UserRole.Resident);
            if (!users.Any(p => p.Email == userForRegisterDTO.Email))
            {
                return Unauthorized("Nie znaleziono użytkownika");
            }
            var userFromRepo = await _buildingRepository.RegisterFlat(userForRegisterDTO);

            if (userFromRepo == null)
                return Forbid("Wystąpił błąd. Użytkownik nie został dodany.");

            else
            {
                var messageSubject = "Home Community App - Mieszkanie zostało dodane do konta";
                var messageContent = $"Użytkownik {creator.FirstName} {creator.LastName} dodał mieszkanie do danego konta.";
                _mailService.SendMail(messageSubject, messageContent, "", "Home Community App");
            }

            return Ok(
                new
                {
                    Result = "Użytkownik został dodany"
                });

        }

        [HttpPost("req-reset-password")]
        public async Task<IActionResult> ResetPasswordRequest(UserForPasswordResetRequest usernameuserForLoginDTO)
        {

            var userFromRepo = await _userRepository.GetUserByEmail(usernameuserForLoginDTO.Email.ToLower());
            if (userFromRepo == null)
                return Unauthorized("Email nie jset powiązany z żadnym z kont!");

            SecurityTokenDescriptor tokenDescriptor = GetTokenDescriptor(userFromRepo);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenToSend = tokenHandler.WriteToken(token);

            var messageSubject = "Password Reset";
            var messageContent = "Otrzymałeś tą wiądomość w związku z prośbą o ustawienie nowego hasła do Twojego konta.\n\n" +
                              "Kliknij w poniższy link lub przeklej go do paska adresu Twojej przeglądarki w celu dokonczenia procesu zmiany hasła:\n\n" +
                              "https://housecommunityappfront.azurewebsites.net" + tokenToSend + "\n\n" +
                              "Jeżeli nie wysyłałeś prośby o zmianę hasła lub nie chcesz go zmieniać - nie musisz nic robić. Po prostu pozostaw tę wiadomość bez odpowiedzi.\n";


            _mailService.SendMail(messageSubject, messageContent, "", "Home Community App");
            return Ok();
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePasswordRequest(PasswordChangeRequest passwordChangeRequest)
        {

            var userFromRepo = await _repo.ChangePassword(passwordChangeRequest);
            if (userFromRepo == null)
                return Unauthorized("Podane hasło jest niepoprawne!");

            return Ok();
        }

        [HttpPost("valid-password-token")]
        public async Task<IActionResult> ValidPasswordToken(TokenForUserVerify token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var id = tokenHandler.ReadJwtToken(token.ResetToken).Claims.FirstOrDefault(p => p.Type == "nameid").Value;
            var intId = 0;
            Int32.TryParse(id, out intId);

            var user = await _userRepository.GetUserById(intId);
            if (user == null)
                return Unauthorized("Token niepoprawny");

            return Ok();
        }

        [HttpPost("new-password")]
        public async Task<IActionResult> SetNewPassword(PasswordForReset token)
        {
            if (token.NewPassword == token.ConfirmPassword)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var id = tokenHandler.ReadJwtToken(token.ResetToken).Claims.FirstOrDefault(p => p.Type == "nameid").Value;
                var user1 = await _userRepository.GetUserById(Convert.ToInt32(id));
                var savedUser = await _repo.ResetPassword(user1.UserName, token.NewPassword);
                return Ok(savedUser);
            }
            else
            {
                return Unauthorized("Passwords are different");
            }

        }

        private SecurityTokenDescriptor GetTokenDescriptor(Model.User userFromRepo)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.FirstName)            };

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

        #endregion //Methods
    }
}