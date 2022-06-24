using Microsoft.EntityFrameworkCore;
using pruebav2.Data;
using pruebav2.Models;
using pruebav2.Repository.IRepository;

namespace pruebav2.Repository;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AplicationDbContext _db;
    public UsuarioRepository(AplicationDbContext db)
    {
        _db = db;
    }

    public bool ExisteUsuario(string usuario)
    {
        if (_db.Usuarios.Any(u => u.UsuarioA == usuario))
            return true;

        return false;
    }

    public Usuario GetUsuario(int UsuarioId)
    {
        return _db.Usuarios.FirstOrDefault(u => u.Id == UsuarioId);
    }

    public ICollection<Usuario> GetUsuarios()
    {
        return _db.Usuarios.OrderBy(u => u.UsuarioA).ToList();
    }

    public Usuario LoginUsuario(string usuario, string password)
    {
        var user = _db.Usuarios.FirstOrDefault(u => u.UsuarioA == usuario);

        if (user == null)
            return null;

        if (!VerificarHash(password, user.PasswordHash, user.PasswordSalt))
            return null;

        return user;
    }


    public Usuario RegistroUsuario(Usuario usuario, string password)
    {
        byte[] passwordHash, passwordsalt;

        CrearPasswordHash(password, out passwordHash, out passwordsalt);

        usuario.PasswordHash = passwordHash;
        usuario.PasswordSalt = passwordsalt;

        _db.Usuarios.Add(usuario);
        Guardar();
        return usuario;
    }

    public bool Guardar()
    {
        return _db.SaveChanges() >= 0 ? true : false;
    }


    private bool VerificarHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
        {
            var hashComputado = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < hashComputado.Length; i++)
            {
                if (hashComputado[i] != passwordHash[i]) return false;
            }
        }

        return true;
    }

    private void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] passwordsalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordsalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

}
