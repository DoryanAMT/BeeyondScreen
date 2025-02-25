using BeeyondScreen.Data;
using BeeyondScreen.Models;
using Microsoft.EntityFrameworkCore;

namespace BeeyondScreen.Repositories
{
    public class RepositoryPelicula
    {
        CineContext context;
        public RepositoryPelicula(CineContext context)
        {
            this.context = context;
        }
        public async Task<List<Pelicula>> GetPeliculasAsync()
        {
            var consulta = from datos in this.context.Peliculas
                           select datos;
            return await consulta.ToListAsync();
        }
    }
}
