using System;
using System.Collections.Generic;

namespace PreventApp.Models;

public partial class Categoria
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Accidente> Accidentes { get; set; } = new List<Accidente>();
}
