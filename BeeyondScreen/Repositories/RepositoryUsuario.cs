using BeeyondScreen.Data;
using BeeyondScreen.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BeeyondScreen.Repositories
{
    public class RepositoryUsuario
    {
        private CineContext context;
        public RepositoryUsuario(CineContext context)
        {
            this.context = context;
        }
        
        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            var consulta = from datos in this.context.Usuarios
                           select datos;
            return await consulta.ToListAsync();
        }
        public async Task<Usuario> FindUsuarioAsync
            (int idUsuario)
        {
            var consulta = from datos in this.context.Usuarios
                           where datos.UsuarioId == idUsuario
                           select datos;
            return await consulta.FirstOrDefaultAsync();
        }
        public async Task InsertUsuario
            (int idUsuario, string nombre, string correo, 
            string contrasenaHash, DateTime fechaCreacion)
        {
            Usuario usuario = new Usuario();
            usuario.UsuarioId = idUsuario;
            usuario.Nombre = nombre;
            usuario.Correo = correo;
            usuario.ContrasenaHash = contrasenaHash;
            usuario.FechaCreacion = fechaCreacion;
            await this.context.AddAsync(usuario);
            await this.context.SaveChangesAsync();
        }
        public async Task UpdateUsuario
            (int idUsuario, string nombre,string correo, 
            string contrasenaHash, DateTime fechaCreacion)
        {
            Usuario usuario = await this.FindUsuarioAsync(idUsuario) ;
            usuario.UsuarioId = idUsuario;
            usuario.Nombre = nombre;
            usuario.Correo = correo;
            usuario.ContrasenaHash = contrasenaHash;
            usuario.FechaCreacion = fechaCreacion;
            await this.context.SaveChangesAsync();
        }
        public async Task DeleteUsuario
            (int idUsuario)
        {
            Usuario usuario = await this.FindUsuarioAsync(idUsuario);
            this.context.Remove(usuario);
            await this.context.SaveChangesAsync();
        }
    }
}
