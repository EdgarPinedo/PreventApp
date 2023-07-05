using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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
    }
}
