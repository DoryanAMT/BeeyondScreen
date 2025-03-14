using BeeyondScreen.Data;
using BeeyondScreen.Helpers;
using BeeyondScreen.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MvcBeeyondScreen.Models;

namespace BeeyondScreen.Repositories
{
    public class RepositoryUsuario
    {
        private CineContext context;
        public RepositoryUsuario(CineContext context)
        {
            this.context = context;
        }

        public async Task<List<ViewFacturaBoleto>> GetFacturasBoletoUserAsync
            (int idUsuario)
        {
            return await this.context.ViewFacturaBoletos
                .Where(x => x.IdUsuario == idUsuario)
                .ToListAsync();
        }

        public async Task<ViewFacturaBoleto> GetFacturaBoletoUserAsync
            (int idBoletoUser)
        {
            return await this.context.ViewFacturaBoletos
                .Where(x => x.Id == idBoletoUser)
                .FirstOrDefaultAsync();
        }
        public async Task<int> GetLastIdUserAsync()
        {
            {
                var consulta = this.context.Usuarios.Any() ?
                    this.context.Usuarios.Max(x => x.IdUsuario) + 1 :
                    1;
                int ultimoId = int.Parse(consulta.ToString());
                return ultimoId;
            }
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
                           where datos.IdUsuario == idUsuario
                           select datos;
            return await consulta.FirstOrDefaultAsync();
        }
        //public async Task DeleteUsuario
        //    (int idUsuario)
        //{
        //    Usuario usuario = await this.FindUsuarioAsync(idUsuario);
        //    this.context.Remove(usuario);
        //    await this.context.SaveChangesAsync();
        //}
        // Actualizar perfil sin cambiar contraseña
        public async Task UpdateUsuarioProfileAsync
            (int idUsuario, string nombre, string email, 
            string imagen)
        {
            Usuario usuario = await this.FindUsuarioAsync(idUsuario);
            if (usuario != null)
            {
                usuario.Nombre = nombre;
                usuario.Email = email;
                usuario.Imagen = imagen;
                await this.context.SaveChangesAsync();
            }
        }

        // Actualizar perfil con cambio de contraseña
        public async Task UpdateUsuarioAsync
            (int idUsuario, string nombre, string email, 
            string imagen, string password)
        {
            Usuario usuario = await this.FindUsuarioAsync(idUsuario);
            if (usuario != null)
            {
                usuario.Nombre = nombre;
                usuario.Email = email;
                usuario.Imagen = imagen;
                // Actualizar la contraseña
                usuario.Pass = HelperCriptography.EncryptPassword(password, usuario.Salt);
                await this.context.SaveChangesAsync();
            }
        }

        public async Task RegisterUserAsync
            (string nombre, string email, string password,
            string imagen)
        {
            Usuario usuario = new Usuario();
            usuario.IdUsuario = await this.GetLastIdUserAsync();
            usuario.Nombre = nombre;
            usuario.Email = email;
            usuario.Imagen = imagen;
            usuario.Salt = HelperCriptography.GenerateSalt();
            usuario.Pass = HelperCriptography.EncryptPassword(password, usuario.Salt);
            usuario.FechaCreacion = DateTime.Now;
            this.context.Usuarios.Add(usuario);
            await this.context.SaveChangesAsync();
        }
        public async Task<Usuario> LoginUserAsync
            (string email, string password)
        {
            var consulta = from datos in this.context.Usuarios
                           where datos.Email == email
                           select datos;
            Usuario usuario = await consulta.FirstOrDefaultAsync();
            if (usuario == null)
            {
                return null;
            }
            else
            {
                string salt = usuario.Salt;
                byte[] temp = HelperCriptography.EncryptPassword(password, salt);
                byte[] passBytes = usuario.Pass;
                bool response = HelperCriptography.CompararArrays(temp, passBytes);
                if (response == true)
                {

                    return usuario;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
