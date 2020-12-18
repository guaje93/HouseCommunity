using HouseCommunity.DTOs;
using HouseCommunity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Data.Interfaces
{
    public interface IBuildingRepository
    {
        Task<ICollection<FlatsForListDTO>> GetFlats(int userId);
        Task <Building> GetBuilding(int userId);
    }
}
