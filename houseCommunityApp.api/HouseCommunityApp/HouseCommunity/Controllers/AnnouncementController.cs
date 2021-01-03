using HouseCommunity.Data;
using HouseCommunity.Data.Interfaces;
using HouseCommunity.DTOs;
using HouseCommunity.Model;
using HouseCommunity.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        #endregion //Constructors

        #region Methods

        [HttpGet("get-announcements-for-user/{userId}")]
        public IActionResult GetAnnouncementsForUser([FromRoute]int userId)
        {
            var announcements = _repo.GetAnnouncementsForUser(userId);

            if (announcements == null)
                return BadRequest();

            return Ok(new { announcements });
        }

        [HttpPost("insert-announcements")]
        public async Task<IActionResult> InsertAnnouncements(AnnouncementForDatabaseInsertDTO announcement)
        {
            var uploader = await _userRepository.GetUserById(announcement.UploaderId);
            Announcement savedAnnouncement = null;

            var newAnnouncement = new Announcement()
            {
                Author = uploader.FirstName + " " + uploader.LastName,
                Name = announcement.Name,
                CreationDate = DateTime.Now,
                Description = announcement.Description,
                FileUrl = announcement.FileUrl
            };

            var receivers = new List<User>();
            foreach (var id in announcement.ReceiverIds)
            {
                var user = await _userRepository.GetUserById(id);
                receivers.Add(user);
            }

            savedAnnouncement = await _repo.InsertAnnouncement(newAnnouncement, receivers);

            if (savedAnnouncement == null)
                return BadRequest();

            var messageSubject = "Dodano nowe ogłoszenie we wspólnocie mieszkaniowej";
            var messageContent = $"Nowe ogłoszenie zostało dodane przez użytkownika: {uploader.FirstName} {uploader.LastName}.\n" +
                                 $"Sprawdź zawartość ogłoszenia na swojej tablicy ogłoszeń. \n\n" +
                                 $"Treść maila wygenerowano automatycznie. Nie odpowiadaj na tego maila.";

            _mailService.SendMail(messageSubject, messageContent, "", "Home Community App");

            return Ok(new { savedAnnouncement });

        }

        #endregion //Constructors
    }
}