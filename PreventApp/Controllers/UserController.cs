using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PreventApp.DTOs;
using PreventApp.Services;
using System.Security.Claims;

namespace PreventApp.Controllers
{
    public class UserController : Controller
    {
        private readonly UsersService _userService;
        private readonly AccidentsService _accidentsService;

        public UserController(UsersService userService, AccidentsService accidentsService)
        {
            _userService = userService;
            _accidentsService = accidentsService;
        }

        [Authorize]
        public async Task<IActionResult> Alerts()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (claim is not null)
            {
                var id = Convert.ToInt32(claim.Value);
                var alerts = await _accidentsService.GetAccidentsByUser(id);
                return View(alerts);
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (claim is not null)
            {
                var id = Convert.ToInt32(claim.Value);
                var user = await _userService.GetProfile(id);
                return View(user);
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Profile(ProfileDTO profile)
        {
            if (!ModelState.IsValid)
            {
                return View(profile);
            }

            var claim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (claim is not null)
            {
                var id = Convert.ToInt32(claim.Value);
                await _userService.EditProfile(id, profile);
            }

            ViewData["Actualizado"] = "Ha sido actualizado correctamente!";
            return View(profile);
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(NewPasswordDTO newPassword)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            var userId = Convert.ToInt32(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var equalsCurrentPassword = await _userService
                .EqualsCurrentPassword(newPassword.ContraseñaActual, userId);

            if(!equalsCurrentPassword)
            {
                ViewData["Validation"] = "La contraseña actual no es correcta";
                return View();
            }

            await _userService.ChangePassword(userId, newPassword.ContraseñaNueva);

            TempData["Success"] = "Contraseña actualizada!";
            return RedirectToAction("Profile");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _userService.DeleteUser(userId);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
