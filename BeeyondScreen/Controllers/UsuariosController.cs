﻿using BeeyondScreen.Extensions;
using BeeyondScreen.Helpers;
using BeeyondScreen.Models;
using Microsoft.AspNetCore.Mvc;
using BeeyondScreen.Repositories;
using BeeyondScreen.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

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
            //int idUsuario = HttpContext.Session.GetObject<Usuario>("USUARIOCLIENTE").IdUsuario;
            //Usuario usuario = await this.repo.FindUsuarioAsync(idUsuario);
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int idUsuario)
        {
            Usuario usuario = await this.repo.FindUsuarioAsync(idUsuario);
            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Usuario usuario, string currentPassword,
            string newPassword, string confirmPassword, bool cambiarPassword)
        {
            try
            {
                // Obtener el usuario actual de la base de datos
                Usuario usuarioActual = await this.repo.FindUsuarioAsync(usuario.IdUsuario);

                // Si se solicitó cambiar la contraseña
                if (cambiarPassword)
                {
                    // Verificar la contraseña actual
                    byte[] passActual = HelperCriptography.EncryptPassword(currentPassword, usuarioActual.Salt);
                    bool passCorrecta = HelperCriptography.CompararArrays(passActual, usuarioActual.Pass);

                    if (!passCorrecta)
                    {
                        ViewData["ERROR"] = "La contraseña actual es incorrecta";
                        return View(usuario);
                    }

                    // Verificar que las nuevas contraseñas coincidan
                    if (newPassword != confirmPassword)
                    {
                        ViewData["ERROR"] = "Las nuevas contraseñas no coinciden";
                        return View(usuario);
                    }

                    // Actualizar el usuario con la nueva contraseña
                    await this.repo.UpdateUsuarioAsync(
                        usuario.IdUsuario,
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
                        usuario.IdUsuario,
                        usuario.Nombre,
                        usuario.Email,
                        usuario.Imagen
                    );
                }

                // Actualizar la sesión si es el usuario actual
                Usuario usuarioActualizado = await this.repo.FindUsuarioAsync(usuario.IdUsuario);
                Usuario usuarioSesion = HttpContext.Session.GetObject<Usuario>("USUARIOCLIENTE");

                if (usuarioSesion != null && usuarioSesion.IdUsuario == usuarioActualizado.IdUsuario)
                {
                    HttpContext.Session.SetObject("USUARIOCLIENTE", usuarioActualizado);
                }

                ViewData["MENSAJE"] = "Perfil actualizado correctamente";
                return View(usuarioActualizado);
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
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn
            (string email, string password)
        {
            Usuario usuario = await this.repo.LoginUserAsync(email, password);
            if (usuario == null)
            {
                ViewData["MENSAJE"] = "Credenciales incorrectas";
                return View();
            }
            else
            {
                ClaimsIdentity identity =
                new ClaimsIdentity(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    ClaimTypes.Name, ClaimTypes.Role);
                Claim claimName =
                    new Claim(ClaimTypes.Name, usuario.Nombre);
                identity.AddClaim(claimName);
          
                ClaimsPrincipal userPrincipal =
                    new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal);
              
                return RedirectToAction("Index", "Peliculas");
            }
        }
        public async Task<IActionResult> BoletosUser()
        {
            int idUsuario = HttpContext.Session.GetObject<Usuario>("USUARIOCLIENTE").IdUsuario;
            List<ViewFacturaBoleto> viewFacturaBoletos = await this.repo.GetFacturasBoletoUserAsync(idUsuario);
            return View(viewFacturaBoletos);
        }
        public async Task<IActionResult> DetailsBoletoUser
            (int idBoletoUser)
        {
            ViewFacturaBoleto viewFacturaBoleto = await this.repo.GetFacturaBoletoUserAsync(idBoletoUser);
            return View(viewFacturaBoleto);
        }

        public async Task<IActionResult> LogOut
            ()
        {
            HttpContext.Session.Remove("USUARIOCLIENTE");
            return RedirectToAction("Index", "Peliculas");
        }
    }
}
