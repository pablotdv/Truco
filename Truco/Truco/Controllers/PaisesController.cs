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

    public class PaisesController : Controller
    {	        
        //
        // GET: /Paises/
        public async Task<ActionResult> Indice()
        {
			var viewModel = JsonConvert.DeserializeObject<PaisesViewModel>(await PesquisaModelStore.GetAsync(PesquisaKey));

            return await Pesquisa(viewModel ?? new PaisesViewModel());
        }

		//
        // GET: /Paises/Pesquisa
		public async Task<ActionResult> Pesquisa(PaisesViewModel viewModel)
		{
			await PesquisaModelStore.AddAsync(PesquisaKey, viewModel);

			var query = db.Paises.AsQueryable();

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
        // GET: /Paises/Detalhes/5
        public async Task<ActionResult> Detalhes(System.Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pais pais = await db.Paises.FindAsync(id);
            if (pais == null)
            {
                return HttpNotFound();
            }  
            return View(pais);
        }

        //
        // GET: /Paises/Criar        
		public ActionResult Criar()
        {
            return View();
        } 

        //
        // POST: /Paises/Criar
        [HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> Criar(Pais pais)
        {
            if (ModelState.IsValid)
            {
                pais.PaisId = Guid.NewGuid();
                db.Paises.Add(pais);
                await db.SaveChangesAsync();
				TempData["Mensagem"] = "Operação realizada com sucesso!";
                return RedirectToAction("Indice");  
            }

            return View(pais);
        }
        
        //
        // GET: /Paises/Editar/5 
        public async Task<ActionResult> Editar(System.Guid? id)
        {
			if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pais pais = await db.Paises.FindAsync(id);
            if (pais == null)
            {
                return HttpNotFound();
            }            
            return View(pais);
        }

        //
        // POST: /Paises/Editar/5
        [HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(Pais pais)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pais).State = EntityState.Modified;
                await db.SaveChangesAsync();
				TempData["Mensagem"] = "Alteração realizada com sucesso!";
                return RedirectToAction("Indice");
            }

            return View(pais);
        }

        //
        // GET: /Paises/Excluir/5 
        public async Task<ActionResult> Excluir(System.Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pais pais = await db.Paises.FindAsync(id);
            if (pais == null)
            {
                return HttpNotFound();
            }   

  
            return View(pais);
        }

        //
        // POST: /Paises/Excluir/5
        [HttpPost, ActionName("Excluir")]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> ExcluirConfirmacao(System.Guid id)
        {
            Pais pais = await db.Paises.FindAsync(id);
            db.Paises.Remove(pais);
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
