using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseCommunity.DTOs;
using HouseCommunity.Model;

namespace HouseCommunity.Data.Interfaces
{
    public interface IDamageRepository
    {
        Task<Damage> AddDamage(AddDamageDTO addDamageDTO);
        Task<Damage> AddImage(AddImageDTO addDamageDTO);
        IEnumerable<Damage> GetDamagesForHouseManager(int id, DamageStatus status);
        Task<Damage> ChangeStatus(int id, DamageStatus status);
    }
}
