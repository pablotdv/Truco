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

    public class CompeticoesFasesGruposController : Controller
    {
        //
        // GET: /CompeticoesFasesGrupos/
        public async Task<ActionResult> Indice()
        {
            var viewModel = JsonConvert.DeserializeObject<CompeticoesFasesGruposViewModel>(await PesquisaModelStore.GetAsync(PesquisaKey));

            return await Pesquisa(viewModel ?? new CompeticoesFasesGruposViewModel());
        }

        //
        // GET: /CompeticoesFasesGrupos/Pesquisa
        public async Task<ActionResult> Pesquisa(CompeticoesFasesGruposViewModel viewModel)
        {
            await PesquisaModelStore.AddAsync(PesquisaKey, viewModel);

            var query = db.CompeticoesFasesGrupos.AsQueryable();

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
        // GET: /CompeticoesFasesGrupos/Detalhes/5
        public async Task<ActionResult> Detalhes(System.Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompeticaoFaseGrupo competicaoFaseGrupo = await db.CompeticoesFasesGrupos.FindAsync(id);
            if (competicaoFaseGrupo == null)
            {
                return HttpNotFound();
            }

            await ViewBags();
            return View(competicaoFaseGrupo);
        }

        //
        // GET: /CompeticoesFasesGrupos/Criar        

        public async Task<ActionResult> Criar()
        {
            await ViewBags();
            return View();
        }

        //
        // POST: /CompeticoesFasesGrupos/Criar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Criar(CompeticaoFaseGrupo competicaoFaseGrupo)
        {
            if (ModelState.IsValid)
            {
                competicaoFaseGrupo.CompeticaoFaseGrupoId = Guid.NewGuid();
                db.CompeticoesFasesGrupos.Add(competicaoFaseGrupo);
                await db.SaveChangesAsync();
                TempData["Mensagem"] = "Operação realizada com sucesso!";
                return RedirectToAction("Indice");
            }


            await ViewBags();
            return View(competicaoFaseGrupo);
        }

        //
        // GET: /CompeticoesFasesGrupos/Editar/5 
        public async Task<ActionResult> Editar(System.Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompeticaoFaseGrupo competicaoFaseGrupo = await db.CompeticoesFasesGrupos.FindAsync(id);
            if (competicaoFaseGrupo == null)
            {
                return HttpNotFound();
            }

            await ViewBags();
            return View(competicaoFaseGrupo);
        }

        //
        // POST: /CompeticoesFasesGrupos/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(CompeticaoFaseGrupo competicaoFaseGrupo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(competicaoFaseGrupo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["Mensagem"] = "Alteração realizada com sucesso!";
                return RedirectToAction("Fase", "Competicoes", new { id = competicaoFaseGrupo.CompeticaoFaseId });
            }


            await ViewBags();
            return View(competicaoFaseGrupo);
        }

        //
        // GET: /CompeticoesFasesGrupos/Excluir/5 
        public async Task<ActionResult> Excluir(System.Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompeticaoFaseGrupo competicaoFaseGrupo = await db.CompeticoesFasesGrupos.FindAsync(id);
            if (competicaoFaseGrupo == null)
            {
                return HttpNotFound();
            }


            await ViewBags();

            return View(competicaoFaseGrupo);
        }

        //
        // POST: /CompeticoesFasesGrupos/Excluir/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExcluirConfirmacao(System.Guid id)
        {
            CompeticaoFaseGrupo competicaoFaseGrupo = await db.CompeticoesFasesGrupos.FindAsync(id);
            db.CompeticoesFasesGrupos.Remove(competicaoFaseGrupo);
            await db.SaveChangesAsync();
            return RedirectToAction("Indice");
        }
        private async Task ViewBags()
        {
            ViewBag.CompeticaoFases = new SelectList(await db.CompeticoesFases.ToListAsync(), "CompeticaoFaseId", "");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
