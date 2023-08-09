using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PreventApp.DTOs
{
    public class CreateUserDTO
    {
        [StringLength(50)]
        [Unicode(false)]
        public string? Nombre { get; set; }

        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 15 caracteres")]
        [DataType(DataType.Password)]
        public string Contraseña { get; set; } = null!;

        [Required]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "<Confirmar contraseña> debe tener entre 6 y 15 caracteres")]
        [DataType(DataType.Password)]
        [Compare("Contraseña", ErrorMessage = "Los campos <Contraseña> y <Confirmar contraseña> deben ser iguales")]
        public string ConfirmarContraseña { get; set; } = null!;

        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10}$", ErrorMessage = "El telefono no es válido")]
        public string? Telefono { get; set; }

        [Required]
        public int RolId { get; set; }
    }
}
