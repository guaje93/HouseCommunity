using HouseCommunity.Data;
using HouseCommunity.Data.Interfaces;
using HouseCommunity.Model;
using HouseCommunity.Request;
using HouseCommunity.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IMediaRepository _repo;
        private readonly IMailService _mailService;
        private readonly IUserRepository _userRepository;

        public MediaController(IMediaRepository mediaRepository, IMailService mailService, IUserRepository userRepository)
        {
            _repo = mediaRepository;
            _mailService = mailService;
            _userRepository = userRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> LoadMediaForUser(int id)
        {

            var media = await _repo.GetAllMediaForUser(id);
            if (media == null)
                return BadRequest();

            return Ok(media);
        }

        [HttpPut("update-media")]
        public async Task<IActionResult> UpdateMedia(MediaUpdatedByUserDTO addMediaToDbRequest)
        {
            var user = await _userRepository.GetUser(addMediaToDbRequest.UserId);
            var media = await _repo.UpdateMedia(addMediaToDbRequest);

            if (media == null)
                return BadRequest();

            var messageSubject = "Użytkownik wysłał formularz zużycia mediów.";
            var messageContent = $"Użytkownik {user.FirstName} {user.LastName} edytował status zgłoszonego zużycia: \n\n" +
                $"Typ: {DecodeMediaType(media.MediaType)} \n" +
                $"Stan: {media.CurrentValue} \n" +
                $"Status: Zgłoszenie wysłane przez użytkownika.\n\n" +
                $"W razie zastrzeżeń skontaktuj się bezpośrednio z użytkownikiem. \n\n" +
                $"Treść maila wygenerowano automatycznie. Nie odpowiadaj na tego maila.";
            _mailService.SendMail(messageSubject, messageContent, "", "Home Community App");

            return Ok(media);
        }

        [HttpPut("book-media")]
        public async Task<IActionResult> BookMedia(MediaUpdatedByAdministrationDTO addMediaToDbRequest)
        {
            var user = await _userRepository.GetUser(addMediaToDbRequest.UserId);
            var media = await _repo.UpdateMedia(addMediaToDbRequest);

            if (media == null)
                return Unauthorized();

            var messageSubject = "Status zgłoszonego zużycia został zmieniony.";
            var messageContent = $"Użytkownik {user.FirstName} {user.LastName} edytował status zgłoszonego zużycia: \n\n" +
                $"Typ: {DecodeMediaType(media.MediaType)} \n" +
                $"Stan: {media.CurrentValue} \n" +
                $"Status: Zgłoszenie zaksięgowane przez administrację.\n\n" +
                $"W razie zastrzeżeń skontaktuj się z pracownikiem administracji. \n\n" +
                $"Treść maila wygenerowano automatycznie. Nie odpowiadaj na tego maila.";

            _mailService.SendMail(messageSubject, messageContent, "", "Home Community App");

            return Ok(media);
        }

        [HttpPut("unlock")]
        public async Task<IActionResult> UnlockMedia(MediaUpdatedByAdministrationDTO addMediaToDbRequest)
        {
            var user = await _userRepository.GetUser(addMediaToDbRequest.UserId);
            var media = await _repo.UnlockMedia(addMediaToDbRequest);

            if (media == null)
                return Unauthorized();

            var messageSubject = "Status zgłoszonego zużycia został zmieniony.";
            var messageContent = $"Użytkownik {user.FirstName} {user.LastName} edytował status zgłoszonego zużycia: \n\n" +
                $"Typ: {DecodeMediaType(media.MediaType)} \n" +
                $"Stan: {media.CurrentValue} \n" +
                $"Status: Zgłoszenie edytowane przez administrację.\n\n" +
                $"W razie zastrzeżeń skontaktuj się z pracownikiem administracji. \n\n" +
                $"Treść maila wygenerowano automatycznie. Nie odpowiadaj na tego maila.";

            _mailService.SendMail(messageSubject, messageContent, "", "Home Community App");

            return Ok(media);
        }

        private string DecodeMediaType(MediaEnum mediaType)
        {
            switch (mediaType)
            {
                case MediaEnum.ColdWater:
                    return "Zużycie wody zimnej";
                case MediaEnum.HotWater:
                    return "Zużycie wody ciepłej";
                case MediaEnum.Heat:
                    return "Zużycie energii na ogrzewanie";
            }
            return "";
        }

        [HttpPost("create-empty-media-entry")]
        public async Task<IActionResult> AddEmptyMediaEntryForUser(AddEmptyMediaRequest addMediaToDbRequest)
        {
            var user = await _userRepository.GetUser(addMediaToDbRequest.AdministratorId);
            var flat = await _repo.CreateEmptyMediaForUser(addMediaToDbRequest);

            if (flat == null)
                return BadRequest();

            var messageSubject = "Wygenerowano nowe formularze do zgłoszenia zużycia mediów";
            var messageContent = $"Użytkownik {user.FirstName} {user.LastName} wygenerował nowe formularze do zgłoszenia zuzycia mediów: \n\n" +
                                 $"-Formularz zużycia wody ciepłej \n" +
                                 $"-Formularz zużycia wody zimnej \n" +
                                 $"-Formularz zużycia energii na ogrzewanie \n\n" +
                                 $"W razie zastrzeżeń skontaktuj się z pracownikiem administracji. \n\n" +
                                 $"Treść maila wygenerowano automatycznie. Nie odpowiadaj na tego maila.";

            _mailService.SendMail(messageSubject, messageContent, "", "Home Community App");

            return Ok(flat);
        }

        [HttpGet("media-for-flat/{id}")]
        public async Task<IActionResult> GetMediaForFlat(int id)
        {
            var media = (await _repo.GetMedia(id)).ToList();

            if (media == null)
                return BadRequest();

            return Ok(media);
        }



    }
}