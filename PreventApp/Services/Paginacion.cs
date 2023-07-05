using Microsoft.EntityFrameworkCore;

namespace PreventApp.Services
{
    public class Paginacion<T> : List<T>
    {
        public int PaginaActual { get; private set; }

        public int PaginasTotales { get; private set; }

        public Paginacion(List<T> items, int totalRegistros, int paginaActual, int cantidadRegistros)
        {
            PaginaActual = paginaActual;
            PaginasTotales = (int)Math.Ceiling(totalRegistros / (double)cantidadRegistros);

            this.AddRange(items);
        }

        public bool PaginasAnteriores => PaginaActual > 1;
        public bool PaginasSiguientes => PaginaActual < PaginasTotales;

        public static async Task<Paginacion<T>> Paginar(IQueryable<T> datos, int paginaActual, int cantidadRegistros)
        {
            var totalRegistros = await datos.CountAsync();
            var items = await datos.Skip((paginaActual - 1) * cantidadRegistros).Take(cantidadRegistros).ToListAsync();
            return new Paginacion<T>(items, totalRegistros, paginaActual, cantidadRegistros);
        }
    }
}
