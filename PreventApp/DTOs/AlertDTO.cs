using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PreventApp.DTOs
{
    public class AlertDTO
    {
        [Required]
        [StringLength(280)]
        [Unicode(false)]
        public string Descripcion { get; set; } = null!;
        public int? UsuarioId { get; set; }

        [Required]
        public int CategoriaId { get; set; }

        [Required]
        [Column(TypeName = "numeric(20, 16)")]
        public decimal Longitud { get; set; }

        [Required]
        [Column(TypeName = "numeric(20, 16)")]
        public decimal Latitud { get; set; }
    }
}
