using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HouseCommunity.Data;
using HouseCommunity.Data.Interfaces;
using HouseCommunity.DTOs;
using HouseCommunity.Model;
using HouseCommunity.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HouseCommunity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DamageController : ControllerBase
    {
        #region Fields

        private readonly IDamageRepository _repo;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        private readonly IUserRepository _userRepository;
        private readonly IBuildingRepository _buildingRepository;

        #endregion

        #region Constructors

        public DamageController(IDamageRepository repo, IMapper mapper, IMailService mailService, IUserRepository userRepository, IBuildingRepository buildingRepository)
        {
            this._repo = repo;
            this._mapper = mapper;
            this._mailService = mailService;
            this._userRepository = userRepository;
            this._buildingRepository = buildingRepository;
        }

        #endregion

        [HttpPost]
        public async Task<IActionResult> InsertDamage(AddDamageDTO addDamageDTO)
        {
            var user = await _userRepository.GetUserById(addDamageDTO.UserId);
            var building = await _buildingRepository.GetBuilding(addDamageDTO.BuildingId);

            var damage = new Damage()
            {
                Building = building,
                RequestCreator = user,
                CreationDate = DateTime.Now,
                Description = addDamageDTO.Description,
                Status = DamageStatus.WaitingForFix,
                Title = addDamageDTO.Title,
            };

            var savedDamage = await _repo.AddDamage(damage);

            var messageSubject = $"Użytkownik zgłosił useterkę wymagającą naprawy";
            var messageContent = $"Zgłoszenie zostało dodane przez użytkownika: {user.FirstName} {user.LastName}.\n" +
                                 $"Status zgłoszenia zmieniono na \'Wymagający naprawy\'. W razie zastrzeżeń skontaktuj się z użytkownikiem. \n\n" +
                                 $"Treść maila wygenerowano automatycznie. Nie odpowiadaj na tego maila.";

            _mailService.SendMail(messageSubject, messageContent, "", "Home Community App");

            return Ok(new { savedDamage.Id });
        }

        [HttpPost("update-image")]
        public async Task<IActionResult> UpdateImage(AddImageDTO addDamageDTO)
        {
            var damage = await _repo.GetDamage(addDamageDTO.Id);

            if (damage.BlobFiles == null)
                damage.BlobFiles = new List<BlobFile>();

            damage.BlobFiles.Add(new BlobFile()
            {
                FileName = addDamageDTO.FileName,
                FileUrl = addDamageDTO.FileUrl,
            });

            var savedDamage = await _repo.UpdateDamage(damage);
            return Ok(new { savedDamage.Id });
        }

        [HttpGet("get-damages-for-building/{id}")]
        public async Task<IActionResult> GetDamages(int id)
        {
            var user = await _userRepository.GetUserById(id);
            var damages = _repo.GetDamagesByUserAndStatus(user, DamageStatus.WaitingForFix);
            
            return Ok(damages.Select(p => _mapper.Map<GetDamageForHouseManagerDTO>(p)
            ));
        }

        [HttpGet("get-fixed-damages-for-building/{id}")]
        public async Task<IActionResult> GetFixedDamages(int id)
        {
            var user = await _userRepository.GetUserById(id);
            var damages = _repo.GetDamagesByUserAndStatus(user, DamageStatus.Fixed);
            return Ok(damages.Select(p => _mapper.Map<GetDamageForHouseManagerDTO>(p)
            ));
        }

        [HttpPost("fix-damage")]
        public async Task<IActionResult> FixDamage([FromBody]int id)
        {
            var damage = await _repo.GetDamage(id);
            var user = damage.Building.HouseManager;
            damage.Status = DamageStatus.Fixed;
            var savedDamage = await _repo.UpdateDamage(damage);
            
            var messageSubject = "Zarządca budynku zmienił status zgłoszenia na \'Naprawiony\'.";
            var messageContent = $"Zgłoszenie zostało zminione przez użytkownika: {user.FirstName} {user.LastName}.\n" +
                                 $"Status zgłoszenia zmieniono na \'Naprawiony\'. W razie zastrzeżeń skontaktuj się z zarządcą budynku. \n\n" +
                                 $"Treść maila wygenerowano automatycznie. Nie odpowiadaj na tego maila.";

            _mailService.SendMail(messageSubject, messageContent, "", "Home Community App");
            return Ok(_mapper.Map<GetDamageForHouseManagerDTO>(damage)
            );
        }

        [HttpPost("revert-fix")]
        public async Task<IActionResult> RevertDamage([FromBody]int id)
        {
            var damage = await _repo.GetDamage(id);           
            var user = damage.Building.HouseManager;
            damage.Status = DamageStatus.WaitingForFix;
            var savedDamage = await _repo.UpdateDamage(damage);

            var messageSubject = "Zarządca budynku zmienił status zgłoszenia na \'Wymagający naprawy\'.";
            var messageContent = $"Zgłoszenie zostało zminione przez użytkownika: {user.FirstName} {user.LastName}.\n" +
                                 $"Status zgłoszenia zmieniono na \'Wymagający naprawy\'. W razie zastrzeżeń skontaktuj się z zarządcą budynku. \n\n" +
                                 $"Treść maila wygenerowano automatycznie. Nie odpowiadaj na tego maila.";

            _mailService.SendMail(messageSubject, messageContent, "", "Home community App");

            return Ok(_mapper.Map<GetDamageForHouseManagerDTO>(savedDamage)
            );
        }
    }
}
