using HouseCommunity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Data.Interfaces
{
    public interface IMediaRepository
    {
        Task<Model.User> AddMediaForUser(Request.AddMediaToDbRequest userRequest);
        Task<MediaFroDisplayHistoryDTO> GetAllMediaForUser(int id);
    }


}
