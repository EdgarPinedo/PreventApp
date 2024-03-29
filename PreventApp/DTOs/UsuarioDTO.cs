﻿using System.ComponentModel.DataAnnotations;

namespace PreventApp.DTOs
{
    public class UsuarioDTO
    {
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Rellena este campo")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 15 caracteres")]
        [DataType(DataType.Password)]
        public string Contraseña { get; set; } = null!;

        [Required(ErrorMessage = "Rellena este campo")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 15 caracteres")]
        [DataType(DataType.Password)]
        [Compare("Contraseña", ErrorMessage = "Los campos <Contraseña> y <Confirmar contraseña> deben ser iguales")]
        public string? ConfirmarContraseña { get; set; }
    }
}
