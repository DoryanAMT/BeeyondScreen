using BeeyondScreen.Extensions;
using BeeyondScreen.Models;
using Microsoft.AspNetCore.Mvc;
using BeeyondScreen.Repositories;
using System.Security.Claims;
using BeeyondScreen.Filters;

namespace BeeyondScreen.Controllers
{
    public class AsientosController : Controller
    {
        private RepositoryCine repo;
        public AsientosController(RepositoryCine repo)
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
        public async Task<IActionResult> Create
            ()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create
            (Asiento asiento)
        {
            await this.repo.InsertAsientoAsync(
                asiento.IdAsiento,
                asiento.IdSala,
                asiento.IdHorario,
                asiento.Numero,
                asiento.Fila,
                asiento.Disponible
                );
            return RedirectToAction("Index", "Peliculas");
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
                asiento.IdHorario,
                asiento.Numero,
                asiento.Fila,
                asiento.Disponible
                );
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete
            (int idAsiento)
        {
            await this.repo.DeleteAsientoAsync(idAsiento);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> AsientosReserva
            (int idHorario)
        {
            ModelAsientosReserva model = await this.repo.ReservaAsientoSalaHorarioId(idHorario);
            return View(model);

        }
        [AuthorizeUsers]
        [HttpPost]
        public async Task<IActionResult> AsientosReserva
            (Asiento asiento)
        {
            int idAsiento = await this.repo.GetLastIdAsientoAsync();
            await this.repo.InsertAsientoAsync(
                idAsiento,
                asiento.IdSala,
                asiento.IdHorario,
                asiento.Numero,
                asiento.Fila,
                asiento.Disponible
                );
            // GENERAMOS LOS BOLETOS POR CADA ENTRADA QUE HAYAMOS ESCOGIDO
            int dato = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            int idBoleto = await this.repo.GetLastIdBoletoAsync();
            await this.repo.InsertBoletoAsync(
                idBoleto,
                dato,
                asiento.IdHorario,
                idAsiento,
                DateTime.Now,
                "Confirmado"
                );

            return RedirectToAction("Index", "Peliculas");
        }
    }
}