using HouseCommunity.Data.Interfaces;
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
    }
}
