using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PreventApp.DTOs;
using PreventApp.Services;
using System.Security.Claims;

namespace PreventApp.Controllers
{
    public class AuthController : Controller
    {

        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Login()
        {
            ClaimsPrincipal claims = HttpContext.User;

            if(claims.Identity is not null && claims.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UsuarioDTO u)
        {
            var usuario = await _authService.FindUserAsync(u);
   
            if (usuario is null)
            {
                ViewData["ValidarLogin"] = "El correo o contraseña son incorrectos";
                return View();
            }

            var (identity, properties) = _authService.ConfigClaims(usuario.Email, usuario.Id, usuario.RolId);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Registro()
        {
            ClaimsPrincipal claims = HttpContext.User;

            if (claims.Identity is not null && claims.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(UsuarioDTO u)
        {
            if(!ModelState.IsValid)
            {
                return View(u);
            }

            bool userExist = await _authService.UserExistAsync(u);
            if (userExist)
            {
                ViewData["ValidarRegistro"] = "Ya existe una cuenta con este email";
                return View();
            }

            await _authService.AddUserAsync(u);
            var user = await _authService.FindUserAsync(u);
            if(user is not null)
            {
                var (identity, properties) = _authService.ConfigClaims(u.Email, user.Id, 1);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
