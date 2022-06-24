using System.ComponentModel.DataAnnotations;

namespace pruebav2.Models.Dto;

public class UsuarioAuthDto  //crear usuario
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string Usuario { get; set; }
    [Required(ErrorMessage = "Este campo es obligatorio")]
    [StringLength(10, MinimumLength = 4, ErrorMessage = "Debe ingresar entre 10 y 4 caracteres")]
    public string Password { get; set; }
}
