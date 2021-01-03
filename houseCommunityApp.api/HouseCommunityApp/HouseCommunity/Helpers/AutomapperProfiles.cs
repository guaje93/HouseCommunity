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

            CreateMap<UserFlat, ResidentToContactDTO>()
                                             .ForMember(dest => dest.Email, opt => opt.MapFrom(source => source.User.Email))
                                             .ForMember(dest => dest.FirstName, opt => opt.MapFrom(source => source.User.FirstName))
                                             .ForMember(dest => dest.LastName, opt => opt.MapFrom(source => source.User.LastName))
                                             .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(source => source.User.PhoneNumber));

            CreateMap<User, UserForInfoDTO>();
            CreateMap<User, UserNamesListDTO>();

            CreateMap<Flat, FlatsForListDTO>()
                                             .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                                             .ForMember(dest => dest.Address, opt => opt.MapFrom(source => source.Building.Address.ToString() + " m." + source.FlatNumber));

            CreateMap<UserFlat, FlatForFilteringDTO>()
                                            .ForMember(dest => dest.FlatId, opt => opt.MapFrom(source => source.Flat.Id))
                                            .ForMember(dest => dest.BuildingId, opt => opt.MapFrom(source => source.Flat.Building.Id))
                                            .ForMember(dest => dest.HousingDevelopmentId, opt => opt.MapFrom(source => source.Flat.Building.HousingDevelopment.Id))
                                            .ForMember(dest => dest.HousingDevelopmentName, opt => opt.MapFrom(source => source.Flat.Building.HousingDevelopment.Name))
                                            .ForMember(dest => dest.Address, opt => opt.MapFrom(source => source.Flat.Building.Address.ToString()))
                                            .ForMember(dest => dest.LocalNumber, opt => opt.MapFrom(source => source.Flat.FlatNumber))
                                            .ForMember(dest => dest.UserId, opt => opt.MapFrom(source => source.User.Id))
                                            .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.User.FirstName + " " +source.User.LastName ))
                                            .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(source => source.User.Email));


            CreateMap<UserFlat, FlatForInfoDTO>()
                                             .ForMember(dest => dest.FlatName, opt => opt.MapFrom(source => source.Flat.Building.Address.ToString() + " m." + source.Flat.FlatNumber))
                                             .ForMember(dest => dest.Area, opt => opt.MapFrom(source => source.Flat.Area))
                                             .ForMember(dest => dest.ColdWaterEstimatedUsage, opt => opt.MapFrom(source => source.Flat.ColdWaterEstimatedUsage))
                                             .ForMember(dest => dest.ColdWaterUnitCost, opt => opt.MapFrom(source => source.Flat.Building.Cost.ColdWaterUnitCost))
                                             .ForMember(dest => dest.HeatingEstimatedUsage, opt => opt.MapFrom(source => source.Flat.HeatingEstimatedUsage))
                                             .ForMember(dest => dest.HeatingUnitCost, opt => opt.MapFrom(source => source.Flat.Building.Cost.HeatingUnitCost))
                                             .ForMember(dest => dest.HotWaterEstimatedUsage, opt => opt.MapFrom(source => source.Flat.HotWaterEstimatedUsage))
                                             .ForMember(dest => dest.HotWaterUnitCost, opt => opt.MapFrom(source => source.Flat.Building.Cost.HotWaterUnitCost))
                                             .ForMember(dest => dest.ResidentsAmount, opt => opt.MapFrom(source => source.Flat.ResidentsAmount))
                                             .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Flat.Id));



            CreateMap<PaymentForBookDTO, Payment>();
            CreateMap<ResidentsForRegisterDTO, User>();
            CreateMap<User, ResidentsForRegisterDTO>();

            CreateMap<Damage, GetDamageForHouseManagerDTO>().ForMember(dest => dest.User, opt => opt.MapFrom(source => $"{source.RequestCreator.FirstName} {source.RequestCreator.LastName} - {source.RequestCreator.Email}"))
                                                            .ForMember(dest => dest.FilesPaths, opt => opt.MapFrom(source => source.BlobFiles.Select(p => p.FileUrl)));

            CreateMap<Message, MessageDTO>();

        }

    }
}