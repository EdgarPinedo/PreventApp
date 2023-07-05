using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PreventApp.Models;

public partial class Categoria
{
    [Key]
    public int Id { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [InverseProperty("Categoria")]
    public virtual ICollection<Accidente> Accidentes { get; } = new List<Accidente>();
}
