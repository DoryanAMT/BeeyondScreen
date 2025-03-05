using BeeyondScreen.Data;
using BeeyondScreen.Helpers;
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
        private async Task<int> GetMaxIdUser()
        {
            if (this.context.Usuarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await this.context.Usuarios.MaxAsync
                    (x => x.IdUsuario) + 1;
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
        //public async Task InsertUsuario
        //    (int idUsuario, string nombre, string email, 
        //    string contrasenaHash, DateTime fechaCreacion)
        //{
        //    Usuario usuario = new Usuario();
        //    usuario.IdUsuario = idUsuario;
        //    usuario.Nombre = nombre;
        //    usuario.Email = email;
        //    usuario.Pass = contrasenaHash;
        //    usuario.FechaCreacion = fechaCreacion;
        //    await this.context.AddAsync(usuario);
        //    await this.context.SaveChangesAsync();
        //}
        //public async Task UpdateUsuario
        //    (int idUsuario, string nombre,string correo, 
        //    string contrasenaHash, DateTime fechaCreacion)
        //{
        //    Usuario usuario = await this.FindUsuarioAsync(idUsuario) ;
        //    usuario.UsuarioId = idUsuario;
        //    usuario.Nombre = nombre;
        //    usuario.Correo = correo;
        //    usuario.ContrasenaHash = contrasenaHash;
        //    usuario.FechaCreacion = fechaCreacion;
        //    await this.context.SaveChangesAsync();
        //}
        //public async Task DeleteUsuario
        //    (int idUsuario)
        //{
        //    Usuario usuario = await this.FindUsuarioAsync(idUsuario);
        //    this.context.Remove(usuario);
        //    await this.context.SaveChangesAsync();
        //}
        public async Task RegisterUserAsync
            (string nombre, string email, string password,
            string imagen)
        {
            Usuario usuario = new Usuario();
            usuario.IdUsuario = await this.GetMaxIdUser();
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
