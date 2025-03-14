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
            (int idAsiento, int idSala, int idHorario,
            string numero, string fila, Boolean disponible)
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
        public async Task DeleteAsientoAsync
            (int idAsiento)
        {
            Asiento asiento = await this.FindAsientoAsync(idAsiento);
            this.context.Asientos.Remove(asiento);
            await this.context.SaveChangesAsync();
        }

        //  INSERT DE ASIENTOS SEGUN EL ID DEL HORARIO
        public async Task<ModelAsientosReserva> ReservaAsientoSalaHorarioId
            (int idHorarioPelicula)
        {
            ModelAsientosReserva model = new ModelAsientosReserva();
            HorarioPelicula horarioPelicula = await this.GetHorarioPeliculaAsync(idHorarioPelicula);
            // la vista me devuelve la informacion que necesito para recuperar los aientos que estan disponibles
            Pelicula pelicula = await this.FindPeliculaAsync(horarioPelicula.IdPelicula);
            List<Asiento> asientosOcupados = await this.GetAsientosSalaHorarioPeliculaAsync(idHorarioPelicula, horarioPelicula.IdSala);
            model.HorarioPelicula = horarioPelicula;
            model.Pelicula = pelicula;
            model.Asientos = asientosOcupados;
            //  POR OTRA PERTE NOS ENCARGAMOS DE GENERAR LOS BOLETOS POR CADA ASIENTO
            return model;

        }

        //  INSERTAR BOLETO
        public async Task InsertBoletoAsync
            (int idBoleto, int idUsuario, int idHorario,
            int idAsiento, DateTime fechaCompra, string estado)
        {
            
            Boleto boleto = new Boleto();
            boleto.IdBoleto = idBoleto;
            boleto.IdUsuario = idUsuario;
            boleto.IdHorario = idHorario;
            boleto.IdAsiento = idAsiento;
            boleto.FechaCompra = fechaCompra;
            boleto.Estado = estado;
            await this.context.AddAsync(boleto);
            await this.context.SaveChangesAsync();
        }

        public async Task<List<Asiento>> GetAsientosSalaHorarioPeliculaAsync
            (int idHorarioPelicula, int idSala)
        {
            return await this.context.Asientos
                .Where(x => x.IdHorario == idHorarioPelicula
                && x.IdSala == idSala)
                .ToListAsync();
        }

        public async Task<HorarioPelicula> GetHorarioPeliculaAsync
            (int idHorarioPelicula)
        {
            return await this.context.HorarioPeliculas
                .Where(x => x.IdHorario == idHorarioPelicula)
                .FirstOrDefaultAsync();
        }
        public async Task<Pelicula> FindPeliculaAsync
           (int idPelicula)
        {
            var consulta = from datos in this.context.Peliculas
                           .Where(x => x.IdPelicula == idPelicula)
                           select datos;
            return await consulta.FirstOrDefaultAsync();
        }
        public async Task<int> GetLastIdAsientoAsync()
        {
            {
                var consulta = this.context.Asientos.Any() ?
                    this.context.Asientos.Max(x => x.IdAsiento) + 1 :
                    1;
                int ultimoId = int.Parse(consulta.ToString());
                return ultimoId;
            }
        }
        public async Task<int> GetLastIdBoletoAsync()
        {
            {
                var consulta = this.context.Boletos.Any() ?
                    this.context.Boletos.Max(x => x.IdBoleto) + 1 :
                    1;
                int ultimoId = int.Parse(consulta.ToString());
                return ultimoId;
            }
        }
    }
}
