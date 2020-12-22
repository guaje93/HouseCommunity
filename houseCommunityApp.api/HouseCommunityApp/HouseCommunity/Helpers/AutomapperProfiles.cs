using System.Linq;
using AutoMapper;
using HouseCommunity.DTOs;
using HouseCommunity.Model;

namespace HouseCommunity.Helpers
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<Payment, PaymentForBookDTO>();
            CreateMap<PaymentForBookDTO, Payment>();

            CreateMap<User, UserForInfoDTO>().ForMember(dest => dest.Area, opt => opt.MapFrom(source => source.Flat.Area))
                                             .ForMember(dest => dest.ColdWaterEstimatedUsage, opt => opt.MapFrom(source => source.Flat.ColdWaterEstimatedUsage))
                                             .ForMember(dest => dest.ColdWaterUnitCost, opt => opt.MapFrom(source => source.Flat.Building.Cost.ColdWaterUnitCost))
                                             .ForMember(dest => dest.HeatingEstimatedUsage, opt => opt.MapFrom(source => source.Flat.HeatingEstimatedUsage))
                                             .ForMember(dest => dest.HeatingUnitCost, opt => opt.MapFrom(source => source.Flat.Building.Cost.HeatingUnitCost))
                                             .ForMember(dest => dest.HotWaterEstimatedUsage, opt => opt.MapFrom(source => source.Flat.Building.Cost.HotWaterUnitCost))
                                             .ForMember(dest => dest.HotWaterUnitCost, opt => opt.MapFrom(source => source.Flat.Building.Cost.HotWaterUnitCost))
                                             .ForMember(dest => dest.ResidentsAmount, opt => opt.MapFrom(source => source.Flat.ResidentsAmount));

            CreateMap<User, ResidentsForListDTO>().ForMember(dest => dest.Address, opt => opt.MapFrom(source => source.Flat.Building.Address.ToString()))
                                                  .ForMember(dest => dest.BuildingId, opt => opt.MapFrom(source => source.Flat.Building.Id))
                                                  .ForMember(dest => dest.FlatId, opt => opt.MapFrom(source => source.Flat.Id))
                                                  .ForMember(dest => dest.HousingDevelopmentId, opt => opt.MapFrom(source => source.Flat.Building.HousingDevelopmentId))
                                                  .ForMember(dest => dest.HousingDevelopmentName, opt => opt.MapFrom(source => source.Flat.Building.HousingDevelopment.Name))
                                                  .ForMember(dest => dest.LocalNumber, opt => opt.MapFrom(source => source.Flat.FlatNumber))
                                                  .ForMember(dest => dest.Name, opt => opt.MapFrom(source => $"{source.FirstName} {source.LastName}"))
                                                  .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(source => source.Email))
                                                  .ForMember(dest => dest.UserId, opt => opt.MapFrom(source => source.Id));

            CreateMap<PaymentForBookDTO, Payment>();

            CreateMap<Damage, GetDamageForHouseManagerDTO>().ForMember(dest => dest.User, opt => opt.MapFrom(source => $"{source.RequestCreator.FirstName} {source.RequestCreator.LastName}, Mieszkanie nr {source.RequestCreator.Flat.FlatNumber}"))
                                                            .ForMember(dest => dest.FilesPaths, opt => opt.MapFrom(source => source.BlobFiles.Select(p => p.FileUrl)));

            CreateMap<Message, MessageDTO>();


            //FirstName = user.FirstName,
            //    LastName = user.LastName,
            //    Birthdate = user.Birthdate,
            //    Id = user.Id,
            //    Email = user.Email,
            //    PhoneNumber = user.PhoneNumber,
            //    Area = user.Flat.Area,
            //    ResidentsAmount = user.Flat.ResidentsAmount,
            //    ColdWaterEstimatedUsage = user.Flat.ColdWaterEstimatedUsage,
            //    HotWaterEstimatedUsage = user.Flat.HotWaterEstimatedUsage,
            //    HeatingEstimatedUsage = user.Flat.HeatingEstimatedUsage,
            //    ColdWaterUnitCost = user.Flat.Building.Cost.ColdWaterUnitCost,
            //    HotWaterUnitCost = user.Flat.Building.Cost.HotWaterUnitCost,
            //    HeatingUnitCost = user.Flat.Building.Cost.HeatingUnitCost,
        }

    }
}