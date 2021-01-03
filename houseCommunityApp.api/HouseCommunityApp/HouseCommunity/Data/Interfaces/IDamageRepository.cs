using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseCommunity.DTOs;
using HouseCommunity.Model;

namespace HouseCommunity.Data.Interfaces
{
    public interface IDamageRepository : IBaseRepository<Damage>
    {
        Task<Damage> GetDamage(int id);
        Task<Damage> AddDamage(Damage damage);
        Task<Damage> UpdateDamage(Damage damage);
        IEnumerable<Damage> GetDamagesByUserAndStatus(User user, DamageStatus status);
    }
}
