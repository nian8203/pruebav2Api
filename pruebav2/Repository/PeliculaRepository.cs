using Microsoft.EntityFrameworkCore;
using pruebav2.Data;
using pruebav2.Models;
using pruebav2.Repository.IRepository;

namespace pruebav2.Repository;

public class PeliculaRepository : IPeliculaRepository
{
    private readonly AplicationDbContext _db;
    public PeliculaRepository(AplicationDbContext db)
    {
        _db = db;
    }

    
    public bool ActualizarPelicula(Pelicula pelicula)
    {
        _db.Peliculas.Update(pelicula);
        return Guardar();
    }

    public IEnumerable<Pelicula> BuscarPelicula(string nombre)
    {
        IQueryable<Pelicula> query = _db.Peliculas;

        if (!string.IsNullOrWhiteSpace(nombre))
            query = query.Where(p => p.Nombre.Contains(nombre) || p.Descripcion.Contains(nombre));

        return query.ToList();
    }

    public bool CrearPelicula(Pelicula pelicula)
    {
        _db.Peliculas.Add(pelicula);
        return Guardar();
    }

    public bool EliminarPelicula(Pelicula pelicula)
    {
        _db.Peliculas.Remove(pelicula);
        return Guardar();
    }

    public bool ExistePelicula(int id)
    {
        return _db.Peliculas.Any(c => c.Id == id);        
    }

    public bool ExistePelicula(string nombre)
    {
        bool res = _db.Peliculas.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
        return res;
    }

    public Pelicula GetPelicula(int PeliculaId)
    {
        return _db.Peliculas.FirstOrDefault(c => c.Id == PeliculaId);
        
    }

    public ICollection<Pelicula> GetPeliculas()
    {
        return _db.Peliculas.OrderBy(c => c.Nombre).ToList();
    }

    public ICollection<Pelicula> GetPeliculasPorCategoria(int catId)
    {
        return _db.Peliculas.Include(c => c.Categoria).Where(p => p.categoriaId == catId).ToList();
    }

    public bool Guardar() 
    {
        return _db.SaveChanges() >= 0 ? true : false;
    }
}
