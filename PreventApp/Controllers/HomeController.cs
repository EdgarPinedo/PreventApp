using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PreventApp.DTOs;
using PreventApp.Models;
using PreventApp.Services;
using System.Diagnostics;
using System.Security.Claims;

namespace PreventApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AccidentsService _accidentsService;
        private readonly UsersService _userService;

        public HomeController(AccidentsService accidentsService, UsersService userService)
        {
            _accidentsService = accidentsService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var accidents = await _accidentsService.GetRecentAccidents();
            return View(accidents);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddAlert()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> UserAlerts()
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

        [HttpPost]
        public async Task<IActionResult> Profile(ProfileDTO profile)
        {
            if(!ModelState.IsValid)
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

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}