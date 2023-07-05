using System.ComponentModel.DataAnnotations;

namespace PreventApp.DTOs
{
    public class UsuarioDTO
    {
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(8)]
        [DataType(DataType.Password)]
        public string Contraseña { get; set; } = null!;

        [Required]
        [StringLength(8)]
        [DataType(DataType.Password)]
        public string? ConfirmarContraseña { get; set; }
    }
}
