using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PreventApp.DTOs;
using PreventApp.Services;

namespace PreventApp.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class DashboardController : Controller
    {
        private readonly AccidentsService _accidentsService;
        private readonly UsersService _usersService;


        public DashboardController(AccidentsService accidentsService, UsersService userService)
        {
            _accidentsService = accidentsService;
            _usersService = userService;
        }

        public IActionResult CreateAccident()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccident(AccidenteDTO accident)
        {
            if (accident.Estado == true)
            {
                if (accident.Latitud is null || accident.Longitud is null)
                {
                    ViewData["Invalido"] = "Imposible publicar sin una localización";
                    return View();
                }
            }
            TempData["Success"] = "Creado correctamente!";
            await _accidentsService.CreateAccident(accident);
            return RedirectToAction("Accidentes", new { filtro = "todos" });
        }

        public async Task<IActionResult> Accidentes(string filtro, int? numpag)
        {
            ViewData["filtro"] = filtro;
            return View(await _accidentsService.GetAccidents(filtro, numpag));
        }

        public async Task<IActionResult> AccidentDetails(int? id)
        {
            if (id is null)
            {
                return RedirectToAction("Accidentes", new { filtro = "todos" });
            }

            var accidente = await _accidentsService.Details(id);
            
            if (accidente == null)
            {
                return RedirectToAction("Accidentes", new { filtro = "todos" });
            }

            return View(accidente);
        }

        public async Task<IActionResult> EditAccident(int? id)
        {
            if (id is null)
            {
                return RedirectToAction("Accidentes", new { filtro = "todos" });
            }

            var accidente = await _accidentsService.GetSingleAccident(id);

            if (accidente == null)
            {
                return RedirectToAction("Accidentes", new { filtro = "todos" });
            }

            return View(accidente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAccident(AccidenteDTO accidente)
        {
            if (accidente.Estado == true)
            {
                if (accidente.Latitud is null || accidente.Longitud is null)
                {
                    ViewData["Invalido"] = "Imposible publicar sin una localización";
                    return View();
                }
            }

            await _accidentsService.EditAccident(accidente);
            TempData["Success"] = "Modificado correctamente!";
            return RedirectToAction(nameof(Accidentes),  new { filtro = "todos" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccident(int? id)
        {
            await _accidentsService.DeleteAccident(id);
            TempData["Success"] = "Eliminado correctamente!";
            return RedirectToAction(nameof(Accidentes), new { filtro = "todos" });
        }




        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDTO user)
        {
            if(!ModelState.IsValid)
            {
                return View(user);
            }
            
            if(await _usersService.IsEmailRegistered(user.Email, null))
            {
                ViewData["EmailError"] = "Ya existe una cuenta con este email";
                return View(user);
            }

            await _usersService.CreateUser(user);
            TempData["Success"] = "Creado correctamente!";
            return RedirectToAction(nameof(Users), new { filtro = "todos" });
        }

        public async Task<IActionResult> Users(string filtro, int? numpag)
        {
            ViewData["filtro"] = filtro;
            return View(await _usersService.GetUsers(filtro, numpag));
        }

        public async Task<IActionResult> UserDetails(int? id)
        {
            if (id is null)
            {
                return RedirectToAction("Users", new { filtro = "todos" });
            }

            var user = await _usersService.UserDetails(id);

            if (user == null)
            {
                return RedirectToAction("Users", new { filtro = "todos" });
            }

            return View(user);
        }

        public async Task<IActionResult> EditUser(int? id)
        {
            if (id is null)
            {
                return RedirectToAction("Users", new { filtro = "todos" });
            }

            var user = await _usersService.GetSingleUser(id);

            if (user == null)
            {
                return RedirectToAction("Users", new { filtro = "todos" });
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(UserDashboardDTO usuario)
        {
            if(!ModelState.IsValid)
            {
                return View(usuario);
            }

            if (await _usersService.IsEmailRegistered(usuario.Email, usuario.Id))
            {
                ViewData["EmailError"] = "Ya existe una cuenta con este email";
                return View(usuario);
            }

            await _usersService.EditUser(usuario);
            TempData["Success"] = "Modificado correctamente!";
            return RedirectToAction(nameof(Users), new { filtro = "todos" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int? id)
        {
            await _usersService.DeleteUser(id);
            TempData["Success"] = "Eliminado correctamente!";
            return RedirectToAction(nameof(Users), new { filtro = "todos" });
        }

        public IActionResult ChangePassword(int id)
        {
            ChangePasswordDTO newPass = new()
            {
                UserId = id,
            };
            return View(newPass);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO newPassword)
        {
            if (!ModelState.IsValid)
            {
                return View(newPassword);
            }

            await _usersService.ChangePassword(newPassword.UserId, newPassword.ContraseñaNueva);
            TempData["Success"] = "Modificado correctamente!";
            return RedirectToAction("EditUser", new { id = newPassword.UserId });
        }
    }
}