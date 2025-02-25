using BeeyondScreen.Models;
using Microsoft.EntityFrameworkCore;

namespace BeeyondScreen.Data
{
    public class CineContext: DbContext
    {
        public CineContext(DbContextOptions options) 
            :base(options){ }
        public DbSet<Pelicula> Peliculas { get; set; }
    }
}
