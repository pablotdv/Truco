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

    public class AtletasController : Controller
    {	        
        //
        // GET: /Atletas/
        public async Task<ActionResult> Indice()
        {
			var viewModel = JsonConvert.DeserializeObject<AtletasViewModel>(await PesquisaModelStore.GetAsync(PesquisaKey));

            return await Pesquisa(viewModel ?? new AtletasViewModel());
        }

		//
        // GET: /Atletas/Pesquisa
		public async Task<ActionResult> Pesquisa(AtletasViewModel viewModel)
		{
			await PesquisaModelStore.AddAsync(PesquisaKey, viewModel);

			var query = db.Atletas.AsQueryable();

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
        // GET: /Atletas/Detalhes/5
        public async Task<ActionResult> Detalhes(System.Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Atleta atleta = await db.Atletas.FindAsync(id);
            if (atleta == null)
            {
                return HttpNotFound();
            }  
            return View(atleta);
        }

        //
        // GET: /Atletas/Criar        
		public ActionResult Criar()
        {
            return View();
        } 

        //
        // POST: /Atletas/Criar
        [HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> Criar(Atleta atleta)
        {
            if (ModelState.IsValid)
            {
                atleta.AtletaId = Guid.NewGuid();
                db.Atletas.Add(atleta);
                await db.SaveChangesAsync();
				TempData["Mensagem"] = "Operação realizada com sucesso!";
                return RedirectToAction("Indice");  
            }

            return View(atleta);
        }
        
        //
        // GET: /Atletas/Editar/5 
        public async Task<ActionResult> Editar(System.Guid? id)
        {
			if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Atleta atleta = await db.Atletas.FindAsync(id);
            if (atleta == null)
            {
                return HttpNotFound();
            }            
            return View(atleta);
        }

        //
        // POST: /Atletas/Editar/5
        [HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(Atleta atleta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(atleta).State = EntityState.Modified;
                await db.SaveChangesAsync();
				TempData["Mensagem"] = "Alteração realizada com sucesso!";
                return RedirectToAction("Indice");
            }

            return View(atleta);
        }

        //
        // GET: /Atletas/Excluir/5 
        public async Task<ActionResult> Excluir(System.Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Atleta atleta = await db.Atletas.FindAsync(id);
            if (atleta == null)
            {
                return HttpNotFound();
            }   

  
            return View(atleta);
        }

        //
        // POST: /Atletas/Excluir/5
        [HttpPost, ActionName("Excluir")]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> ExcluirConfirmacao(System.Guid id)
        {
            Atleta atleta = await db.Atletas.FindAsync(id);
            db.Atletas.Remove(atleta);
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
