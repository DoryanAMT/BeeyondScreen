using BeeyondScreen.Models;
using BeeyondScreen.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BeeyondScreen.Controllers
{
    public class BoletosController : Controller
    {
        private RepositoryBoletos repo;
        public BoletosController(RepositoryBoletos repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            List<Boleto> boletos = await this.repo.GetBoletosAsync();
            return View(boletos);
        }
        public async Task<IActionResult> Details
            (int idBoleto)
        {
            Boleto boleto = await this.repo.FindBoletoAsync(idBoleto);
            return View(boleto);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create
            (Boleto boleto)
        {
            //  EL HORARIO LO SELECCIONARA DESDE UN VISTA DE RESERVA
            int ultimoId = await this.repo.GetUltimoIdBoletoAsync();
            await this.repo.InsertBoletoAsync(
                ultimoId,
                boleto.IdUsuario,
                boleto.IdAsiento,
                boleto.FechaCompra,
                boleto.Estado);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update
            (int idBoleto)
        {
            Boleto boleto = await this.repo.FindBoletoAsync(idBoleto);
            return View(boleto);
        }
        [HttpPost]
        public async Task<IActionResult> Update
                (Boleto boleto)
        {
            //  EL HORARIO LO SELECCIONARA DESDE UN VISTA DE RESERVA
            int ultimoId = await this.repo.GetUltimoIdBoletoAsync();
            await this.repo.UpdateBoletoAsync(
                ultimoId,
                boleto.IdUsuario,
                boleto.IdAsiento,
                boleto.FechaCompra,
                boleto.Estado);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete
            (int idBoleto)
        {
            await this.repo.DeleteBoletoAsync(idBoleto);
            return RedirectToAction("Index");
        }

    }
}
