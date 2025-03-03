using BeeyondScreen.Models;
using BeeyondScreen.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BeeyondScreen.Controllers
{
    public class HorarioPeliculasController : Controller
    {
        RepositoryHorarioPelicula repo;
        public HorarioPeliculasController(RepositoryHorarioPelicula repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            List<HorarioPelicula> horarioPeliculas = await this.repo.GetHorarioPeliculasAsync();
            return View(horarioPeliculas);
        }
        public async Task<IActionResult> Details
            (int idHorarioPelicula)
        {
            HorarioPelicula horarioPelicula = await this.repo.FindHorarioPeliculaAsync(idHorarioPelicula);
            return View(horarioPelicula);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create
            (HorarioPelicula horarioPelicula)
        {
            await this.repo.InserHorarioPeliculaAsync(horarioPelicula.IdHorario, horarioPelicula.IdPelicula,
                horarioPelicula.IdSala, horarioPelicula.IdVersion, horarioPelicula.HoraFuncion,
                horarioPelicula.AsientosDisponibles);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit
            (int idHorarioPelicula)
        {
            HorarioPelicula horarioPelicula = await this.repo.FindHorarioPeliculaAsync(idHorarioPelicula);
            return View(horarioPelicula);
        }
        [HttpPost]
        public async Task<IActionResult> Edit
            (HorarioPelicula horarioPelicula)
        {
            await this.repo.UpdateHorarioPeliculaAsync(horarioPelicula.IdHorario, horarioPelicula.IdPelicula,
                horarioPelicula.IdSala, horarioPelicula.IdVersion, horarioPelicula.HoraFuncion,
                horarioPelicula.AsientosDisponibles);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete
            (int idHorarioPelicula)
        {
            await this.repo.DeleteHorarioPeliculaAsync(idHorarioPelicula);
            return RedirectToAction("Index");
        }
    }
}
