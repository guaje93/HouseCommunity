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
            var user = await _context.Users.Include(p => p.Flat).ThenInclude(p => p.MediaHistory).FirstOrDefaultAsync(p => p.Id == userRequest.UserId);
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

        public async Task<Flat> CreateEmptyMediaForUser(AddEmptyMediaRequest userRequest)
        {
            var flat = await _context.Flats.Include(p => p.MediaHistory).FirstOrDefaultAsync(p => p.Id == userRequest.FlatId);
            var startDate = GenerateStartDateFromCurrentDate();
            var endDate = GenerateEndDateFromCurrentDate();
            if (!_context.MediaHistory.Select(p => p.EndPeriodDate).Any(p => p >= startDate && p <= endDate))
            {
                AddMediaTemplate(flat, startDate, endDate, MediaEnum.ColdWater);
                AddMediaTemplate(flat, startDate, endDate, MediaEnum.HotWater);
                AddMediaTemplate(flat, startDate, endDate, MediaEnum.Heat);
                _context.SaveChanges();
                return flat;
            }
            else
                return null;
        }

        private Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Media> AddMediaTemplate(Flat flat, DateTime startDate, DateTime endDate, MediaEnum mediaEnum)
        {
            return _context.MediaHistory.Add(new Media()
            {
                Flat = flat,
                MediaType = mediaEnum,
                StartPeriodDate = startDate,
                EndPeriodDate = endDate,
                Status = MediaStatus.WaitingForUser
            }


                            );
        }

        private DateTime GenerateEndDateFromCurrentDate()
        {
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;
            if (currentMonth < 7)
                return new DateTime(currentYear, 6, 30);
            else
                return new DateTime(currentYear, 12, 31);
        }

        private DateTime GenerateStartDateFromCurrentDate()
        {
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;
            if (currentMonth < 7)
                return new DateTime(currentYear, 1, 1);
            else
                return new DateTime(currentYear, 7, 1);
        }

        private MediaEnum GetMediaEnum(int mediaType)
        {
            switch (mediaType)
            {
                case 1: return MediaEnum.ColdWater;
                case 2: return MediaEnum.HotWater;
                case 3: return MediaEnum.Heat;
                default: return MediaEnum.Undefined;
            }
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
                    MediaEnum = p.MediaType,
                    Status = p.Status,
                    StartPeriodDate = p.StartPeriodDate,
                    EndPeriodDate = p.EndPeriodDate
                }).ToList()
            };
        }

        public async Task<IEnumerable<MediaForAndministrationDTO>> GetMedia(int id)
        {
            var flat = await _context.Flats
                                     .Include(p => p.MediaHistory)
                                     .FirstOrDefaultAsync(p => p.Id == id);

            if (flat != null)
                return flat.MediaHistory.Select(p => new MediaForAndministrationDTO()
                {
                    CreationDate = p.CreationDate,
                    CurrentValue = p.CurrentValue,
                    EndPeriodDate = p.EndPeriodDate,
                    FileName = p.FileName,
                    Id = p.Id,
                    ImageUrl = p.ImageUrl,
                    MediaType = p.MediaType,
                    StartPeriodDate = p.StartPeriodDate,
                    UserDescription = p.UserDescription,
                    Status = p.Status
                });

            else return null;
        }
    }
}
