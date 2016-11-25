using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using PagedList.EntityFramework;
using Newtonsoft.Json;
using System.Net;
using Truco.Infraestrutura.Helpers;
using Truco.ViewModels;
using Truco.Models;

namespace Truco.Controllers
{   

    public class RegioesController : Controller
    {	        
        //
        // GET: /Regioes/
        public async Task<ActionResult> Indice()
        {
			var viewModel = JsonConvert.DeserializeObject<RegioesViewModel>(await PesquisaModelStore.GetAsync(PesquisaKey));

            return await Pesquisa(viewModel ?? new RegioesViewModel());
        }

		//
        // GET: /Regioes/Pesquisa
		public async Task<ActionResult> Pesquisa(RegioesViewModel viewModel)
		{
			await PesquisaModelStore.AddAsync(PesquisaKey, viewModel);

			var query = db.Regiaos.AsQueryable();

			//TODO: parâmetros de pesquisa
			if (!String.IsNullOrWhiteSpace(viewModel.Numero))
            {
                var numeros = viewModel.Numero?.Split(' ');
                //query = query.Where(a => numeros.All(numero => a.Numero.Contains(numero)));
            }

            viewModel.Resultados = await query.OrderBy(a => a.Numero).ToPagedListAsync(viewModel.Pagina, viewModel.TamanhoPagina);

            if (Request.IsAjaxRequest())
                return PartialView("_Pesquisa", viewModel);

            return View("Indice", viewModel);
		}

        //
        // GET: /Regioes/Detalhes/5
        public async Task<ActionResult> Detalhes(System.Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Regiao regiao = await db.Regiaos.FindAsync(id);
            if (regiao == null)
            {
                return HttpNotFound();
            }  
            return View(regiao);
        }

        //
        // GET: /Regioes/Criar        
		public ActionResult Criar()
        {
            return View();
        } 

        //
        // POST: /Regioes/Criar
        [HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> Criar(Regiao regiao)
        {
            if (ModelState.IsValid)
            {
                regiao.RegiaoId = Guid.NewGuid();
                db.Regiaos.Add(regiao);
                await db.SaveChangesAsync();
				TempData["Mensagem"] = "Operação realizada com sucesso!";
                return RedirectToAction("Indice");  
            }

            return View(regiao);
        }
        
        //
        // GET: /Regioes/Editar/5 
        public async Task<ActionResult> Editar(System.Guid? id)
        {
			if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Regiao regiao = await db.Regiaos.FindAsync(id);
            if (regiao == null)
            {
                return HttpNotFound();
            }            
            return View(regiao);
        }

        //
        // POST: /Regioes/Editar/5
        [HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(Regiao regiao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(regiao).State = EntityState.Modified;
                await db.SaveChangesAsync();
				TempData["Mensagem"] = "Alteração realizada com sucesso!";
                return RedirectToAction("Indice");
            }

            return View(regiao);
        }

        //
        // GET: /Regioes/Excluir/5 
        public async Task<ActionResult> Excluir(System.Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Regiao regiao = await db.Regiaos.FindAsync(id);
            if (regiao == null)
            {
                return HttpNotFound();
            }   

  
            return View(regiao);
        }

        //
        // POST: /Regioes/Excluir/5
        [HttpPost, ActionName("Excluir")]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> ExcluirConfirmacao(System.Guid id)
        {
            Regiao regiao = await db.Regiaos.FindAsync(id);
            db.Regiaos.Remove(regiao);
            await db.SaveChangesAsync();
            return RedirectToAction("Indice");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
