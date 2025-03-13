using Microsoft.AspNetCore.Mvc;
using MvcBeeyondScreen.Models;
using MvcBeeyondScreen.Repositories;

namespace MvcBeeyondScreen.Controllers
{
    public class AsientosController : Controller
    {
        private RepositoryAsientos repo;
        public AsientosController(RepositoryAsientos repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            List<Asiento> asientos = await this.repo.GetAsientosAsync();
            return View(asientos);
        }
        public async Task<IActionResult> Details
            (int idAsiento)
        {
            Asiento asiento = await this.repo.FindAsientoAsync(idAsiento);
            return View(asiento);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create
            (Asiento asiento)
        {
            await this.repo.InsertAsientoAsync(
                asiento.IdAsiento,
                asiento.IdHorario,
                asiento.IdSala,
                asiento.Numero,
                asiento.Fila,
                asiento.Disponible
                );
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update
            (int idAsiento)
        {
            Asiento asiento = await this.repo.FindAsientoAsync(idAsiento);
            return View(asiento);
        }
        [HttpPost]
        public async Task<IActionResult> Update
            (Asiento asiento)
        {
            await this.repo.UpdateAsientoAsync(
                asiento.IdAsiento,
                asiento.IdSala,
                asiento.Numero,
                asiento.Fila,
                asiento.Disponible
                );
            return RedirectToAction("Index");
        }
    }
}
