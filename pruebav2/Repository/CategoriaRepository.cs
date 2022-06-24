using pruebav2.Data;
using pruebav2.Models;
using pruebav2.Repository.IRepository;

namespace pruebav2.Repository;

public class CategoriaRepository : ICategotiaRepository
{
    private readonly AplicationDbContext _db;
    public CategoriaRepository(AplicationDbContext db)
    {
        _db = db;
    }

    
    public bool ActualizarCategoria(Categoria categoria)
    {
        _db.Categorias.Update(categoria);
        return Guardar();
    }

    public bool CrearCategoria(Categoria categoria)
    {
        _db.Categorias.Add(categoria);
        return Guardar();
    }

    public bool EliminarCategoria(Categoria categoria)
    {
        _db.Categorias.Remove(categoria);
        return Guardar();
    }

    public bool ExisteCategoria(int id)
    {
        return _db.Categorias.Any(c => c.Id == id);        
    }

    public bool ExisteCategoria(string nombre)
    {
        bool res = _db.Categorias.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
        return res;
    }

    public Categoria GetCategoria(int CategoriaId)
    {
        return _db.Categorias.FirstOrDefault(c => c.Id == CategoriaId);
        
    }

    public ICollection<Categoria> GetCategorias()
    {
        return _db.Categorias.OrderBy(c => c.Nombre).ToList();
    }

    public bool Guardar()
    {
        return _db.SaveChanges() >= 0 ? true : false;
    }
}
