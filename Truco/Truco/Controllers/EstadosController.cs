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

    public class EstadosController : Controller
    {	        
        //
        // GET: /Estados/
        public async Task<ActionResult> Indice()
        {
			var viewModel = JsonConvert.DeserializeObject<EstadosViewModel>(await PesquisaModelStore.GetAsync(PesquisaKey));

            return await Pesquisa(viewModel ?? new EstadosViewModel());
        }

		//
        // GET: /Estados/Pesquisa
		public async Task<ActionResult> Pesquisa(EstadosViewModel viewModel)
		{
			await PesquisaModelStore.AddAsync(PesquisaKey, viewModel);

			var query = db.Estados.AsQueryable();

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
        // GET: /Estados/Detalhes/5
        public async Task<ActionResult> Detalhes(System.Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estado estado = await db.Estados.FindAsync(id);
            if (estado == null)
            {
                return HttpNotFound();
            }  
          
			await ViewBags();
            return View(estado);
        }

        //
        // GET: /Estados/Criar        
          
		public async Task<ActionResult> Criar()
        {
			await ViewBags();
            return View();
        } 

        //
        // POST: /Estados/Criar
        [HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> Criar(Estado estado)
        {
            if (ModelState.IsValid)
            {
                estado.EstadoId = Guid.NewGuid();
                db.Estados.Add(estado);
                await db.SaveChangesAsync();
				TempData["Mensagem"] = "Operação realizada com sucesso!";
                return RedirectToAction("Indice");  
            }

          
			await ViewBags();
            return View(estado);
        }
        
        //
        // GET: /Estados/Editar/5 
        public async Task<ActionResult> Editar(System.Guid? id)
        {
			if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estado estado = await db.Estados.FindAsync(id);
            if (estado == null)
            {
                return HttpNotFound();
            }            
          
			await ViewBags();
            return View(estado);
        }

        //
        // POST: /Estados/Editar/5
        [HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(Estado estado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estado).State = EntityState.Modified;
                await db.SaveChangesAsync();
				TempData["Mensagem"] = "Alteração realizada com sucesso!";
                return RedirectToAction("Indice");
            }

          
			await ViewBags();
            return View(estado);
        }

        //
        // GET: /Estados/Excluir/5 
        public async Task<ActionResult> Excluir(System.Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estado estado = await db.Estados.FindAsync(id);
            if (estado == null)
            {
                return HttpNotFound();
            }   

          
			await ViewBags();
  
            return View(estado);
        }

        //
        // POST: /Estados/Excluir/5
        [HttpPost, ActionName("Excluir")]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> ExcluirConfirmacao(System.Guid id)
        {
            Estado estado = await db.Estados.FindAsync(id);
            db.Estados.Remove(estado);
            await db.SaveChangesAsync();
            return RedirectToAction("Indice");
        }
		private async Task ViewBags()
		{
            ViewBag.Pais = new SelectList(await db.Paises.ToListAsync(), "PaisId", "Nome");
    
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
