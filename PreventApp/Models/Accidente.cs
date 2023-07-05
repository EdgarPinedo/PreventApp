using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PreventApp.Models;

public partial class Accidente
{
    [Key]
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    [StringLength(280)]
    [Unicode(false)]
    public string Descripcion { get; set; } = null!;

    public int? UsuarioId { get; set; }

    public int CategoriaId { get; set; }

    [ForeignKey("CategoriaId")]
    [InverseProperty("Accidentes")]
    public virtual Categoria Categoria { get; set; } = null!;

    [ForeignKey("UsuarioId")]
    [InverseProperty("Accidentes")]
    public virtual Usuario? Usuario { get; set; }
}
