using System.ComponentModel.DataAnnotations;

namespace PreventApp.DTOs
{
    public class NewPasswordDTO
    {
        [Required(ErrorMessage = "Rellena este campo")]
        [StringLength(30)]
        [DataType(DataType.Password)]
        public string ContraseñaActual { get; set; } = null!;

        [Required(ErrorMessage = "Rellena este campo")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 15 caracteres")]
        [DataType(DataType.Password)]
        public string ContraseñaNueva { get; set; } = null!;
        
        [Required(ErrorMessage = "Rellena este campo")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 15 caracteres")]
        [DataType(DataType.Password)]
        [Compare("ContraseñaNueva", ErrorMessage = "Los campos <Contraseña nueva> y <Confirmar contraseña nueva> deben ser iguales")]
        public string ConfirmarContraseñaNueva { get; set; } = null!;
    }
}
