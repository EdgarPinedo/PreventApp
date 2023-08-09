using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PreventApp.DTOs
{
    public class UserDashboardDTO
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        [Unicode(false)]
        public string? Nombre { get; set; }

        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Email { get; set; } = null!;

        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10}$", ErrorMessage = "El telefono no es válido")]
        public string? Telefono { get; set; }

        [Required]
        public int RolId { get; set; }
    }
}
