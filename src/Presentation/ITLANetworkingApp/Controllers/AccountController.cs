using AutoMapper;
using ITLANetworking.Core.Application.Dtos.Account;
using ITLANetworking.Core.Application.Features.Users.Commands.RegisterUser;
using ITLANetworking.Core.Application.Features.Users.Queries.AuthenticateUser;
using ITLANetworking.Core.Application.Interfaces.Services;
using ITLANetworking.Core.Application.Interfaces.Services.Shared;
using ITLANetworking.Core.Application.ViewModels.User;
using ITLANetworking.Infrastructure.Identity.Entities;
using ITLANetworking.Infrastructure.Shared.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITLANetworking.Presentation.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IFileUploadService _fileUploadService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(
            IAccountService accountService,
            IFileUploadService fileUploadService,
            IMapper mapper,
            IMediator mediator,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _accountService = accountService;
            _mapper = mapper;
            _mediator = mediator;
            _fileUploadService = fileUploadService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            try
            {
                ApplicationUser user = null;

                if (!string.IsNullOrEmpty(vm.Email))
                {
                    user = await _userManager.FindByEmailAsync(vm.Email);
                }
                else if (!string.IsNullOrEmpty(vm.UserName))
                {
                    user = await _userManager.FindByNameAsync(vm.UserName);
                }

                if (user == null)
                {
                    vm.HasError = true;
                    vm.Error = "Usuario o contraseña incorrectos";
                    return View(vm);
                }

                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    vm.HasError = true;
                    vm.Error = "Debes confirmar tu email antes de iniciar sesión";
                    return View(vm);
                }

                var result = await _signInManager.PasswordSignInAsync(
                    user,
                    vm.Password,
                    isPersistent: true,
                    lockoutOnFailure: false
                );

                if (result.Succeeded)
                {
                    await AddCustomClaimsAsync(user);

                    return RedirectToAction("Index", "Home");
                }
                else if (result.IsLockedOut)
                {
                    vm.HasError = true;
                    vm.Error = "Cuenta bloqueada temporalmente";
                    return View(vm);
                }
                else if (result.RequiresTwoFactor)
                {
                    return RedirectToAction("LoginWith2fa", new { returnUrl = Url.Action("Index", "Home") });
                }
                else
                {
                    vm.HasError = true;
                    vm.Error = "Usuario o contraseña incorrectos";
                    return View(vm);
                }
            }
            catch
            {
                TempData["ErrorMessage"] = "Ocurrió un error inesperado al iniciar sesión.";
                return RedirectToAction("Login");
            }
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new SaveUserViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            try
            {
                var origin = $"{Request.Scheme}://{Request.Host}";

                var command = new RegisterUserCommand
                {
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    UserName = vm.UserName,
                    ProfilePicture = vm.Image != null
                        ? await _fileUploadService.UploadAsync(vm.Image, $"profile/{vm.UserName}")
                        : null,
                    Email = vm.Email,
                    Phone = vm.Phone ?? "",
                    Password = vm.Password,
                    ConfirmPassword = vm.ConfirmPassword,
                    Origin = origin
                };

                var result = await _mediator.Send(command);

                if (!result.Success)
                {
                    vm.HasError = true;
                    vm.Error = result.Message;
                    TempData["ErrorMessage"] = result.Message;
                    return View(vm);
                }

                TempData["SuccessMessage"] = "Registro exitoso. Por favor revisa tu email para confirmar tu cuenta.";
                return RedirectToAction("Login");
            }
            catch
            {
                TempData["ErrorMessage"] = "Ocurrió un error inesperado al registrarte.";
                return RedirectToAction("Register");
            }
        }

        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            try
            {
                var origin = $"{Request.Scheme}://{Request.Host}";

                var request = new ForgotPasswordRequest
                {
                    Email = vm.Email,
                    Origin = origin
                };

                await _accountService.ForgotPasswordAsync(request, origin);

                TempData["SuccessMessage"] = "Si el email existe, recibirás instrucciones para resetear tu contraseña.";
                return RedirectToAction("Login");
            }
            catch
            {
                TempData["ErrorMessage"] = "Ocurrió un error inesperado al solicitar el restablecimiento.";
                return RedirectToAction("ForgotPassword");
            }
        }

        [AllowAnonymous]
        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token requerido");
            }

            return View(new ResetPasswordViewModel { Email = email, Token = token });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            try
            {
                var request = new ResetPasswordRequest
                {
                    Email = vm.Email,
                    Token = vm.Token,
                    Password = vm.Password,
                    ConfirmPassword = vm.ConfirmPassword
                };

                var result = await _accountService.ResetPasswordAsync(request);

                if (result.HasError)
                {
                    vm.HasError = true;
                    vm.Error = result.Error;
                    return View(vm);
                }

                TempData["SuccessMessage"] = "Contraseña restablecida exitosamente. Ya puedes iniciar sesión.";
                return RedirectToAction("Login");
            }
            catch
            {
                TempData["ErrorMessage"] = "Ocurrió un error inesperado al restablecer tu contraseña.";
                return RedirectToAction("ResetPassword", new { vm.Email, vm.Token });
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                TempData["ErrorMessage"] = "El enlace es inválido o está incompleto.";
                return View("ConfirmEmail");
            }

            try
            {
                var result = await _accountService.ConfirmAccountAsync(userId, token);

                if (result == "Success")
                {
                    TempData["SuccessMessage"] = "Tu cuenta ha sido confirmada exitosamente. Ya puedes iniciar sesión.";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo confirmar la cuenta. El enlace puede haber expirado o ser inválido.";
                }
            }
            catch
            {
                TempData["ErrorMessage"] = "Ocurrió un error inesperado al confirmar tu cuenta.";
            }

            return View("ConfirmEmail");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private async Task AddCustomClaimsAsync(ApplicationUser user)
        {
            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(user.FirstName))
                claims.Add(new Claim("FirstName", user.FirstName));

            if (!string.IsNullOrEmpty(user.LastName))
                claims.Add(new Claim("LastName", user.LastName));

            if (!string.IsNullOrEmpty(user.PhoneNumber))
                claims.Add(new Claim("Phone", user.PhoneNumber));

            if (!string.IsNullOrEmpty(user.ProfilePicture))
                claims.Add(new Claim("ProfilePicture", user.ProfilePicture));

            if (claims.Any())
            {
                await _userManager.AddClaimsAsync(user, claims);
            }
        }
    }
}
