using HouseCommunity.Data;
using HouseCommunity.Data.Interfaces;
using HouseCommunity.DTOs;
using HouseCommunity.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HouseCommunity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        #region Fields

        private readonly IAnnouncementRepository _repo;
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;

        #endregion

        #region Constructors

        public AnnouncementController(IAnnouncementRepository repo,
                                      IUserRepository userRepository,
                                      IMailService mailService)
        {
            this._repo = repo;
            this._userRepository = userRepository;
            this._mailService = mailService;
        }

        [HttpGet("get-announcements-for-user/{userId}")]
        public IActionResult GetAnnouncementsForUser([FromRoute]int userId)
        {

            var announcements = _repo.GetAnnouncementsForUser(userId);

            if (announcements == null)
                return BadRequest();
          
            return Ok(
                new
                {
                    announcements
                });

        }

        [HttpPost("insert-announcements")]
        public async Task<IActionResult> InsertAnnouncements(AnnouncementForDatabaseInsertDTO announcement)
        {
            var uploader = await _userRepository.GetUser(announcement.UploaderId);
            var announcements = await _repo.InsertAnnouncement(announcement);

            if (announcements == null)
                return BadRequest();

            var messageSubject = "Dodano nowe ogłoszenie we wspólnocie mieszkaniowej";
            var messageContent = $"Nowe ogłoszenie zostało dodane przez użytkownika: {uploader.FirstName} {uploader.LastName}.\n" +
                                 $"Sprawdź zawartość ogłoszenia na swojej tablicy ogłoszeń. \n\n" +
                                 $"Treść maila wygenerowano automatycznie. Nie odpowiadaj na tego maila.";
            _mailService.SendMail(messageSubject, messageContent, "");

            return Ok(
                new
                {
                    announcements
                });

        }

        #endregion //Constructors
    }
}