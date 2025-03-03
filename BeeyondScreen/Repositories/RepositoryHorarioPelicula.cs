using BeeyondScreen.Data;
using BeeyondScreen.Models;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
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

        public async Task<HorarioPelicula> FindHorarioPeliculaAsync
            (int idHorarioPelicula)
        {
            var consulta = from datos in this.context.HorarioPeliculas
                           where datos.IdHorario == idHorarioPelicula
                           select datos;
            return await consulta.FirstOrDefaultAsync();
        }
        public async Task InserHorarioPeliculaAsync
            (int idHorarioPelicula, int idPelicula, int idSala, 
            int idVersion, DateTime horaFuncion, int asientosDisponibles)
        {
            HorarioPelicula horarioPelicula = new HorarioPelicula();
            horarioPelicula.IdHorario = idHorarioPelicula;
            horarioPelicula.IdPelicula = idPelicula;
            horarioPelicula.IdSala = idSala;
            horarioPelicula.IdVersion = idVersion;
            horarioPelicula.HoraFuncion = horaFuncion;
            horarioPelicula.AsientosDisponibles = asientosDisponibles;
            await this.context.HorarioPeliculas.AddAsync(horarioPelicula);
            await this.context.SaveChangesAsync();
        }
        public async Task UpdateHorarioPeliculaAsync
            (int idHorarioPelicula, int idPelicula, int idSala,
            int idVersion, DateTime horaFuncion, int asientosDisponibles)
        {
            HorarioPelicula horarioPelicula = await this.FindHorarioPeliculaAsync(idHorarioPelicula);
            horarioPelicula.IdHorario = idHorarioPelicula;
            horarioPelicula.IdPelicula = idPelicula;
            horarioPelicula.IdSala = idSala;
            horarioPelicula.IdVersion = idVersion;
            horarioPelicula.HoraFuncion = horaFuncion;
            horarioPelicula.AsientosDisponibles = asientosDisponibles;
            await this.context.SaveChangesAsync();
        }
        public async Task DeleteHorarioPeliculaAsync
            (int idHorarioPelicula) 
        {
            HorarioPelicula horarioPelicula = await this.FindHorarioPeliculaAsync(idHorarioPelicula);
            this.context.Remove(horarioPelicula);
            await this.context.SaveChangesAsync();
        }
    }
}
