using System.ComponentModel.DataAnnotations;

namespace pruebav2.Models;

public class Categoria
{
    [Key]
    public int Id { get; set; }
    public string Nombre { get; set; }
    public DateTime FechaCreacion { get; set; }
}
