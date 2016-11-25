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

    public class LogradourosController : Controller
    {	        
        //
        // GET: /Logradouros/
        public async Task<ActionResult> Indice()
        {
			var viewModel = JsonConvert.DeserializeObject<LogradourosViewModel>(await PesquisaModelStore.GetAsync(PesquisaKey));

            return await Pesquisa(viewModel ?? new LogradourosViewModel());
        }

		//
        // GET: /Logradouros/Pesquisa
		public async Task<ActionResult> Pesquisa(LogradourosViewModel viewModel)
		{
			await PesquisaModelStore.AddAsync(PesquisaKey, viewModel);

			var query = db.Logradouroes.AsQueryable();

			//TODO: parâmetros de pesquisa
			if (!String.IsNullOrWhiteSpace(viewModel.Descricao))
            {
                var descricaos = viewModel.Descricao?.Split(' ');
                query = query.Where(a => descricaos.All(descricao => a.Descricao.Contains(descricao)));
            }

            viewModel.Resultados = await query.OrderBy(a => a.Descricao).ToPagedListAsync(viewModel.Pagina, viewModel.TamanhoPagina);

            if (Request.IsAjaxRequest())
                return PartialView("_Pesquisa", viewModel);

            return View("Indice", viewModel);
		}

        //
        // GET: /Logradouros/Detalhes/5
        public async Task<ActionResult> Detalhes(System.Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Logradouro logradouro = await db.Logradouroes.FindAsync(id);
            if (logradouro == null)
            {
                return HttpNotFound();
            }  
          
			await ViewBags();
            return View(logradouro);
        }

        //
        // GET: /Logradouros/Criar        
          
		public async Task<ActionResult> Criar()
        {
			await ViewBags();
            return View();
        } 

        //
        // POST: /Logradouros/Criar
        [HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> Criar(Logradouro logradouro)
        {
            if (ModelState.IsValid)
            {
                logradouro.LogradouroId = Guid.NewGuid();
                db.Logradouroes.Add(logradouro);
                await db.SaveChangesAsync();
				TempData["Mensagem"] = "Operação realizada com sucesso!";
                return RedirectToAction("Indice");  
            }

          
			await ViewBags();
            return View(logradouro);
        }
        
        //
        // GET: /Logradouros/Editar/5 
        public async Task<ActionResult> Editar(System.Guid? id)
        {
			if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Logradouro logradouro = await db.Logradouroes.FindAsync(id);
            if (logradouro == null)
            {
                return HttpNotFound();
            }            
          
			await ViewBags();
            return View(logradouro);
        }

        //
        // POST: /Logradouros/Editar/5
        [HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(Logradouro logradouro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(logradouro).State = EntityState.Modified;
                await db.SaveChangesAsync();
				TempData["Mensagem"] = "Alteração realizada com sucesso!";
                return RedirectToAction("Indice");
            }

          
			await ViewBags();
            return View(logradouro);
        }

        //
        // GET: /Logradouros/Excluir/5 
        public async Task<ActionResult> Excluir(System.Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Logradouro logradouro = await db.Logradouroes.FindAsync(id);
            if (logradouro == null)
            {
                return HttpNotFound();
            }   

          
			await ViewBags();
  
            return View(logradouro);
        }

        //
        // POST: /Logradouros/Excluir/5
        [HttpPost, ActionName("Excluir")]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> ExcluirConfirmacao(System.Guid id)
        {
            Logradouro logradouro = await db.Logradouroes.FindAsync(id);
            db.Logradouroes.Remove(logradouro);
            await db.SaveChangesAsync();
            return RedirectToAction("Indice");
        }
		private async Task ViewBags()
		{
            ViewBag.Bairroes = new SelectList(await db.Bairroes.ToListAsync(), "BairroId", "Nome");
    
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
