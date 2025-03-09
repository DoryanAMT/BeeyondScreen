using BeeyondScreen.Data;
using BeeyondScreen.DTOs;
using BeeyondScreen.Models;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using NuGet.Repositories;
using System.Collections.Immutable;

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
        public async Task<int> GetUltimoIdHorarioPelicula()
        {
            var consulta = this.context.HorarioPeliculas.Any() ?
                this.context.HorarioPeliculas.Max(x => x.IdHorario) :
                1;
            int ultimoId = int.Parse(consulta.ToString());
            return ultimoId;
        }



        //  COMBO PELICULAS
        public async Task<List<ComboPeliculas>> GetComboPeliculasAsync()
        {
            var consulta = await this.context.Peliculas
                .Select(x => new ComboPeliculas
                {
                    Id = x.IdPelicula,
                    Nombre = x.Titulo
                })
                .ToListAsync();
            return consulta;
        }
        //  COMBO SALAS
        public async Task<List<ComboSalas>> GetComboSalasAsync
            (int idCine)
        {
            var consulta = await this.context.Salas
                .Where(x => x.IdCine == idCine)
                .Select(x => new ComboSalas
                {
                    Id = x.IdSala,
                    Nombre = x.Nombre
                })
                .ToListAsync();
            return consulta;
        }
        //  COMBO VERSIONES
        public async Task<List<ComboVersiones>> GetComboVersionesAsync()
        {
            var consulta = await this.context.Versions
                .Select(x => new ComboVersiones
                {
                    Id = x.IdVersion,
                    Nombre = x.Idioma
                })
                .ToListAsync();
            return consulta;
        }
        //  CALENDARIO HORARIO SIN TERMINAR *******
        public async Task<List<Evento>> GetCalendarioAsync()
        {
            List<HorarioPelicula> horarioPeliculas = await this.GetHorarioPeliculasAsync();
            var eventos = new List<Evento>();
            foreach (HorarioPelicula horarioPelicula in horarioPeliculas)
            {
                new Evento
                {
                    Id = horarioPelicula.IdHorario,
                    Titulo = horarioPelicula.IdPelicula.ToString(),
                    FechaInicio = horarioPelicula.HoraFuncion,
                    FechaFin = horarioPelicula.HoraFuncion,
                    //INCLUIR LA DESCRIPCION DE LA PELICULA
                    //SUMA EL NUMERO DE MINUTOS DE LA PELICULA
                };
            }
            return eventos;
        }
    }
}
