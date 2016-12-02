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

    public class CompeticoesEquipesController : Controller
    {	        
        //
        // GET: /CompeticoesEquipes/
        public async Task<ActionResult> Indice()
        {
			var viewModel = JsonConvert.DeserializeObject<CompeticoesEquipesViewModel>(await PesquisaModelStore.GetAsync(PesquisaKey));

            return await Pesquisa(viewModel ?? new CompeticoesEquipesViewModel());
        }

		//
        // GET: /CompeticoesEquipes/Pesquisa
		public async Task<ActionResult> Pesquisa(CompeticoesEquipesViewModel viewModel)
		{
			await PesquisaModelStore.AddAsync(PesquisaKey, viewModel);

			var query = db.CompeticoesEquipes.AsQueryable();

			//TODO: parâmetros de pesquisa
			if (!String.IsNullOrWhiteSpace(viewModel.Equipe))
            {
                var equipes = viewModel.Equipe?.Split(' ');
                query = query.Where(a => equipes.All(equipe => a.Nome.Contains(equipe)));
            }

            viewModel.Resultados = await query.OrderBy(a => a.Nome).ToPagedListAsync(viewModel.Pagina, viewModel.TamanhoPagina);

            if (Request.IsAjaxRequest())
                return PartialView("_Pesquisa", viewModel);

            return View("Indice", viewModel);
		}

        //
        // GET: /CompeticoesEquipes/Detalhes/5
        public async Task<ActionResult> Detalhes(System.Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompeticaoEquipe competicaoEquipe = await db.CompeticoesEquipes.FindAsync(id);
            if (competicaoEquipe == null)
            {
                return HttpNotFound();
            }  
          
			await ViewBags();
            return View(competicaoEquipe);
        }

        //
        // GET: /CompeticoesEquipes/Criar        
          
		public async Task<ActionResult> Criar()
        {
			await ViewBags();
            return View();
        } 

        //
        // POST: /CompeticoesEquipes/Criar
        [HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> Criar(CompeticaoEquipe competicaoEquipe)
        {
            if (ModelState.IsValid)
            {
                competicaoEquipe.CompeticaoEquipeId = Guid.NewGuid();
                db.CompeticoesEquipes.Add(competicaoEquipe);
                await db.SaveChangesAsync();
				TempData["Mensagem"] = "Operação realizada com sucesso!";
                return RedirectToAction("Indice");  
            }

          
			await ViewBags();
            return View(competicaoEquipe);
        }
        
        //
        // GET: /CompeticoesEquipes/Editar/5 
        public async Task<ActionResult> Editar(System.Guid? id)
        {
			if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompeticaoEquipe competicaoEquipe = await db.CompeticoesEquipes.FindAsync(id);
            if (competicaoEquipe == null)
            {
                return HttpNotFound();
            }            
          
			await ViewBags();
            return View(competicaoEquipe);
        }

        //
        // POST: /CompeticoesEquipes/Editar/5
        [HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(CompeticaoEquipe competicaoEquipe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(competicaoEquipe).State = EntityState.Modified;
                await db.SaveChangesAsync();
				TempData["Mensagem"] = "Alteração realizada com sucesso!";
                return RedirectToAction("Indice");
            }

          
			await ViewBags();
            return View(competicaoEquipe);
        }

        //
        // GET: /CompeticoesEquipes/Excluir/5 
        public async Task<ActionResult> Excluir(System.Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompeticaoEquipe competicaoEquipe = await db.CompeticoesEquipes.FindAsync(id);
            if (competicaoEquipe == null)
            {
                return HttpNotFound();
            }   

          
			await ViewBags();
  
            return View(competicaoEquipe);
        }

        //
        // POST: /CompeticoesEquipes/Excluir/5
        [HttpPost, ActionName("Excluir")]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> ExcluirConfirmacao(System.Guid id)
        {
            CompeticaoEquipe competicaoEquipe = await db.CompeticoesEquipes.FindAsync(id);
            db.CompeticoesEquipes.Remove(competicaoEquipe);
            await db.SaveChangesAsync();
            return RedirectToAction("Indice");
        }
		private async Task ViewBags()
		{
            ViewBag.Competicaos = new SelectList(await db.Competicoes.ToListAsync(), "CompeticaoId", "Nome");
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
