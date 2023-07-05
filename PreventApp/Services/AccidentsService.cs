using Microsoft.EntityFrameworkCore;
using PreventApp.DTOs;
using PreventApp.Models;

namespace PreventApp.Services
{
    public class AccidentsService
    {
        private readonly PreventAppDbContext _context;

        public AccidentsService(PreventAppDbContext context)
        {
            _context = context;
        }

        public async Task<Paginacion<Accidente>> GetAccidents(string filtro, int? numpag)
        {
            int cantidadRegistros = 10;

            IQueryable<Accidente> accidentes = _context.Accidentes;

            if (filtro == "trafico") accidentes = accidentes.Where(a => a.CategoriaId == 1);

            if (filtro == "incendios") accidentes = accidentes.Where(a => a.CategoriaId == 2);

            if (filtro == "robos") accidentes = accidentes.Where(a => a.CategoriaId == 3);

            if (filtro == "fallecimientos") accidentes = accidentes.Where(a => a.CategoriaId == 4);

            accidentes = accidentes.Include(a => a.Categoria).Include(a => a.Usuario);
            return await Paginacion<Accidente>.Paginar(accidentes, numpag ?? 1, cantidadRegistros);
        }

        public async Task<List<Accidente>> GetRecentAccidents()
        {
            return await _context.Accidentes.ToListAsync();
        }

        public async Task<Accidente?> Details(int? id)
        {
            return await _context.Accidentes
                .Include(a => a.Categoria)
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task DeleteAccident(int? id)
        {
            var accidente = await _context.Accidentes.FindAsync(id);
            if (accidente != null)
            {
                _context.Accidentes.Remove(accidente);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<AccidenteDTO?> GetSingleAccident(int? id)
        {
            var accidente = await _context.Accidentes.FirstOrDefaultAsync(m => m.Id == id);
            if (accidente != null)
            {
                AccidenteDTO acc = new()
                {
                    Id = accidente.Id,
                    Fecha = accidente.Fecha,
                    Descripcion = accidente.Descripcion,
                    UsuarioId = accidente.UsuarioId,
                    CategoriaId = accidente.CategoriaId
                };
                return acc;
            }
            return null;
        }

        public async Task EditAccident(AccidenteDTO accidente)
        {
            Accidente acc = new()
            {
                Id = accidente.Id,
                Fecha = accidente.Fecha,
                Descripcion = accidente.Descripcion,
                UsuarioId = accidente.UsuarioId,
                CategoriaId = accidente.CategoriaId
            };

            _context.Update(acc);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Accidente>> GetAccidentsByUser(int id)
        {
            return await _context.Accidentes.Where(x => x.UsuarioId == id)
                                            .Include(x => x.Categoria)
                                            .ToListAsync();
        }
    }
}
