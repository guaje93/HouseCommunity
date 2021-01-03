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
    public class AnnouncementRepository : BaseRepository<Announcement>, IAnnouncementRepository
    {

        #region Constructors

        public AnnouncementRepository(DataContext dataContext) : base(dataContext)
        {
        }

        #endregion //Constructors

        public ICollection<Announcement> GetAnnouncementsForUser(int userId)
        {
            var announcements = GetAll(x => x.Include(p => p.UserAnnouncements)
                                             .ThenInclude(p => p.User)
                                             .Include(p => p.UserAnnouncements)
                                             .ThenInclude(p => p.Announcement));

            announcements = announcements.SelectMany(p => p.UserAnnouncements)
                                         .Where(p => p.UserId == userId)
                                         .Select(p => p.Announcement);
            return announcements.ToList();
        }

        public async Task<Announcement> InsertAnnouncement(Announcement announcement, IEnumerable<User> users)
        {
            var userAnnouncements = users.Select(user => new UserAnnouncement()
            {
                User = user,
                Announcement = announcement
            });

                announcement.UserAnnouncements = userAnnouncements.ToList();

            await AddAsync(announcement);

            return announcement;
        }

    }
}
