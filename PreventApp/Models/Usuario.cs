using System;
using System.Collections.Generic;

namespace PreventApp.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string Email { get; set; } = null!;

    public string? Telefono { get; set; }

    public string Contraseña { get; set; } = null!;

    public int RolId { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Accidente> Accidentes { get; set; } = new List<Accidente>();

    public virtual Rol Rol { get; set; } = null!;
}
