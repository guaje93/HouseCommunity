using HouseCommunity.DTOs;
using HouseCommunity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Data.Interfaces
{
    public interface IAnnouncementRepository
    {
        ICollection<Announcement> GetAnnouncementsForUser(int userId);
        Task<IEnumerable<Announcement>> InsertAnnouncement(AnnouncementForDatabaseInsertDTO announcement);
    }
}
