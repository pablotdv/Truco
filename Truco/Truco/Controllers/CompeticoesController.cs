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

    public class CompeticoesController : Controller
    {	        
        //
        // GET: /Competicoes/
        public async Task<ActionResult> Indice()
        {
			var viewModel = JsonConvert.DeserializeObject<CompeticoesViewModel>(await PesquisaModelStore.GetAsync(PesquisaKey));

            return await Pesquisa(viewModel ?? new CompeticoesViewModel());
        }

		//
        // GET: /Competicoes/Pesquisa
		public async Task<ActionResult> Pesquisa(CompeticoesViewModel viewModel)
		{
			await PesquisaModelStore.AddAsync(PesquisaKey, viewModel);

			var query = db.Competicoes.AsQueryable();

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
        // GET: /Competicoes/Detalhes/5
        public async Task<ActionResult> Detalhes(System.Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Competicao competicao = await db.Competicoes.FindAsync(id);
            if (competicao == null)
            {
                return HttpNotFound();
            }  
            return View(competicao);
        }

        //
        // GET: /Competicoes/Criar        
		public ActionResult Criar()
        {
            return View();
        } 

        //
        // POST: /Competicoes/Criar
        [HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> Criar(Competicao competicao)
        {
            if (ModelState.IsValid)
            {
                competicao.CompeticaoId = Guid.NewGuid();
                db.Competicoes.Add(competicao);
                await db.SaveChangesAsync();
				TempData["Mensagem"] = "Operação realizada com sucesso!";
                return RedirectToAction("Indice");  
            }

            return View(competicao);
        }
        
        //
        // GET: /Competicoes/Editar/5 
        public async Task<ActionResult> Editar(System.Guid? id)
        {
			if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Competicao competicao = await db.Competicoes.FindAsync(id);
            if (competicao == null)
            {
                return HttpNotFound();
            }            
            return View(competicao);
        }

        //
        // POST: /Competicoes/Editar/5
        [HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(Competicao competicao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(competicao).State = EntityState.Modified;
                await db.SaveChangesAsync();
				TempData["Mensagem"] = "Alteração realizada com sucesso!";
                return RedirectToAction("Indice");
            }

            return View(competicao);
        }

        //
        // GET: /Competicoes/Excluir/5 
        public async Task<ActionResult> Excluir(System.Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Competicao competicao = await db.Competicoes.FindAsync(id);
            if (competicao == null)
            {
                return HttpNotFound();
            }   

  
            return View(competicao);
        }

        //
        // POST: /Competicoes/Excluir/5
        [HttpPost, ActionName("Excluir")]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> ExcluirConfirmacao(System.Guid id)
        {
            Competicao competicao = await db.Competicoes.FindAsync(id);
            db.Competicoes.Remove(competicao);
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
