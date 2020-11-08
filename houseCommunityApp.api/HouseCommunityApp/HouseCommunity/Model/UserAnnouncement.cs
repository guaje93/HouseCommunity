using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Model
{
    public class UserAnnouncement
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AnnouncementId { get; set; }
        public User User { get; set; }
        public Announcement Announcement { get; set; }
    }
}
