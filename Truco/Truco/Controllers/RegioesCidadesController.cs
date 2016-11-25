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

    public class RegioesCidadesController : Controller
    {	        
        //
        // GET: /RegioesCidades/
        public async Task<ActionResult> Indice()
        {
			var viewModel = JsonConvert.DeserializeObject<RegioesCidadesViewModel>(await PesquisaModelStore.GetAsync(PesquisaKey));

            return await Pesquisa(viewModel ?? new RegioesCidadesViewModel());
        }

		//
        // GET: /RegioesCidades/Pesquisa
		public async Task<ActionResult> Pesquisa(RegioesCidadesViewModel viewModel)
		{
			await PesquisaModelStore.AddAsync(PesquisaKey, viewModel);

			var query = db.RegiaoCidades.AsQueryable();

			//TODO: parâmetros de pesquisa
			if (!String.IsNullOrWhiteSpace(viewModel.Cidade))
            {
                var cidades = viewModel.Cidade?.Split(' ');
                query = query.Where(a => cidades.All(cidade => a.Cidade.Nome.Contains(cidade)));
            }

            viewModel.Resultados = await query.OrderBy(a => a.Cidade.Nome).ToPagedListAsync(viewModel.Pagina, viewModel.TamanhoPagina);

            if (Request.IsAjaxRequest())
                return PartialView("_Pesquisa", viewModel);

            return View("Indice", viewModel);
		}

        //
        // GET: /RegioesCidades/Detalhes/5
        public async Task<ActionResult> Detalhes(System.Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegiaoCidade regiaoCidade = await db.RegiaoCidades.FindAsync(id);
            if (regiaoCidade == null)
            {
                return HttpNotFound();
            }  
          
			await ViewBags();
            return View(regiaoCidade);
        }

        //
        // GET: /RegioesCidades/Criar        
          
		public async Task<ActionResult> Criar()
        {
			await ViewBags();
            return View();
        } 

        //
        // POST: /RegioesCidades/Criar
        [HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> Criar(RegiaoCidade regiaoCidade)
        {
            if (ModelState.IsValid)
            {
                regiaoCidade.RegiaoCidadeId = Guid.NewGuid();
                db.RegiaoCidades.Add(regiaoCidade);
                await db.SaveChangesAsync();
				TempData["Mensagem"] = "Operação realizada com sucesso!";
                return RedirectToAction("Indice");  
            }

          
			await ViewBags();
            return View(regiaoCidade);
        }
        
        //
        // GET: /RegioesCidades/Editar/5 
        public async Task<ActionResult> Editar(System.Guid? id)
        {
			if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegiaoCidade regiaoCidade = await db.RegiaoCidades.FindAsync(id);
            if (regiaoCidade == null)
            {
                return HttpNotFound();
            }            
          
			await ViewBags();
            return View(regiaoCidade);
        }

        //
        // POST: /RegioesCidades/Editar/5
        [HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(RegiaoCidade regiaoCidade)
        {
            if (ModelState.IsValid)
            {
                db.Entry(regiaoCidade).State = EntityState.Modified;
                await db.SaveChangesAsync();
				TempData["Mensagem"] = "Alteração realizada com sucesso!";
                return RedirectToAction("Indice");
            }

          
			await ViewBags();
            return View(regiaoCidade);
        }

        //
        // GET: /RegioesCidades/Excluir/5 
        public async Task<ActionResult> Excluir(System.Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegiaoCidade regiaoCidade = await db.RegiaoCidades.FindAsync(id);
            if (regiaoCidade == null)
            {
                return HttpNotFound();
            }   

          
			await ViewBags();
  
            return View(regiaoCidade);
        }

        //
        // POST: /RegioesCidades/Excluir/5
        [HttpPost, ActionName("Excluir")]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> ExcluirConfirmacao(System.Guid id)
        {
            RegiaoCidade regiaoCidade = await db.RegiaoCidades.FindAsync(id);
            db.RegiaoCidades.Remove(regiaoCidade);
            await db.SaveChangesAsync();
            return RedirectToAction("Indice");
        }
		private async Task ViewBags()
		{
            ViewBag.Regiaos = new SelectList(await db.Regiaos.ToListAsync(), "RegiaoId", "Numero");
            ViewBag.Cidades = new SelectList(await db.Cidades.ToListAsync(), "CidadeId", "Nome");
    
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
