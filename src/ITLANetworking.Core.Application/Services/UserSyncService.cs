using ITLANetworking.Core.Application.Interfaces.Services;
using ITLANetworking.Core.Domain.Entities;
using ITLANetworking.Core.Domain.Interfaces.Repositories;
using ITLANetworking.Core.Application.Dtos.User;
using ITLANetworking.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;

namespace ITLANetworking.Core.Application.Services
{
    public class UserSyncService : IUserSyncService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserSyncService(
            IUserRepository userRepository,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task DeactivateUserAsync(string userId)
        {
            // Desactivar usuario de dominio
            var domainUser = await _userRepository.GetByIdAsync(userId);
            if (domainUser != null)
            {
                domainUser.IsActive = false;
                await _userRepository.UpdateAsync(domainUser, userId);
            }

            // Desactivar usuario de Identity
            var identityUser = await _userManager.FindByIdAsync(userId);
            if (identityUser != null)
            {
                identityUser.IsActive = false;
                await _userManager.UpdateAsync(identityUser);
            }
        }

        public async Task<UserDto?> GetUserByIdAsync(string userId)
        {
            var domainUser = await _userRepository.GetByIdAsync(userId);
            return domainUser == null ? null : _mapper.Map<UserDto>(domainUser);
        }

        public async Task<List<UserDto>> SearchUsersAsync(string searchTerm)
        {
            var users = await _userRepository.SearchUsersByUsernameAsync(searchTerm);
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task SyncUserAsync(UserDto userDto)
        {
            var domainUser = await _userRepository.GetByIdAsync(userDto.Id);
            if (domainUser == null)
            {
                domainUser = _mapper.Map<User>(userDto);
                await _userRepository.AddAsync(domainUser);
            }
            else
            {
                _mapper.Map(userDto, domainUser);
                await _userRepository.UpdateAsync(domainUser, userDto.Id);
            }
        }

        public async Task CreateDomainUserAsync(string userId, string firstName, string lastName, string? phone = null, string? profilePicture = null)
        {
            var existingUser = await _userRepository.GetByIdAsync(userId);
            if (existingUser == null)
            {
                var domainUser = new User
                {
                    Id = userId,
                    FirstName = firstName,
                    LastName = lastName,
                    Phone = phone,
                    ProfilePicture = profilePicture,
                    IsActive = false
                };
                await _userRepository.AddAsync(domainUser);
            }
        }

        public async Task ActivateUserAsync(string userId)
        {
            // Activar usuario de dominio
            var domainUser = await _userRepository.GetByIdAsync(userId);
            if (domainUser != null)
            {
                domainUser.IsActive = true;
                await _userRepository.UpdateAsync(domainUser, userId);
            }
        }

        public async Task UpdateUserAsync(UserDto userDto)
        {
            // Actualizar usuario de dominio
            var domainUser = await _userRepository.GetByIdAsync(userDto.Id);
            if (domainUser != null)
            {
                _mapper.Map(userDto, domainUser);
                await _userRepository.UpdateAsync(domainUser, userDto.Id);
            }

            // Actualizar usuario de Identity
            var identityUser = await _userManager.FindByIdAsync(userDto.Id);
            if (identityUser != null)
            {
                identityUser.FirstName = userDto.FirstName;
                identityUser.LastName = userDto.LastName;
                identityUser.Phone = userDto.Phone;
                identityUser.ProfilePicture = userDto.ProfilePicture;
                identityUser.IsActive = userDto.IsActive;
                await _userManager.UpdateAsync(identityUser);
            }
        }

        public async Task UpdateUserProfileAsync(string userId, string firstName, string lastName, string? phone, string? profilePicture)
        {
            // Actualizar usuario de dominio
            var domainUser = await _userRepository.GetByIdAsync(userId);
            if (domainUser != null)
            {
                domainUser.FirstName = firstName;
                domainUser.LastName = lastName;
                domainUser.Phone = phone;
                domainUser.ProfilePicture = profilePicture;
                await _userRepository.UpdateAsync(domainUser, userId);
            }

            // Actualizar usuario de Identity
            var identityUser = await _userManager.FindByIdAsync(userId);
            if (identityUser != null)
            {
                identityUser.FirstName = firstName;
                identityUser.LastName = lastName;
                identityUser.Phone = phone;
                identityUser.ProfilePicture = profilePicture;
                await _userManager.UpdateAsync(identityUser);
            }
        }

        /// <summary>
        /// Método adicional para sincronizar completamente ambas entidades
        /// </summary>
        public async Task SyncBothUserEntitiesAsync(string userId)
        {
            var domainUser = await _userRepository.GetByIdAsync(userId);
            var identityUser = await _userManager.FindByIdAsync(userId);

            if (domainUser != null && identityUser != null)
            {
                // Sincronizar datos del usuario de Identity al de dominio
                domainUser.FirstName = identityUser.FirstName;
                domainUser.LastName = identityUser.LastName;
                domainUser.Phone = identityUser.Phone;
                domainUser.ProfilePicture = identityUser.ProfilePicture;
                domainUser.IsActive = identityUser.IsActive;

                await _userRepository.UpdateAsync(domainUser, userId);
            }
        }
    }
}