using pruebav2.Models;

namespace pruebav2.Repository.IRepository;

public interface ICategotiaRepository
{
    ICollection<Categoria> GetCategorias();
    Categoria GetCategoria(int CategoriaId);
    bool ExisteCategoria(int id);
    bool ExisteCategoria(string nombre);
    bool CrearCategoria(Categoria categoria);
    bool ActualizarCategoria(Categoria categoria);
    bool EliminarCategoria(Categoria categoria);
    bool Guardar();

}
