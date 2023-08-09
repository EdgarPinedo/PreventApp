using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PreventApp.DTOs;
using PreventApp.Models;
using PreventApp.Services;
using System.Diagnostics;

namespace PreventApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AccidentsService _accidentsService;

        public HomeController(AccidentsService accidentsService, UsersService userService)
        {
            _accidentsService = accidentsService;
        }

        public async Task<IActionResult> Index()
        {
            var accidents = await _accidentsService.GetRecentAccidents();
            return View(accidents);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Index(AlertDTO alert)
        {
            if(ModelState.IsValid)
            {
                await _accidentsService.AddAlert(alert);
                TempData["Success"] = "Se ha enviado su alerta!";
            }
            else
            {
                TempData["Failed"] = "Olvidaste marcar en el mapa la ubicación del accidente!";
            }

            return RedirectToAction("Index");
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