using BeeyondScreen.Extensions;
using BeeyondScreen.Helpers;
using BeeyondScreen.Models;
using Microsoft.AspNetCore.Mvc;
using BeeyondScreen.Repositories;
using BeeyondScreen.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.CodeAnalysis.Elfie.Model;

namespace BeeyondScreen.Controllers
{
    public class UsuariosController : Controller
    {
        private RepositoryCine repo;
        public UsuariosController(RepositoryCine repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            List<Usuario> usuarios = await this.repo.GetUsuariosAsync();
            return View(usuarios);
        }
        public async Task<IActionResult> Details
            (int idUsuario)
        {
            Usuario usuario = await this.repo.FindUsuarioAsync(idUsuario);
            return View(usuario);
        }
        
        [AuthorizeUsers]
        public IActionResult Perfil
            ()
        {
            return View();
        }
        // **** CORREGIR CAMBIAR CONTRASEÑA
        [HttpPost]
        public async Task<IActionResult> Perfil
            (Usuario usuario, string currentPassword,
            string newPassword, string confirmPassword, bool cambiarPassword)
        {
            try
            {
                int dato = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                // Obtener el usuario actual de la base de datos
                Usuario usuarioActual = await this.repo.FindUsuarioAsync(dato);

                // Si se solicitó cambiar la contraseña
                if (cambiarPassword)
                {
                    // Verificar la contraseña actual
                    byte[] passActual = HelperCriptography.EncryptPassword(currentPassword, usuarioActual.Salt);
                    bool passCorrecta = HelperCriptography.CompararArrays(passActual, usuarioActual.Pass);

                    if (!passCorrecta)
                    {
                        ViewData["ERROR"] = "La contraseña actual es incorrecta";
                        return View();
                    }

                    // Verificar que las nuevas contraseñas coincidan
                    if (newPassword != confirmPassword)
                    {
                        ViewData["ERROR"] = "Las nuevas contraseñas no coinciden";
                        return View();
                    }

                    // Actualizar el usuario con la nueva contraseña
                    await this.repo.UpdateUsuarioAsync(
                        dato,
                        usuario.Nombre,
                        usuario.Email,
                        usuario.Imagen,
                        newPassword
                    );
                }
                else
                {
                    // Actualizar el usuario sin cambiar la contraseña
                    await this.repo.UpdateUsuarioProfileAsync(
                        dato,
                        usuario.Nombre,
                        usuario.Email,
                        usuario.Imagen
                    );
                }

                // Actualizar la sesión si es el usuario actual
                // Actualizar los claims

                ViewData["MENSAJE"] = "Perfil actualizado correctamente";
                return View();
            }
            catch (Exception ex)
            {
                ViewData["ERROR"] = "Error al actualizar el perfil: " + ex.Message;
                return View(usuario);
            }
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register
            (string nombre, string email, string imagen, string password)
        {
            await this.repo.RegisterUserAsync(nombre, email, password, imagen);
            ViewData["MENSAJE"] = "Usuario registrado correctamente";
            return View();
        }
        
        public async Task<IActionResult> BoletosUser()
        {
            int idUsuario = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            List<ViewFacturaBoleto> viewFacturaBoletos = await this.repo.GetFacturasBoletoUserAsync(idUsuario);
            return View(viewFacturaBoletos);
        }
        public async Task<IActionResult> DetailsBoletoUser
            (int idBoletoUser)
        {
            ViewFacturaBoleto viewFacturaBoleto = await this.repo.GetFacturaBoletoUserAsync(idBoletoUser);
            return View(viewFacturaBoleto);
        }
    }
}
