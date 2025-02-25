using BeeyondScreen.Models;
using BeeyondScreen.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BeeyondScreen.Controllers
{
    public class PeliculasController : Controller
    {
        RepositoryPelicula repo;
        public PeliculasController(RepositoryPelicula repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {

            List<Pelicula> peliculas = await this.repo.GetPeliculasAsync();
            return View(peliculas);
        }
    }
}
