using AutoMapper;
using ITLANetworking.Core.Application.Dtos.Account;
using ITLANetworking.Core.Application.Dtos.Email;
using ITLANetworking.Core.Application.Helpers;
using ITLANetworking.Core.Application.Interfaces.Services.Shared;
using ITLANetworking.Core.Application.Interfaces.Services;
using ITLANetworking.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Collections.Generic;

namespace ITLANetworking.Core.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserSyncService _userSyncService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserSyncService userSyncService,
            IEmailService emailService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _mapper = mapper;
            _userSyncService = userSyncService;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            var response = new AuthenticationResponse();

            var user = await _userManager.FindByNameAsync(request.UserName)
                       ?? await _userManager.FindByEmailAsync(request.Email);

            if (user == null || !user.IsActive)
            {
                response.HasError = true;
                response.Error = user == null
                    ? "Los datos de acceso son incorrectos"
                    : "Su cuenta está inactiva. Es necesario activar la cuenta mediante el enlace enviado al correo electrónico registrado.";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName!, request.Password, false, false);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = "Los datos de acceso son incorrectos";
                return response;
            }

            var roles = await _userManager.GetRolesAsync(user);

            response.Id = user.Id;
            response.Email = user.Email!;
            response.UserName = user.UserName!;
            response.IsVerified = user.EmailConfirmed;
            response.FirstName = user.FirstName;
            response.LastName = user.LastName;
            response.Phone = user.Phone;
            response.ProfilePicture = user.ProfilePicture;
            response.IsActive = user.IsActive;
            response.CreatedDate = user.CreatedDate;
            response.Roles = roles.ToList();

            return response;
        }

        public async Task<RegisterResponse> RegisterBasicUserAsync(RegisterRequest request, string origin)
        {
            var response = new RegisterResponse();

            if (await _userManager.FindByNameAsync(request.UserName) != null)
            {
                response.HasError = true;
                response.Error = "El nombre de usuario ya existe";
                return response;
            }

            if (await _userManager.FindByEmailAsync(request.Email) != null)
            {
                response.HasError = true;
                response.Error = "El email ya está registrado";
                return response;
            }

            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.UserName,
                Phone = request.Phone,
                ProfilePicture = request.ProfilePicture,
                IsActive = false,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                var createdUser = await _userManager.FindByEmailAsync(request.Email);
                if (createdUser != null)
                {
                    await _userSyncService.CreateDomainUserAsync(
                        createdUser.Id,
                        createdUser.FirstName,
                        createdUser.LastName,
                        createdUser.Phone,
                        createdUser.ProfilePicture
                    );

                    response.UserId = createdUser.Id;
                }
                else
                {
                    response.UserId = user.Id;
                }

                var verificationUri = await SendVerificationEmailUri(createdUser ?? user, origin);

                await _emailService.SendAsync(new EmailRequest()
                {
                    To = (createdUser ?? user).Email,
                    From = "borrome941@gmail.com",
                    Subject = "🎉 ¡Bienvenido a ITLA Networking! - Confirma tu cuenta",
                    Body = EmailTemplates.GetWelcomeEmail((createdUser ?? user).FirstName, verificationUri)
                });

                response.UserId = user.Id;
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Error: {error.Code} - {error.Description}");
                }
                var algo = result.Errors.Select(e => e.Description).ToList();
                response.HasError = true;
                response.Error = algo.FirstOrDefault();

            }

            return response;
        }

        public async Task<string> ConfirmAccountAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return "No se encontró una cuenta con este token";

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                user.IsActive = true;
                await _userManager.UpdateAsync(user);

                await _userSyncService.ActivateUserAsync(user.Id);

                await _emailService.SendAsync(new EmailRequest()
                {
                    To = user.Email!,
                    From = "borrome941@gmail.com",
                    Subject = "✅ ¡Cuenta activada exitosamente! - ITLA Networking",
                    Body = EmailTemplates.GetAccountActivatedEmail(user.FirstName)
                });

                return "Success";
            }

            return "Ocurrió un error confirmando tu cuenta";
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            var response = new ForgotPasswordResponse();

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.HasError = true;
                response.Error = "No se encontró una cuenta con este nombre de usuario";
                return response;
            }

            user.IsActive = false;
            await _userManager.UpdateAsync(user);

            // Desactivar también el usuario de dominio
            await _userSyncService.DeactivateUserAsync(user.Id);

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            resetToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetToken));

            var resetUri = $"{origin}/Account/ResetPassword?token={resetToken}&email={user.Email}";

            // ✨ Usando el template estático
            await _emailService.SendAsync(new EmailRequest()
            {
                To = user.Email!,
                Subject = "🔐 Recuperación de contraseña - ITLA Networking",
                Body = EmailTemplates.GetPasswordResetEmail(user.FirstName, resetUri)
            });

            response.HasError = false;
            return response;
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var response = new ResetPasswordResponse();

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.HasError = true;
                response.Error = "No se encontró una cuenta con este email";
                return response;
            }

            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            var result = await _userManager.ResetPasswordAsync(user, token, request.Password);

            if (result.Succeeded)
            {
                user.IsActive = true;
                await _userManager.UpdateAsync(user);

                // Activar también el usuario de dominio
                await _userSyncService.ActivateUserAsync(user.Id);

                // ✨ Usando el template estático
                await _emailService.SendAsync(new EmailRequest()
                {
                    To = user.Email!,
                    From = "borrome941@gmail.com",
                    Subject = "🔒 Contraseña restablecida exitosamente - ITLA Networking",
                    Body = EmailTemplates.GetPasswordChangedEmail(user.FirstName)
                });

                response.HasError = false;
            }
            else
            {
                response.HasError = true;
                response.Error = "Ocurrió un error restableciendo la contraseña";
            }

            return response;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        private async Task<string> SendVerificationEmailUri(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "Account/ConfirmEmail";
            var uri = new Uri($"{origin}/{route}");
            var verificationUri = QueryHelpers.AddQueryString(uri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "token", code);

            return verificationUri;
        }
    }
}
