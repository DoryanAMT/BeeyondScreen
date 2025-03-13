using BeeyondScreen.Data;
using BeeyondScreen.DTOs;
using BeeyondScreen.Models;
using Microsoft.EntityFrameworkCore;
using MvcBeeyondScreen.Models;

namespace BeeyondScreen.Repositories
{
    public class RepositoryPelicula
    {
        CineContext context;
        public RepositoryPelicula(CineContext context)
        {
            this.context = context;
        }
        public async Task<ModelDetailsPelicula> GetDetailsPeliculaAsync
            (int idPelicula)
        {
            ModelDetailsPelicula model = new ModelDetailsPelicula();
            var detailsPelicula = await this.context.Peliculas
                .Where(x => x.IdPelicula == idPelicula)
                .FirstOrDefaultAsync();
            //var HorarioPelicula = await this.context.HorarioPeliculas
            return model;

        }

        //  RECUPERAR TODAS LAS PELICULAS
        public async Task<List<Pelicula>> GetPeliculasAsync()
        {
            var consulta = from datos in this.context.Peliculas
                           orderby datos.FechaLanzamiento descending
                           select datos;
            return await consulta.ToListAsync();
        }
        //  ENCONTRAR UN PELICULA
        public async Task<Pelicula> FindPeliculaAsync
            (int idPelicula)
        {
            var consulta = from datos in this.context.Peliculas
                           select datos;
            return await consulta.FirstOrDefaultAsync();
        }
        //  INSERTAR UNA PELICULA
        public async Task InsertPeliculaAsync
            (int idPelicula, string titulo, DateOnly fechaLanzamiento,
            int duracionMinutos, string tituloEtiqueta, string sinopsis,
            string imgFondo, string imgPoster)
        {
            Pelicula pelicula = new Pelicula();
            pelicula.IdPelicula = idPelicula;
            pelicula.Titulo = titulo;
            pelicula.FechaLanzamiento = fechaLanzamiento;
            pelicula.DuracionMinutos = duracionMinutos;
            pelicula.TituloEtiqueta = tituloEtiqueta;
            pelicula.Sinopsis = sinopsis;
            pelicula.ImgFondo = imgFondo;
            pelicula.ImgPoster = imgPoster;
            await this.context.Peliculas.AddAsync(pelicula);
            await this.context.SaveChangesAsync();
        }
        //  ACTUALIZAR UNA PELICULA
        public async Task UpdatePeliculaAsync
            (int idPelicula, string titulo, DateOnly fechaLanzamiento,
            int duracionMinutos, string tituloEtiqueta, string sinopsis,
            string imgFondo, string imgPoster)
        {
            Pelicula pelicula = await this.FindPeliculaAsync(idPelicula);
            pelicula.IdPelicula = idPelicula;
            pelicula.Titulo = titulo;
            pelicula.FechaLanzamiento = fechaLanzamiento;
            pelicula.DuracionMinutos = duracionMinutos;
            pelicula.TituloEtiqueta = tituloEtiqueta;
            pelicula.Sinopsis = sinopsis;
            pelicula.ImgFondo = imgFondo;
            pelicula.ImgPoster = imgPoster;
            await this.context.SaveChangesAsync();
        }
        //  BORRA PELICULA
        public async Task DeletePeliculaAsync
            (int idPelicula)
        {
            Pelicula pelicula = await this.FindPeliculaAsync(idPelicula);
            this.context.Peliculas.Remove(pelicula);
            await this.context.SaveChangesAsync();
        }
        
    }
}
