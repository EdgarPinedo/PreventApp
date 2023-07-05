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

        public async Task<IActionResult> Accidentes(string filtro, int? numpag)
        {
            ViewData["filtro"] = filtro;
            return View(await _accidentsService.GetAccidents(filtro, numpag));
        }

        public async Task<IActionResult> AccidentDetails(int? id)
        {
            if (id is null)
            {
                return RedirectToAction("Accidentes");
            }

            var accidente = await _accidentsService.Details(id);
            
            if (accidente == null)
            {
                return RedirectToAction("Accidentes");
            }

            return View(accidente);
        }

        /*public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Id");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fecha,Descripcion,UsuarioId,CategoriaId")] Accidente accidente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(accidente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Id", accidente.CategoriaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", accidente.UsuarioId);
            return View(accidente);
        }*/

        public async Task<IActionResult> EditAccident(int? id)
        {
            if (id is null)
            {
                return RedirectToAction("Accidentes");
            }

            var accidente = await _accidentsService.GetSingleAccident(id);

            if (accidente == null)
            {
                return RedirectToAction("Accidentes");
            }

            return View(accidente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAccident(AccidenteDTO accidente)
        {
            if (ModelState.IsValid)
            {
                await _accidentsService.EditAccident(accidente);
                return RedirectToAction(nameof(Accidentes));
            }
            
            return View(accidente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccident(int? id)
        {
            await _accidentsService.DeleteAccident(id);
            return RedirectToAction(nameof(Accidentes));
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
            if (ModelState.IsValid)
            {
                await _usersService.EditUser(usuario);
                return RedirectToAction(nameof(Users), new { filtro = "todos" });
            }

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int? id)
        {
            await _usersService.DeleteUser(id);
            return RedirectToAction(nameof(Users), new { filtro = "todos" });
        }
    }
}
