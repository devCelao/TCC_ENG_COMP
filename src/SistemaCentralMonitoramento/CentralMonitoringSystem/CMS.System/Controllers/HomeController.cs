using CMS.System.Data.Models;
using CMS.System.Data.Repositories;
using CMS.System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CMS.System.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDispositivoRepository catalogo;

        public HomeController(IDispositivoRepository context)
        {
            this.catalogo = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = await FiltraLista(2,0,0);
            var retorno = new MedicaoViewModel(data);
            var dispositivos = await catalogo.GetDispositivosById();

            retorno.Dispositivos = dispositivos
                                        .Select(d => Convert.ToInt32(d.ID_DEVICE))
                                        .ToList();

            retorno.DispositivosGrupos = dispositivos
                                            .Where(d => d.ID_GROUP != 0)
                                            .Select(d => d.ID_GROUP)
                                            .Distinct()
                                            .ToList();
            return View(retorno);
        }
        [HttpPost]
        public async Task<IActionResult> Index(MedicaoViewModel model)
        {
            var data = await FiltraLista(model.filtro, model.DispositivoSelecionado, model.GrupoSelecionado) ;
            var retorno = new MedicaoViewModel(data, model);
            var dispositivos = await catalogo.GetDispositivosById();

            retorno.Dispositivos = dispositivos
                                        .Select(d => Convert.ToInt32(d.ID_DEVICE))
                                        .ToList();
            retorno.DispositivosGrupos = dispositivos
                                            .Where(d =>  d.ID_GROUP != 0)
                                            .Select(d => d.ID_GROUP)
                                            .Distinct()
                                            .ToList();
            return View(retorno);
        }

        private async Task<List<DispositivoMedicao>> FiltraLista(int filtro, int ID_DEVICE, int GRUPO)
         {

            var data = GRUPO != 0 ? await catalogo.GetMedicoesByGroup(GRUPO) : await catalogo.GetMedicoes();


            if (filtro == 2)
            {
                data = data.Where(c => c.DT_PUBLICACAO.Date == DateTime.Today.Date).ToList();
            }
            if (filtro == 1)
            {
                data = data.Where(c => c.DT_PUBLICACAO.Month == DateTime.Now.Month && c.DT_PUBLICACAO.Year == DateTime.Now.Year).ToList();
            }
            if (filtro == 3)
            {
                data = data.Where(c => c.DT_PUBLICACAO.Year == DateTime.Now.Year).ToList();
            }

            if(ID_DEVICE != 0)
            {
                data = data.Where( c=> c.ID_DEVICE == ID_DEVICE.ToString()).ToList();
            }
            return data;
        }
    }
}