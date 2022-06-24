using System.ComponentModel.DataAnnotations;
using static pruebav2.Models.Pelicula;

namespace pruebav2.Models.Dto;

public class PeliculaCreateDto
{
    [Required(ErrorMessage = "este campo es obligatorio")]
    public string Nombre { get; set; }
    public string RutaImagen { get; set; }
    public IFormFile Foto { get; set; }
    [Required(ErrorMessage = "este campo es obligatorio")]   
    public string Descripcion { get; set; }
    [Required(ErrorMessage = "este campo es obligatorio")]
    public string Duracion { get; set; }
    public TipoClasificacion Clasificacion { get; set; }
    public int categoriaId { get; set; }
}
