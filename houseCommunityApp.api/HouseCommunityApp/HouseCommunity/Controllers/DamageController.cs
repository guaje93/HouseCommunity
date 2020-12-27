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

        #endregion

        #region Constructors

        public DamageController(IDamageRepository repo, IMapper mapper, IMailService mailService, IUserRepository userRepository)
        {
            this._repo = repo;
            this._mapper = mapper;
            this._mailService = mailService;
            this._userRepository = userRepository;
        }

        #endregion

        [HttpPost]
        public async Task<IActionResult> InsertDamage(AddDamageDTO addDamageDTO)
        {
            var damage = await _repo.AddDamage(addDamageDTO);
            var user = await _userRepository.GetUser(addDamageDTO.UserId);
            var messageSubject = $"Użytkownik zgłosił useterkę wymagającą naprawy";
            var messageContent = $"Zgłoszenie zostało dodane przez użytkownika: {user.FirstName} {user.LastName}.\n" +
                                 $"Status zgłoszenia zmieniono na \'Wymagający naprawy\'. W razie zastrzeżeń skontaktuj się z użytkownikiem. \n\n" +
                                 $"Treść maila wygenerowano automatycznie. Nie odpowiadaj na tego maila.";

            _mailService.SendMail(messageSubject, messageContent, "");

            return Ok(new
            {
                damage.Id
            }
            );
        }

        [HttpPost("update-image")]
        public async Task<IActionResult> UpdateImage(AddImageDTO addDamageDTO)
        {
            var damage = await _repo.AddImage(addDamageDTO);
            return Ok(new
            {
                damage.Id
            }
            );
        }

        [HttpGet("get-damages-for-building/{id}")]
        public IActionResult GetDamages(int id)
        {
            var damages = _repo.GetDamagesForHouseManager(id, DamageStatus.WaitingForFix);
            return Ok(damages.Select(p => _mapper.Map<GetDamageForHouseManagerDTO>(p)
            ));
        }

        [HttpGet("get-fixed-damages-for-building/{id}")]
        public IActionResult GetFixedDamages(int id)
        {
            var damages = _repo.GetDamagesForHouseManager(id, DamageStatus.Fixed);
            return Ok(damages.Select(p => _mapper.Map<GetDamageForHouseManagerDTO>(p)
            ));
        }

        [HttpPost("fix-damage")]
        public async Task<IActionResult> FixDamage([FromBody]int id)
        {
            var user = await _userRepository.GetUser(id);
            var damage = await _repo.ChangeStatus(id, DamageStatus.Fixed);
            var messageSubject = "Zarządca budynku zmienił status zgłoszenia na \'Naprawiony\'.";
            var messageContent = $"Zgłoszenie zostało zminione przez użytkownika: {user.FirstName} {user.LastName}.\n" +
                                 $"Status zgłoszenia zmieniono na \'Naprawiony\'. W razie zastrzeżeń skontaktuj się z zarządcą budynku. \n\n" +
                                 $"Treść maila wygenerowano automatycznie. Nie odpowiadaj na tego maila.";

            _mailService.SendMail(messageSubject, messageContent, "");
            return Ok(_mapper.Map<GetDamageForHouseManagerDTO>(damage)
            );
        }

        [HttpPost("revert-fix")]
        public async Task<IActionResult> RevertDamage([FromBody]int id)
        {
            var damage = await _repo.ChangeStatus(id, DamageStatus.WaitingForFix);
            var user = await _userRepository.GetUser(damage.Building.HouseManager.Id);

            var messageSubject = "Zarządca budynku zmienił status zgłoszenia na \'Wymagający naprawy\'.";
            var messageContent = $"Zgłoszenie zostało zminione przez użytkownika: {user.FirstName} {user.LastName}.\n" +
                                 $"Status zgłoszenia zmieniono na \'Wymagający naprawy\'. W razie zastrzeżeń skontaktuj się z zarządcą budynku. \n\n" +
                                 $"Treść maila wygenerowano automatycznie. Nie odpowiadaj na tego maila.";

            _mailService.SendMail(messageSubject, messageContent, "");

            return Ok(_mapper.Map<GetDamageForHouseManagerDTO>(damage)
            );
        }
    }
}
