﻿using HouseCommunity.DTOs;
using HouseCommunity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Data.Interfaces
{
    public interface IAnnouncementRepository : IBaseRepository<Announcement>
    {
        ICollection<Announcement> GetAnnouncementsForUser(int userId);
        Task<Announcement> InsertAnnouncement(Announcement announcement, IEnumerable<User> receiver);
    }
}
