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

    public class CidadesController : Controller
    {	        
        //
        // GET: /Cidades/
        public async Task<ActionResult> Indice()
        {
			var viewModel = JsonConvert.DeserializeObject<CidadesViewModel>(await PesquisaModelStore.GetAsync(PesquisaKey));

            return await Pesquisa(viewModel ?? new CidadesViewModel());
        }

		//
        // GET: /Cidades/Pesquisa
		public async Task<ActionResult> Pesquisa(CidadesViewModel viewModel)
		{
			await PesquisaModelStore.AddAsync(PesquisaKey, viewModel);

			var query = db.Cidades.AsQueryable();

			//TODO: parâmetros de pesquisa
			if (!String.IsNullOrWhiteSpace(viewModel.Nome))
            {
                var nomes = viewModel.Nome?.Split(' ');
                query = query.Where(a => nomes.All(nome => a.Nome.Contains(nome)));
            }

            viewModel.Resultados = await query.OrderBy(a => a.Nome).ToPagedListAsync(viewModel.Pagina, viewModel.TamanhoPagina);

            if (Request.IsAjaxRequest())
                return PartialView("_Pesquisa", viewModel);

            return View("Indice", viewModel);
		}

        //
        // GET: /Cidades/Detalhes/5
        public async Task<ActionResult> Detalhes(System.Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cidade cidade = await db.Cidades.FindAsync(id);
            if (cidade == null)
            {
                return HttpNotFound();
            }  
          
			await ViewBags();
            return View(cidade);
        }

        //
        // GET: /Cidades/Criar        
          
		public async Task<ActionResult> Criar()
        {
			await ViewBags();
            return View();
        } 

        //
        // POST: /Cidades/Criar
        [HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> Criar(Cidade cidade)
        {
            if (ModelState.IsValid)
            {
                cidade.CidadeId = Guid.NewGuid();
                db.Cidades.Add(cidade);
                await db.SaveChangesAsync();
				TempData["Mensagem"] = "Operação realizada com sucesso!";
                return RedirectToAction("Indice");  
            }

          
			await ViewBags();
            return View(cidade);
        }
        
        //
        // GET: /Cidades/Editar/5 
        public async Task<ActionResult> Editar(System.Guid? id)
        {
			if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cidade cidade = await db.Cidades.FindAsync(id);
            if (cidade == null)
            {
                return HttpNotFound();
            }            
          
			await ViewBags();
            return View(cidade);
        }

        //
        // POST: /Cidades/Editar/5
        [HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(Cidade cidade)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cidade).State = EntityState.Modified;
                await db.SaveChangesAsync();
				TempData["Mensagem"] = "Alteração realizada com sucesso!";
                return RedirectToAction("Indice");
            }

          
			await ViewBags();
            return View(cidade);
        }

        //
        // GET: /Cidades/Excluir/5 
        public async Task<ActionResult> Excluir(System.Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cidade cidade = await db.Cidades.FindAsync(id);
            if (cidade == null)
            {
                return HttpNotFound();
            }   

          
			await ViewBags();
  
            return View(cidade);
        }

        //
        // POST: /Cidades/Excluir/5
        [HttpPost, ActionName("Excluir")]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> ExcluirConfirmacao(System.Guid id)
        {
            Cidade cidade = await db.Cidades.FindAsync(id);
            db.Cidades.Remove(cidade);
            await db.SaveChangesAsync();
            return RedirectToAction("Indice");
        }
		private async Task ViewBags()
		{
            ViewBag.Estadoes = new SelectList(await db.Estados.ToListAsync(), "EstadoId", "Nome");
    
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
