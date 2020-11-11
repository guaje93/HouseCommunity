using HouseCommunity.Data.Interfaces;
using HouseCommunity.DTOs;
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

        #endregion

        #region Constructors

        public AnnouncementController(IAnnouncementRepository repo)
        {
            this._repo = repo;
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
        public IActionResult InsertAnnouncements(AnnouncementForDatabaseInsertDTO announcement)
        {

            var announcements = _repo.InsertAnnouncement(announcement);

            if (announcements == null)
                return BadRequest();

            return Ok(
                new
                {
                    announcements
                });

        }

        #endregion //Constructors
    }
}