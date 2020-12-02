using HouseCommunity.Controllers;
using HouseCommunity.DTOs;
using HouseCommunity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Data.Interfaces
{
    public interface IMediaRepository
    {
        Task<Model.User> AddMediaForUser(Request.AddMediaToDbRequest userRequest);
        Task<Model.Flat> CreateEmptyMediaForUser(Request.AddEmptyMediaRequest userRequest);
        Task<MediaForUsrDisplayDTO> GetAllMediaForUser(int id);
        Task<IEnumerable<MediaForAndministrationDTO>> GetMedia(int id);
        Task<Media> UpdateMedia(MediaUpdatedByAdministrationDTO addMediaToDbRequest);
        Task<Media> UpdateMedia(MediaUpdatedByUserDTO addMediaToDbRequest);
        Task<Media> UnlockMedia(MediaUpdatedByAdministrationDTO addMediaToDbRequest);
    }


}
