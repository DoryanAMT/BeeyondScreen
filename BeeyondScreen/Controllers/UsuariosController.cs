using BeeyondScreen.Models;
using BeeyondScreen.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BeeyondScreen.Controllers
{
    public class UsuariosController : Controller
    {
        private RepositoryUsuario repo;
        public UsuariosController(RepositoryUsuario repo)
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
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create
            (Usuario usuario)
        {
            await this.repo.InsertUsuario(usuario.UsuarioId, usuario.Nombre,
                usuario.Correo ,usuario.ContrasenaHash, usuario.FechaCreacion);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit
            (int idUsuario)
        {
            Usuario usuario = await this.repo.FindUsuarioAsync(idUsuario);
            return View(usuario);
        }
        [HttpPost]
        public async Task<IActionResult> Edit
            (Usuario usuario)
        {
            await this.repo.UpdateUsuario(usuario.UsuarioId, usuario.Nombre,
               usuario.Correo, usuario.ContrasenaHash, usuario.FechaCreacion);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete
            (int idUsuario)
        {
            await this.repo.DeleteUsuario(idUsuario);
            return RedirectToAction("Index");
        }
    }
}
