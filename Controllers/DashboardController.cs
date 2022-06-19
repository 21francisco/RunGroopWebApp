using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashBoardRepository _dashBoardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoService _photoService;

        public DashboardController(IDashBoardRepository dashBoardRepository,  IHttpContextAccessor httpContextAccessor , IPhotoService photoService)
        {
           
            _dashBoardRepository = dashBoardRepository;
            _httpContextAccessor = httpContextAccessor;
            _photoService = photoService;
        }
     
        public async Task<IActionResult> Index()
        {

            var userRaces = await _dashBoardRepository.GetAllUserRaces();
            var userClubs = await _dashBoardRepository.GetAllUserClubs();
            var dashBoardViewModel = new DashBoardViewModel()

            {

                Races = userRaces,
                Clubs = userClubs,

            };
            return View(dashBoardViewModel);
        }

        public async Task<IActionResult> EditUserProfile()      
        
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _dashBoardRepository.GetUserById(curUserId);
            if (user == null) return View("Error");
            var editUserViewModel = new EditUserDashboardViewModel()

            {
                Id = curUserId,
                Pace = user.Pace,
                Mileage = user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl,
                City = user.City,
                State = user.State,
            };
            return View(editUserViewModel);            
        
        }

        private void MapUserEdit(AppUser user, EditUserDashboardViewModel editvM, ImageUploadResult photoResult)
        {
            user.Id = editvM.Id;
            user.Pace = editvM.Pace;
            user.Mileage = editvM.Mileage;
            user.ProfileImageUrl = photoResult.Url.ToString();
            user.City = editvM.City;
            user.State = editvM.State;

        }

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editVM) 
        
        {
            if (!ModelState.IsValid) 
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditUserProfile", editVM);           
            
            }

            AppUser user = await _dashBoardRepository.GetByIdNoTracking(editVM.Id);
            if(user.ProfileImageUrl == "" || user.ProfileImageUrl == null)
            
            {
                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);

                MapUserEdit(user, editVM, photoResult);

                _dashBoardRepository.Update(user);
                return RedirectToAction("Index");
            } 
            else
            {
                try
                {
                    await _photoService.DeletePhotoAsync(user.ProfileImageUrl);


                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("", "Could not delete Photo");
                    return View(editVM);               
                
                }

                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);

                MapUserEdit(user, editVM, photoResult);

                _dashBoardRepository.Update(user);
                return RedirectToAction("Index");



            }

        
        }
    }
}