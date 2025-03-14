using BeeyondScreen.Data;
using BeeyondScreen.DTOs;
using BeeyondScreen.Models;
using Microsoft.EntityFrameworkCore;

namespace BeeyondScreen.Repositories
{
    public class RepositoryBoletos
    {
        private CineContext context;
        public RepositoryBoletos(CineContext context)
        {
            this.context = context;
        }

        //  CRUD DE BOLETOS
        public async Task<List<Boleto>> GetBoletosAsync()
        {
            return await this.context.Boletos.ToListAsync();
        }
        public async Task<Boleto> FindBoletoAsync
            (int idBoleto)
        {
            return await this.context.Boletos
                .Where(x => x.IdBoleto == idBoleto)
                .FirstOrDefaultAsync();
        }
        //  INSERTAR BOLETO
        public async Task InsertBoletoAsync
            (int idBoleto, int idUsuario, int idAsiento,
            DateTime fechaCompra, string estado)
        {
            fechaCompra = DateTime.Now;
            Boleto boleto = new Boleto();
            boleto.IdBoleto = idBoleto;
            boleto.IdUsuario = idUsuario;
            boleto.IdAsiento = idAsiento;
            boleto.FechaCompra = fechaCompra;
            boleto.Estado = estado;
            await this.context.AddAsync(boleto);
            await this.context.SaveChangesAsync();
        }
        //  ACTUALIZAR UN BOLETO
        public async Task UpdateBoletoAsync
            (int idBoleto, int idUsuario, int idAsiento,
            DateTime fechaCompra, string estado)
        {
            Boleto boleto = await this.FindBoletoAsync(idBoleto);
            boleto.IdBoleto = idBoleto;
            boleto.IdUsuario = idUsuario;
            boleto.IdAsiento = idAsiento;
            boleto.FechaCompra = fechaCompra;
            boleto.Estado = estado;
            await this.context.SaveChangesAsync();
        }
        //  ELIMINAR UN BOLETO
        public async Task DeleteBoletoAsync
            (int idBoleto)
        {
            Boleto boleto = await this.FindBoletoAsync(idBoleto);
            this.context.Remove(boleto);
            await this.context.SaveChangesAsync();
        }
        
        //  GET ULTIMO ID BOLETO
        public async Task<int> GetLastIdBoletoAsync()
        {
            var consulta = this.context.Boletos.Any() ?
                this.context.Boletos.Max(x => x.IdBoleto) + 1 :
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

        //  FIND PELIUCLA
        public async Task<Pelicula> FindPeliculaAsync
            (int idPelicula)
        {
            Pelicula pelicula = await this.context.Peliculas
                .Where(x => x.IdPelicula == idPelicula)
                .FirstOrDefaultAsync();
            return pelicula;
        }
    }
}
