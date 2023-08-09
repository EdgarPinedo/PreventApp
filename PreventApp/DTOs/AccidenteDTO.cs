using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PreventApp.DTOs
{
    public class AccidenteDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Fecha { get; set; }

        [Required]
        [StringLength(280)]
        [Unicode(false)]
        public string Descripcion { get; set; } = null!;
        
        public int? UsuarioId { get; set; }

        [Required]
        public int CategoriaId { get; set; }

        [Column(TypeName = "numeric(20, 16)")]
        public decimal? Longitud { get; set; }

        [Column(TypeName = "numeric(20, 16)")]
        public decimal? Latitud { get; set; }

        [Required]
        public bool Estado { get; set; }
    }
}
