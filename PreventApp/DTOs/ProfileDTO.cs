using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace PreventApp.DTOs
{
    public class ProfileDTO
    {
        [StringLength(50)]
        [Unicode(false)]
        public string? Nombre { get; set; }

        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Email { get; set; } = null!;

        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10}$", ErrorMessage = "El telefono no es válido")]
        public string? Telefono { get; set; }
    }
}
