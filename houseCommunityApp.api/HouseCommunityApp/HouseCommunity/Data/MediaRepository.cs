using HouseCommunity.Data.Interfaces;
using HouseCommunity.DTOs;
using HouseCommunity.Model;
using HouseCommunity.Request;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Data
{
    public class MediaRepository : IMediaRepository
    {
        #region Fields

        private readonly DataContext _context;

        #endregion //Fields

        #region Constructors

        public MediaRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        #endregion //Constructors

        public async Task<User> AddMediaForUser(AddMediaToDbRequest userRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == userRequest.UserId);
            if (user.Flat.MediaHistory is null)
                user.Flat.MediaHistory = new List<Model.Media>();
            MediaEnum mediaType = MediaEnum.Undefined;
            switch (userRequest.MediaType.ToLower())
            {
                case "coldwater":
                    {
                        mediaType = MediaEnum.ColdWater; break;
                    }
                case "hotwater":
                    {
                        mediaType = MediaEnum.HotWater; break;
                    }
            }

            user.Flat.MediaHistory.Add(new Model.Media()
            {
                ImageUrl = userRequest.ImageUrl,
                CreationDate = DateTime.Now,
                MediaType = mediaType,
                UserDescription = userRequest.UserDescription,
                CurrentValue = userRequest.CurrentValue
            }
            );
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<MediaFroDisplayHistoryDTO> GetAllMediaForUser(int id)
        {
            var user = await _context.Users.Include(p => p.Flat.MediaHistory).FirstOrDefaultAsync(p => p.Id == id);
            return new MediaFroDisplayHistoryDTO()
            {
                SingleMediaItems = user.Flat.MediaHistory.Select(p => new SingleMediaItem()
                {
                    ImageUrl = p.ImageUrl,
                    FileName = p.FileName,
                    CreationDate = p.CreationDate,
                    Description = p.UserDescription,
                    CurrentValue = p.CurrentValue,
                    AcceptanceDate = p.AcceptanceDate,
                    MediaEnum = p.MediaType
                }).ToList()
            };
        }
    }
}
