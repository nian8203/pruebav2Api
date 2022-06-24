using pruebav2.Models;

namespace pruebav2.Repository.IRepository;

public interface IPeliculaRepository
{
    ICollection<Pelicula> GetPeliculas();
    ICollection<Pelicula> GetPeliculasPorCategoria(int catId);
    Pelicula GetPelicula(int PeliculaId);
    bool ExistePelicula(int id);
    IEnumerable<Pelicula> BuscarPelicula(string nombre);
    bool ExistePelicula(string nombre);
    bool CrearPelicula(Pelicula pelicula);
    bool ActualizarPelicula(Pelicula pelicula);
    bool EliminarPelicula(Pelicula pelicula);
    bool Guardar();

}
