using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.DTOs
{
    public class AnnouncementForDatabaseInsertDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int UploaderId { get; set; }
        public ICollection<int> ReceiverIds{ get; set; }
        public string FileUrl { get; set; }
    }
}
