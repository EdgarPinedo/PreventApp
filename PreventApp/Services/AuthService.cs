using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PreventApp.DTOs;
using PreventApp.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PreventApp.Services
{
    public class AuthService
    {
        private readonly PreventAppDbContext _context;

        public AuthService(PreventAppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> FindUserAsync(UsuarioDTO user)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(
                u => u.Email == user.Email && u.Contraseña == GetSHA256(user.Contraseña));
        }

        public async Task<bool> UserExistAsync(UsuarioDTO user)
        {
            return await _context.Usuarios.AnyAsync(u => u.Email == user.Email);
        }

        public async Task AddUserAsync(UsuarioDTO user)
        {
            var usuario = new Usuario()
            {
                Email = user.Email,
                Contraseña = GetSHA256(user.Contraseña),
                RolId = 1
            };

            await _context.Usuarios.AddAsync(usuario);
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

        public (ClaimsPrincipal, AuthenticationProperties) ConfigClaims(string email, int id, int rol)
        {
            var roles = new Dictionary<int, string>()
            {
                { 1, "Usuario" },
                { 2, "Institucion" },
                { 3, "Administrador" },
            };

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.NameIdentifier, Convert.ToString(id)),
                new Claim(ClaimTypes.Role, roles[rol])
            };

            ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new()
            {
                AllowRefresh = true,
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
            };

            return (new ClaimsPrincipal(identity), properties);
        }
    }
}
