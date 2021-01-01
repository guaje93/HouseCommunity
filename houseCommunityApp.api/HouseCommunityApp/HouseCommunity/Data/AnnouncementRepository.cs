using HouseCommunity.Data.Interfaces;
using HouseCommunity.DTOs;
using HouseCommunity.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Data
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        #region Fields

        private readonly DataContext _context;

        #endregion //Fields

        #region Constructors

        public AnnouncementRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        #endregion //Constructors

        public ICollection<Announcement> GetAnnouncementsForUser(int userId)
        {
            var announcements = _context.UserAnnouncements.Include(p => p.Announcement).Include(p => p.User).Where(p => p.UserId == userId).Select(p => p.Announcement).ToList();
            return announcements;
        }

        public async Task<IEnumerable<Announcement>> InsertAnnouncement(AnnouncementForDatabaseInsertDTO announcement)
        {
            var announcements = new List<Announcement>();
            var uploader = _context.Users.FirstOrDefault(p => p.Id == announcement.UploaderId);
            var newAnnouncement = new Announcement()
            {
                Author = uploader.FirstName + " " + uploader.LastName,
                Name = announcement.Name,
                CreationDate = DateTime.Now,
                Description = announcement.Description,
                FileUrl = announcement.FileUrl
            };

            foreach (var receiverId in announcement.ReceiverIds.Distinct())
            {
                var receiver = _context.Users.FirstOrDefault(p => p.Id == receiverId);

                announcements.Add(newAnnouncement);

                _context.UserAnnouncements.Add(new UserAnnouncement()
                {
                    User = receiver,
                    Announcement = newAnnouncement
                }
                );

                await _context.SaveChangesAsync();
            }
            return announcements;
        }
    }
}
