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

    public class EquipesAtletasController : Controller
    {	        
        //
        // GET: /EquipesAtletas/
        public async Task<ActionResult> Indice()
        {
			var viewModel = JsonConvert.DeserializeObject<EquipesAtletasViewModel>(await PesquisaModelStore.GetAsync(PesquisaKey));

            return await Pesquisa(viewModel ?? new EquipesAtletasViewModel());
        }

		//
        // GET: /EquipesAtletas/Pesquisa
		public async Task<ActionResult> Pesquisa(EquipesAtletasViewModel viewModel)
		{
			await PesquisaModelStore.AddAsync(PesquisaKey, viewModel);

			var query = db.EquipesAtletas.AsQueryable();

			//TODO: parâmetros de pesquisa
			if (!String.IsNullOrWhiteSpace(viewModel.Atleta))
            {
                var atletas = viewModel.Atleta?.Split(' ');
                query = query.Where(a => atletas.All(atleta => a.Atleta.Nome.Contains(atleta)));
            }

            viewModel.Resultados = await query.OrderBy(a => a.Atleta).ToPagedListAsync(viewModel.Pagina, viewModel.TamanhoPagina);

            if (Request.IsAjaxRequest())
                return PartialView("_Pesquisa", viewModel);

            return View("Indice", viewModel);
		}

        //
        // GET: /EquipesAtletas/Detalhes/5
        public async Task<ActionResult> Detalhes(System.Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipeAtleta equipeAtleta = await db.EquipesAtletas.FindAsync(id);
            if (equipeAtleta == null)
            {
                return HttpNotFound();
            }  
          
			await ViewBags();
            return View(equipeAtleta);
        }

        //
        // GET: /EquipesAtletas/Criar        
          
		public async Task<ActionResult> Criar()
        {
			await ViewBags();
            return View();
        } 

        //
        // POST: /EquipesAtletas/Criar
        [HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> Criar(EquipeAtleta equipeAtleta)
        {
            if (ModelState.IsValid)
            {
                equipeAtleta.EquipeAtletaId = Guid.NewGuid();
                db.EquipesAtletas.Add(equipeAtleta);
                await db.SaveChangesAsync();
				TempData["Mensagem"] = "Operação realizada com sucesso!";
                return RedirectToAction("Indice");  
            }

          
			await ViewBags();
            return View(equipeAtleta);
        }
        
        //
        // GET: /EquipesAtletas/Editar/5 
        public async Task<ActionResult> Editar(System.Guid? id)
        {
			if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipeAtleta equipeAtleta = await db.EquipesAtletas.FindAsync(id);
            if (equipeAtleta == null)
            {
                return HttpNotFound();
            }            
          
			await ViewBags();
            return View(equipeAtleta);
        }

        //
        // POST: /EquipesAtletas/Editar/5
        [HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(EquipeAtleta equipeAtleta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipeAtleta).State = EntityState.Modified;
                await db.SaveChangesAsync();
				TempData["Mensagem"] = "Alteração realizada com sucesso!";
                return RedirectToAction("Indice");
            }

          
			await ViewBags();
            return View(equipeAtleta);
        }

        //
        // GET: /EquipesAtletas/Excluir/5 
        public async Task<ActionResult> Excluir(System.Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipeAtleta equipeAtleta = await db.EquipesAtletas.FindAsync(id);
            if (equipeAtleta == null)
            {
                return HttpNotFound();
            }   

          
			await ViewBags();
  
            return View(equipeAtleta);
        }

        //
        // POST: /EquipesAtletas/Excluir/5
        [HttpPost, ActionName("Excluir")]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> ExcluirConfirmacao(System.Guid id)
        {
            EquipeAtleta equipeAtleta = await db.EquipesAtletas.FindAsync(id);
            db.EquipesAtletas.Remove(equipeAtleta);
            await db.SaveChangesAsync();
            return RedirectToAction("Indice");
        }
		private async Task ViewBags()
		{
            ViewBag.Equipes = new SelectList(await db.Equipes.ToListAsync(), "EquipeId", "Nome");
            ViewBag.Atletas = new SelectList(await db.Atletas.ToListAsync(), "AtletaId", "Nome");
    
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
