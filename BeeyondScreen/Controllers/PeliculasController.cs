using BeeyondScreen.Models;
using BeeyondScreen.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

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
        public async Task<IActionResult> Details
            (int idPelicula)
        {
            Pelicula pelicula = await this.repo.FindPeliculaAsync(idPelicula);
            return View(pelicula);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create
            (Pelicula pelicula)
        {
            await this.repo.InsertPeliculaAsync(pelicula.IdPelicula, pelicula.Titulo,
                pelicula.FechaLanzamiento, pelicula.DuracionMinutos, pelicula.TituloEtiqueta,
                pelicula.Sinopsis, pelicula.ImgFondo, pelicula.ImgPoster);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update
            (int idPelicula)
        {
            Pelicula pelicula = await this.repo.FindPeliculaAsync(idPelicula);
            return View(pelicula);
        }
        [HttpPost]
        public async Task<IActionResult> Update
                (Pelicula pelicula)
        {
            await this.repo.UpdatePeliculaAsync(pelicula.IdPelicula, pelicula.Titulo,
                pelicula.FechaLanzamiento, pelicula.DuracionMinutos, pelicula.TituloEtiqueta,
                pelicula.Sinopsis, pelicula.ImgFondo, pelicula.ImgPoster);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete
            (int idPelicula)
        {
            await this.repo.DeletePeliculaAsync(idPelicula);
            return RedirectToAction("Index");
        }

        

    }
}
