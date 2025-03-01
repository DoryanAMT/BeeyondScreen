using BeeyondScreen.Data;
using BeeyondScreen.Models;
using Microsoft.EntityFrameworkCore;

namespace BeeyondScreen.Repositories
{
    public class RepositoryHorarioPelicula
    {
        private CineContext context;
        public RepositoryHorarioPelicula(CineContext context)
        {
            this.context = context;
        }

        public async Task<List<HorarioPelicula>> GetHorarioPeliculasAsync()
        {
            var consulta = from datos in this.context.HorarioPeliculas
                           orderby datos.HoraFuncion descending
                           select datos;
            return await consulta.ToListAsync();
        }

        public async Task<HorarioPelicula> FindHorarioPelicula
            (int idHorarioPelicula)
        {
            var consulta = from datos in this.context.HorarioPeliculas
                           where datos.IdHorario == idHorarioPelicula
                           select datos;
            return await consulta.FirstOrDefaultAsync();
        }
    }
}
