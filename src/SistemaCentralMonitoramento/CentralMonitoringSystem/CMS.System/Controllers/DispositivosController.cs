using CMS.System.Data.Models;
using CMS.System.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CMS.System.Controllers
{
    public class DispositivosController : Controller
    {
        private readonly IDispositivoRepository catalogo;
        public DispositivosController(IDispositivoRepository context)
        {
            this.catalogo = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var dispositivos = await catalogo.GetDispositivosById();

            return View(dispositivos);
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute]string id)
        {
            var dispositivo = await catalogo.GetDispositivosById(id);

            return View(dispositivo.FirstOrDefault());
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Dispositivo dispositivo)
        {
            await catalogo.UpdateDispositivo(await ValidaEdicao(dispositivo));

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var dispositivo = await catalogo.GetDispositivosById(id);

            return View(dispositivo.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Dispositivo dispositivo)
        {
            await catalogo.RemoveDispositivo(dispositivo.ID_DEVICE);

            return RedirectToAction("Index");
        }
        private async Task<Dispositivo> ValidaEdicao(Dispositivo dispositivo)
        {
            dispositivo.DT_ALTERACAO = DateTime.Now;

            return dispositivo;
        }
    }
}
