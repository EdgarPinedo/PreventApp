using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PreventApp.Models;

[Table("Usuario")]
public partial class Usuario
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Nombre { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    public string? Telefono { get; set; }

    [StringLength(64)]
    [Unicode(false)]
    public string Contraseña { get; set; } = null!;

    public int RolId { get; set; }

    [InverseProperty("Usuario")]
    public virtual ICollection<Accidente> Accidentes { get; } = new List<Accidente>();

    [ForeignKey("RolId")]
    [InverseProperty("Usuarios")]
    public virtual Rol Rol { get; set; } = null!;
}
