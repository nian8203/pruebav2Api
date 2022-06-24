using System.ComponentModel.DataAnnotations;

namespace pruebav2.Models.Dto;

public class UsuarioAuthLoginDto
{
    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string Usuario { get; set; }
    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string Password { get; set; }
}
