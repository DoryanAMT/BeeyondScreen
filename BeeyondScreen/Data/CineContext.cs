using BeeyondScreen.Models;
using Microsoft.EntityFrameworkCore;
using MvcBeeyondScreen.Models;

namespace BeeyondScreen.Data
{
    public class CineContext: DbContext
    {
        public CineContext(DbContextOptions options) 
            :base(options){ }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<HorarioPelicula> HorarioPeliculas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Sala> Salas { get; set; }
        public DbSet<Models.Version> Versions { get; set; }
        public DbSet<Boleto> Boletos { get; set; }
        public DbSet<Asiento> Asientos { get; set; }
    }
}
