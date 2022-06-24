using pruebav2.Models;

namespace pruebav2.Repository.IRepository;

public interface IUsuarioRepository
{
    ICollection<Usuario> GetUsuarios();
    Usuario GetUsuario(int UsuarioId);
    bool ExisteUsuario(string usuario);
    Usuario RegistroUsuario(Usuario usuario, string password);
    Usuario LoginUsuario(string usuario, string password);
    bool Guardar();

}
