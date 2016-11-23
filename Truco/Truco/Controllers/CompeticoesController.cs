using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Truco.Models;

namespace Truco.Controllers
{
    public class CompeticoesController : Controller
    {
        

        // GET: /Competicoes/
        public async Task<ActionResult> Indice()
        {
            return View(await db.Competicoes.ToListAsync());
        }

        // GET: /Competicoes/Detalhes/5
        public async Task<ActionResult> Detalhes(Guid? id)
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

        // GET: /Competicoes/Criar
        public ActionResult Criar()
        {
            return View();
        }

        // POST: /Competicoes/Criar
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Criar([Bind(Include="CompeticaoId,Nome,Tipo,Modalidade,UsuarioCad,DataHoraCad")] Competicao competicao)
        {
            if (ModelState.IsValid)
            {
                competicao.CompeticaoId = Guid.NewGuid();
                db.Competicoes.Add(competicao);
                await db.SaveChangesAsync();
                return RedirectToAction("Indice");
            }

            return View(competicao);
        }

        // GET: /Competicoes/Editar/5
        public async Task<ActionResult> Editar(Guid? id)
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

        // POST: /Competicoes/Editar/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar([Bind(Include="CompeticaoId,Nome,Tipo,Modalidade,UsuarioCad,DataHoraCad")] Competicao competicao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(competicao).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(competicao);
        }

        // GET: /Competicoes/Excluir/5
        public async Task<ActionResult> Excluir(Guid? id)
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

        // POST: /Competicoes/Excluir/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExcluirConfirmacao(Guid id)
        {
            Competicao competicao = await db.Competicoes.FindAsync(id);
            db.Competicoes.Remove(competicao);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
