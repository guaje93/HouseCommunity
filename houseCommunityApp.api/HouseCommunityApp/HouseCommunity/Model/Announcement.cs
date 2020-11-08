using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Model
{
    public class Announcement
    {
        public int Id { get; set; }
        public string Name{ get; set; }
        public string Description{ get; set; }
        public DateTime CreationDate{ get; set; }
        public string Author { get; set; }
        public string FileUrl { get; set; }
        public ICollection<UserAnnouncement> UserAnnouncements { get; set; }
    }
}
