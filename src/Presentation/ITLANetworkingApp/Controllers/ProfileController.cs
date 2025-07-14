using ITLANetworking.Core.Application.Interfaces.Services;
using ITLANetworking.Core.Application.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AutoMapper;
using ITLANetworking.Core.Domain.Interfaces.Repositories;
using ITLANetworking.Core.Application.Interfaces.Services.Shared;

namespace ITLANetworking.Presentation.WebApp.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUserSyncService _userSyncService;
        private readonly IUserRepository _userRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly IFriendshipService _friendShipService;
        private readonly IMapper _mapper;

        public ProfileController(
            IUserSyncService userSyncService, 
            IUserRepository userRepository,
            IFileUploadService fileUploadService,
            IMapper mapper,
            IFriendshipService friendShipService)
        {
            _userSyncService = userSyncService;
            _userRepository = userRepository;
            _fileUploadService = fileUploadService;
            _mapper = mapper;
            _friendShipService = friendShipService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var user = await _userRepository.GetUserWithAllDetailsAsync(userId);
            var FriendsOFCurrendUser = await _friendShipService.GetFriendsByUserIdAsync(userId);
            
            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new EditProfileViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PostCount = user.Posts.Count(),
                FriendsCount = FriendsOFCurrendUser.Count(),
                JuegosCount = user.WonGames.Count(),
                Phone = user.Phone ?? string.Empty,
                ProfilePicture = user.ProfilePicture ?? string.Empty,
            }; 

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(EditProfileViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", vm);
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                string? profilePicturePath = vm.ProfilePicture;

                if (vm.ProfilePictureFile != null)
                {
                    if (!string.IsNullOrEmpty(vm.ProfilePicture))
                    {
                        _fileUploadService.DeleteFile(vm.ProfilePicture);
                    }
                    
                    profilePicturePath = await _fileUploadService.UploadAsync(vm.ProfilePictureFile, "profiles");
                }

                await _userSyncService.UpdateUserAsync(new Core.Application.Dtos.User.UserDto
                {
                    Id = userId,
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    Phone = vm.Phone,
                    ProfilePicture = profilePicturePath
                });

                TempData["SuccessMessage"] = "Perfil actualizado exitosamente";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                vm.HasError = true;
                vm.Error = "Ocurrió un error al actualizar el perfil";
                return View("Index", vm);
            }
        }
    }
}
