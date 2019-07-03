using System;
using ClientNotifications;
using ClientNotifications.Helpers;
using mvcdemo.Data;
using mvcdemo.Models;
using mvcdemo.Repository;
using Microsoft.AspNetCore.Mvc;

namespace mvcdemo.Controllers
{
    public class PetController : Controller
    {
        
        private readonly IPetRepository _petRepository;
        private readonly IClientNotification _clientNotification;

        public PetController( IClientNotification clientNotification,
            IPetRepository petRepository 
            )
        {
            
            _petRepository = petRepository;
            _clientNotification = clientNotification;
        }

        // GET 
        public IActionResult Index(string search = null)
        {
            if (!string.IsNullOrEmpty(search))
            {
                var foundPets = _petRepository.SearchPets(search);
                return View(foundPets);
            }
            
            var pets = _petRepository.GetAllPets();
            return View(pets);
        }

        public IActionResult Details(int id)
        {
            var pet = _petRepository.GetSinglePet(id);
            return View(pet);
        }

        [HttpGet]
        public IActionResult New()
        {
            ViewBag.IsEditMode = "false";
            var pet = new Pet();
            return View(pet);
        }

        [HttpPost]
        public IActionResult New(Pet pet, string  IsEditMode)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.IsEditMode = IsEditMode;
                return View(pet);
            }
            try
            {
                if (IsEditMode.Equals("false"))
                {
                    _petRepository.Create(pet);

                    _clientNotification.AddSweetNotification("Success", "Your pet is created.",
                        NotificationHelper.NotificationType.success);
                }

                else
                {
                    _petRepository.Edit(pet);
                    _clientNotification.AddSweetNotification("Success", "Your pet has been edited.",
                        NotificationHelper.NotificationType.success);

                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _clientNotification.AddSweetNotification("Error", "There was a problem.",
                    NotificationHelper.NotificationType.error);
                return RedirectToAction(nameof(Index));
            }

        }

        public IActionResult Delete(int id)
        {
            try
            {
                var pet = _petRepository.GetSinglePet(id);
                _petRepository.Delete(pet);

                return Ok();

            }
            catch (Exception e)
            {
               
                return BadRequest();
            }
            
           
        }

        public IActionResult Edit(int id)
        {
            ViewBag.IsEditMode = "true";
            var pet = _petRepository.GetSinglePet(id);

            return View("New", pet);
        }

        public IActionResult VerifyName(string name)
        {

            if (_petRepository.VerifyName(name))
            {
                return Json($"The pet with name {name} already exists in database. Please try another name." );
            }

            return Json(true);
        }

        public IActionResult AutocompleteResult(string search)
        {
            return Json(_petRepository.SearchPets(search));
        }
    }
}