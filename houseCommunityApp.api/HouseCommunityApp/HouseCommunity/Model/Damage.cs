using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Model
{
    public class Damage : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public User RequestCreator { get; set; }
        public Building Building { get; set; }
        public DamageStatus Status { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<BlobFile> BlobFiles { get; set; }
    }

    public class BlobFile
    {
        public int Id { get; set; }
        public string FileUrl { get;  set; }
        public string FileName { get;  set; }
        public int DamageId { get; set; }
    }

    public enum DamageStatus
    {
        Unknown, WaitingForFix, Fixed
    }
}
