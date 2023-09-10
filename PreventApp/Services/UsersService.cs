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

        public async Task CreateUser(CreateUserDTO user)
        {
            Usuario usuario = new()
            {
                Nombre = user.Nombre,
                Email = user.Email,
                Telefono = user.Telefono,
                Contraseña = GetSHA256(user.Contraseña),
                RolId = user.RolId
            };

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsEmailRegistered(string email, int? id)
        {
            if(id != null )
            {
                return await _context.Usuarios.AnyAsync(u => u.Email == email && u.Id != id);
            }

            return await _context.Usuarios.AnyAsync(u => u.Email == email);
        }

        public async Task<Paginacion<Usuario>> GetUsers(string filtro, int? numpag)
        {
            int cantidadRegistros = 20;

            IQueryable<Usuario> usuarios = _context.Usuarios;

            if (filtro == "usuarios") usuarios = usuarios.Where(a => a.RolId == 1);

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
            _context.Usuarios.Where(u => u.Id == id)
                            .ExecuteUpdate(b => b
                            .SetProperty(u => u.IsDeleted, true));
            await _context.SaveChangesAsync();
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
                    RolId = usuario.RolId
                };
                return user;
            }
            return null;
        }

        public async Task EditUser(UserDashboardDTO usuario)
        {
            _context.Usuarios.Where(u => u.Id == usuario.Id)
                             .ExecuteUpdate(b => b
                                .SetProperty(u => u.Nombre, usuario.Nombre)
                                .SetProperty(u => u.Email, usuario.Email)
                                .SetProperty(u => u.Telefono, usuario.Telefono)
                                .SetProperty(u => u.RolId, usuario.RolId));
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

        public async Task<bool> EqualsCurrentPassword(string pass, int id)
        {
            var currentPass = await _context.Usuarios
                                    .Select(u => new { u.Contraseña, u.Id})
                                    .Where(u => u.Id == id)
                                    .FirstOrDefaultAsync();

            if(currentPass != null && currentPass.Contraseña == GetSHA256(pass))
            {
                return true;
            }

            return false;
        }

        public async Task ChangePassword(int userId, string password)
        {
            password = GetSHA256(password);

            _context.Usuarios.Where(u => u.Id == userId)
                             .ExecuteUpdate(b => b
                                .SetProperty(u => u.Contraseña, password));
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
