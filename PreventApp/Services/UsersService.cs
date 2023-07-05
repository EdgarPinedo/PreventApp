using Microsoft.EntityFrameworkCore;
using PreventApp.DTOs;
using PreventApp.Models;
using System.Security.Cryptography;
using System.Text;

namespace PreventApp.Services
{
    public class UsersService
    {
        private readonly PreventAppDbContext _context;

        public UsersService(PreventAppDbContext context)
        {
            _context = context;
        }

        public async Task<Paginacion<Usuario>> GetUsers(string filtro, int? numpag)
        {
            int cantidadRegistros = 20;

            IQueryable<Usuario> usuarios = _context.Usuarios;

            if (filtro == "usuarios") usuarios = usuarios.Where(a => a.RolId == 1);

            if (filtro == "instituciones") usuarios = usuarios.Where(a => a.RolId == 2);

            if (filtro == "administradores") usuarios = usuarios.Where(a => a.RolId == 3);

            usuarios = usuarios.Include(a => a.Rol);
            return await Paginacion<Usuario>.Paginar(usuarios, numpag ?? 1, cantidadRegistros);
        }

        public async Task<Usuario?> UserDetails(int? id)
        {
            return await _context.Usuarios
                .Include(a => a.Rol)
                .Include(a => a.Accidentes)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task DeleteUser(int? id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<UserDashboardDTO?> GetSingleUser(int? id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.Id == id);
            if (usuario != null)
            {
                UserDashboardDTO user = new()
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Email = usuario.Email,
                    Telefono = usuario.Telefono,
                    Contraseña = usuario.Contraseña,
                    RolId = usuario.RolId
                };
                return user;
            }
            return null;
        }

        public async Task EditUser(UserDashboardDTO usuario)
        {
            Usuario user = new()
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Telefono = usuario.Telefono,
                Contraseña = GetSHA256(usuario.Contraseña),
                RolId = usuario.RolId
            };

            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<ProfileDTO?> GetProfile(int? id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.Id == id);
            if (usuario != null)
            {
                ProfileDTO user = new()
                {
                    Nombre = usuario.Nombre,
                    Email = usuario.Email,
                    Telefono = usuario.Telefono,
                };
                return user;
            }
            return null;
        }

        public async Task EditProfile(int id, ProfileDTO profile)
        {
            _context.Usuarios.Where(u => u.Id == id)
                             .ExecuteUpdate(b => b
                                .SetProperty(u => u.Nombre, profile.Nombre)
                                .SetProperty(u => u.Telefono, profile.Telefono));
            await _context.SaveChangesAsync();
        }

        public static string GetSHA256(string pass)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(pass));

            var contraseña = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                contraseña.Append(bytes[i].ToString("x2"));
            }

            return contraseña.ToString();
        }
    }
}
