using HouseCommunity.Controllers;
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
            var flat = await _context.Flats.Include(p => p.MediaHistory).FirstOrDefaultAsync(p => p.Id == userRequest.FlatId);
            var user = await _context.Users.Include(p => p.UserFlats).ThenInclude(p => p.Flat).ThenInclude(p => p.MediaHistory).FirstOrDefaultAsync(p => p.Id == userRequest.UserId);
            if (flat.MediaHistory is null)
                flat.MediaHistory = new List<Model.Media>();
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

            flat.MediaHistory.Add(new Model.Media()
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
            var startDate = GenerateStartDateFromCurrentDate(userRequest.Period);
            var endDate = GenerateEndDateFromCurrentDate(userRequest.Period);

            var coldWaterLastValue = 0.0;
            var hotWaterLastValue = 0.0;
            var heatingLastValue = 0.0;
            if (flat.MediaHistory?.Count() > 0)
            {
                coldWaterLastValue = flat.MediaHistory.Where(p => p.MediaType == MediaEnum.ColdWater).OrderByDescending(p => p.EndPeriodDate).First().CurrentValue;
                hotWaterLastValue = flat.MediaHistory.Where(p => p.MediaType == MediaEnum.HotWater).OrderByDescending(p => p.EndPeriodDate).First().CurrentValue;
                heatingLastValue = flat.MediaHistory.Where(p => p.MediaType == MediaEnum.Heat).OrderByDescending(p => p.EndPeriodDate).First().CurrentValue;
            }

            if (!flat.MediaHistory.Select(p => p.EndPeriodDate).Any(p => p >= startDate && p <= endDate))
            {
                AddMediaTemplate(flat, startDate, endDate, coldWaterLastValue, MediaEnum.ColdWater);
                AddMediaTemplate(flat, startDate, endDate, hotWaterLastValue, MediaEnum.HotWater);
                AddMediaTemplate(flat, startDate, endDate, heatingLastValue, MediaEnum.Heat);
                _context.SaveChanges();
                return flat;
            }
            else
                return null;
        }

        private Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Media> AddMediaTemplate(Flat flat, DateTime startDate, DateTime endDate, double lastValue, MediaEnum mediaEnum)
        {
            return _context.MediaHistory.Add(new Media()
            {
                Flat = flat,
                MediaType = mediaEnum,
                StartPeriodDate = startDate,
                EndPeriodDate = endDate,
                LastValue = lastValue,
                Status = MediaStatus.WaitingForUser
            }


                            );
        }

        private DateTime GenerateEndDateFromCurrentDate(string period)
        {
            bool isFirstHalf = period.Substring(0, 3) == "H01";
            var currentYear = Convert.ToInt32(period.Substring(3));

            if (isFirstHalf)
                return new DateTime(currentYear, 6, 30);
            else
                return new DateTime(currentYear, 12, 31);
        }

        private DateTime GenerateStartDateFromCurrentDate(string period)
        {
            bool isFirstHalf = period.Substring(0, 3) == "H01";
            var currentYear = Convert.ToInt32(period.Substring(3));

            if (isFirstHalf)
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

        public async Task<MediaForUsrDisplayDTO> GetAllMediaForUser(int id)
        {
            var user = await _context.Users.Include(p => p.UserFlats)
                                           .ThenInclude(p=> p.Flat)
                                           .ThenInclude(p => p.MediaHistory)
                                           .Include(p => p.UserFlats)
                                           .ThenInclude(p => p.Flat)
                                           .ThenInclude(p => p.Building)
                                           .ThenInclude(p => p.Address)
                                           .FirstOrDefaultAsync(p => p.Id == id);
            return new MediaForUsrDisplayDTO()
            {
                SingleMediaItems = user.UserFlats.SelectMany(p => p.Flat.MediaHistory).Select(p => new SingleMediaItem()
                {
                    Id = p.Id,
                    ImageUrl = p.ImageUrl,
                    FlatAddress = p.Flat.Building.Address.ToString() + " m. " + p.Flat.FlatNumber,
                    CreationDate = p.CreationDate,
                    Description = p.UserDescription,
                    CurrentValue = p.CurrentValue,
                    LastValue = p.LastValue,
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
                    AcceptanceDate = p.AcceptanceDate,
                    CurrentValue = p.CurrentValue,
                    LastValue = p.LastValue,
                    Period = p.StartPeriodDate.Month > 6 ? $"H02{p.EndPeriodDate.Year}" : $"H01{p.EndPeriodDate.Year}",
                    Id = p.Id,
                    ImageUrl = p.ImageUrl,
                    MediaType = p.MediaType,
                    UserDescription = p.UserDescription,
                    Status = p.Status
                });

            else return null;
        }

        public async Task<Media> UpdateMedia(MediaUpdatedByUserDTO addMediaToDbRequest)
        {
            var media = await _context.MediaHistory.FirstOrDefaultAsync(p => p.Id == addMediaToDbRequest.Id);
            if (media != null)
            {
                media.ImageUrl = addMediaToDbRequest.ImageUrl;
                media.CreationDate = DateTime.Now;
                media.CurrentValue = addMediaToDbRequest.CurrentValue;
                media.UserDescription = addMediaToDbRequest.UserDescription;
                media.Status = MediaStatus.UpdatedByUser;

                _context.Update(media);
                await _context.SaveChangesAsync();
                return media;
            }
            return null;
        }

        public async Task<Media> UpdateMedia(MediaUpdatedByAdministrationDTO addMediaToDbRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == addMediaToDbRequest.UserId);
            //2 = Administration
            if (user.UserRole == UserRole.Administrator)
            {
                var media = await _context.MediaHistory.FirstOrDefaultAsync(p => p.Id == addMediaToDbRequest.MediaId);
                if (media != null)
                {

                    media.Status = MediaStatus.AcceptedByAdministration;
                    media.CurrentValue = addMediaToDbRequest.CurrentValue;
                    media.AcceptanceDate = DateTime.Now;
                    _context.Update(media);
                    await _context.SaveChangesAsync();
                    return media;
                }
            }

            return null;
        }

        public async Task<Media> UnlockMedia(MediaUpdatedByAdministrationDTO addMediaToDbRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == addMediaToDbRequest.UserId);
            //2 = Administration
            if (user.UserRole == UserRole.Administrator)
            {
                var media = await _context.MediaHistory.FirstOrDefaultAsync(p => p.Id == addMediaToDbRequest.MediaId);
                if (media != null)
                {

                    media.Status = MediaStatus.UpdatedByUser;
                    media.AcceptanceDate = null;
                    _context.Update(media);
                    await _context.SaveChangesAsync();
                    return media;
                }
            }

            return null;
        }
    }
}
