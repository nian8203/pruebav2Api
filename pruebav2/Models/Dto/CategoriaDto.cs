using System.ComponentModel.DataAnnotations;

namespace pruebav2.Models.Dto;

public class CategoriaDto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string Nombre { get; set; }
    public DateTime FechaCreacion { get; set; }
}
