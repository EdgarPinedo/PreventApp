using System;
using System.Collections.Generic;

namespace PreventApp.Models;

public partial class Accidente
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public int? UsuarioId { get; set; }

    public int CategoriaId { get; set; }

    public DateTime Fecha { get; set; }

    public decimal? Longitud { get; set; }

    public decimal? Latitud { get; set; }

    public bool Estado { get; set; }

    public virtual Categoria Categoria { get; set; } = null!;

    public virtual Usuario? Usuario { get; set; }
}
