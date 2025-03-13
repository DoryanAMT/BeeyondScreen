using BeeyondScreen.Data;
using BeeyondScreen.Models;
using Microsoft.EntityFrameworkCore;
using MvcBeeyondScreen.Models;

namespace MvcBeeyondScreen.Repositories
{
    public class RepositoryAsientos
    {
        private CineContext context;
        public RepositoryAsientos(CineContext context)
        {
            this.context = context;
        }

        public async Task<List<Asiento>> GetAsientosAsync()
        {
            return await this.context.Asientos.ToListAsync();
        }
        public async Task<Asiento> FindAsientoAsync
            (int idAsiento)
        {
            return await this.context.Asientos
                .Where(x => x.IdAsiento == idAsiento)
                .FirstOrDefaultAsync();
        }

        public async Task InsertAsientoAsync
            (int idAsiento, int idSala, int idHorario,
            string numero, string fila, Boolean disponible)
        {
            Asiento asiento = new Asiento();
            asiento.IdAsiento = idAsiento;
            asiento.IdSala = idSala;
            asiento.IdHorario = idHorario;
            asiento.Numero = numero;
            asiento.Fila = fila;
            asiento.Disponible = disponible;
            await this.context.Asientos.AddAsync(asiento);
            await this.context.SaveChangesAsync();
        }
        
        public async Task UpdateAsientoAsync
            (int idAsiento, int idSala, string numero,
            int idHorario, string fila, Boolean disponible)
        {
            Asiento asiento = await this.FindAsientoAsync(idAsiento);
            asiento.IdAsiento = idAsiento;
            asiento.IdSala = idSala;
            asiento.IdHorario = idHorario;
            asiento.Numero = numero;
            asiento.Fila = fila;
            asiento.Disponible = disponible;
            await this.context.SaveChangesAsync();
        }

        //  INSERT DE ASIENTOS SEGUN EL ID DEL HORARIO
        public async Task ReservaAsientoSalaHorarioId
            (int idHorarioPelicula)
        {
            HorarioPelicula horarioPelicula = await this.GetHorarioPeliculaAsync(idHorarioPelicula);
            // la vista me devuelve la informacion que necesito para recuperar los aientos que estan disponibles


        }

        public async Task<List<Asiento>> GetAsientosSalaHorarioPeliculaAsync
            (int idHorarioPelicula)
        {
            HorarioPelicula horarioPelicula = await this.GetHorarioPeliculaAsync(idHorarioPelicula);
            List<Asiento> asientos = await 
        }

        public async Task<HorarioPelicula> GetHorarioPeliculaAsync
            (int idHorarioPelicula)
        {
            return await this.context.HorarioPeliculas
                .Where(x => x.IdHorario == idHorarioPelicula)
                .FirstOrDefaultAsync();
        }

    }
}
