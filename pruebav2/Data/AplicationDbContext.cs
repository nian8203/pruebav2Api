using Microsoft.EntityFrameworkCore;
using pruebav2.Models;

namespace pruebav2.Data
{
    public class AplicationDbContext : DbContext
    {
        public AplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
