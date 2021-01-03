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
        Task<ICollection<Flat>> GetFlats();
        Task<Flat> GetFlat(int flatId);
        Task<Building> GetBuildingByManager(User manager);
        Task <Building> GetBuilding(int userId);
        Task <Flat> RegisterFlat(UserForRegisterExistingDTO userForRegisterDTO);
        Task <ICollection<Building>> GetBuildings();
        Task <Flat> UpdateFlat(Flat flat);
    }
}
