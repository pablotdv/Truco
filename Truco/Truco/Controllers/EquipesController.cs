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

    public class EquipesController : Controller
    {
        //
        // GET: /Equipes/
        public async Task<ActionResult> Indice()
        {
            var viewModel = JsonConvert.DeserializeObject<EquipesViewModel>(await PesquisaModelStore.GetAsync(PesquisaKey));

            return await Pesquisa(viewModel ?? new EquipesViewModel());
        }

        //
        // GET: /Equipes/Pesquisa
        public async Task<ActionResult> Pesquisa(EquipesViewModel viewModel)
        {
            await PesquisaModelStore.AddAsync(PesquisaKey, viewModel);

            var query = db.Equipes.AsQueryable();

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
        // GET: /Equipes/Detalhes/5
        public async Task<ActionResult> Detalhes(System.Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipe equipe = await db.Equipes.FindAsync(id);
            if (equipe == null)
            {
                return HttpNotFound();
            }

            await ViewBags();
            return View(equipe);
        }

        //
        // GET: /Equipes/Criar        

        public async Task<ActionResult> Criar()
        {
            await ViewBags();
            return View();
        }

        //
        // POST: /Equipes/Criar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Criar(Equipe equipe)
        {
            if (ModelState.IsValid)
            {
                equipe.EquipeId = Guid.NewGuid();
                db.Equipes.Add(equipe);
                await db.SaveChangesAsync();
                TempData["Mensagem"] = "Operação realizada com sucesso!";
                return RedirectToAction("Indice");
            }


            await ViewBags();
            return View(equipe);
        }

        //
        // GET: /Equipes/Editar/5 
        public async Task<ActionResult> Editar(System.Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipe equipe = await db.Equipes.FindAsync(id);
            if (equipe == null)
            {
                return HttpNotFound();
            }

            await ViewBags();
            return View(equipe);
        }

        //
        // POST: /Equipes/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(Equipe equipe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipe).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["Mensagem"] = "Alteração realizada com sucesso!";
                return RedirectToAction("Indice");
            }


            await ViewBags();
            return View(equipe);
        }

        //
        // GET: /Equipes/Excluir/5 
        public async Task<ActionResult> Excluir(System.Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipe equipe = await db.Equipes.FindAsync(id);
            if (equipe == null)
            {
                return HttpNotFound();
            }


            await ViewBags();

            return View(equipe);
        }

        //
        // POST: /Equipes/Excluir/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExcluirConfirmacao(System.Guid id)
        {
            Equipe equipe = await db.Equipes.FindAsync(id);
            db.Equipes.Remove(equipe);
            await db.SaveChangesAsync();
            return RedirectToAction("Indice");
        }
        private async Task ViewBags()
        {
            ViewBag.Regiaos = new SelectList(await db.Regioes.ToListAsync(), "RegiaoId", "Numero");

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InserirAtleta(EquipeAtleta model)
        {

            if (ModelState.IsValid)
            {
                var equipe = db.Equipes
                    .Include(a => a.Atletas)
                    .FirstOrDefault(a => a.EquipeId == model.EquipeId);

                var atleta = db.Atletas
                    .Find(model.AtletaId);

                equipe.Atletas.Add(new EquipeAtleta() { EquipeAtletaId = Guid.NewGuid(), AtletaId = atleta.AtletaId });
                db.SaveChanges();

                var dados = equipe.Atletas
                .Select(a => new EquipeAtleta()
                {
                    AtletaId = a.AtletaId,
                    EquipeId = equipe.EquipeId,
                    EquipeAtletaId = a.EquipeAtletaId,

                }).ToList();

                return PartialView("_Atletas", dados);
            }

            return View();

        }


        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public async Task<JsonResult> PesquisarAtletas(string nome)
        {
            var nomes = String.IsNullOrEmpty(nome) ? new string[0] : nome.Split(' ');
            var dados = await db.Atletas.Where(b => nomes.All(c => b.Nome.Contains(c)))
                .Select(a => new
                {
                    Id = a.AtletaId,
                    value = a.Nome,

                }).ToListAsync();

            return Json(dados, JsonRequestBehavior.AllowGet);

        }

    }
}
