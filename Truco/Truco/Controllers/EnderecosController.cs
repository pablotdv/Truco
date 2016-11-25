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

    public class EnderecosController : Controller
    {	        
        //
        // GET: /Enderecos/
        public async Task<ActionResult> Indice()
        {
			var viewModel = JsonConvert.DeserializeObject<EnderecosViewModel>(await PesquisaModelStore.GetAsync(PesquisaKey));

            return await Pesquisa(viewModel ?? new EnderecosViewModel());
        }

		//
        // GET: /Enderecos/Pesquisa
		public async Task<ActionResult> Pesquisa(EnderecosViewModel viewModel)
		{
			await PesquisaModelStore.AddAsync(PesquisaKey, viewModel);

			var query = db.Enderecos.AsQueryable();

			//TODO: parâmetros de pesquisa
			if (!String.IsNullOrWhiteSpace(viewModel.Numero))
            {
                var numeros = viewModel.Numero?.Split(' ');
                //query = query.Where(a => numeros.All(numero => a.Numero.Contains(numero)));
            }

            viewModel.Resultados = await query.OrderBy(a => a.Numero).ToPagedListAsync(viewModel.Pagina, viewModel.TamanhoPagina);

            if (Request.IsAjaxRequest())
                return PartialView("_Pesquisa", viewModel);

            return View("Indice", viewModel);
		}

        //
        // GET: /Enderecos/Detalhes/5
        public async Task<ActionResult> Detalhes(System.Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Endereco endereco = await db.Enderecos.FindAsync(id);
            if (endereco == null)
            {
                return HttpNotFound();
            }  
          
			await ViewBags();
            return View(endereco);
        }

        //
        // GET: /Enderecos/Criar        
          
		public async Task<ActionResult> Criar()
        {
			await ViewBags();
            return View();
        } 

        //
        // POST: /Enderecos/Criar
        [HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> Criar(Endereco endereco)
        {
            if (ModelState.IsValid)
            {
                endereco.EnderecoId = Guid.NewGuid();
                db.Enderecos.Add(endereco);
                await db.SaveChangesAsync();
				TempData["Mensagem"] = "Operação realizada com sucesso!";
                return RedirectToAction("Indice");  
            }

          
			await ViewBags();
            return View(endereco);
        }
        
        //
        // GET: /Enderecos/Editar/5 
        public async Task<ActionResult> Editar(System.Guid? id)
        {
			if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Endereco endereco = await db.Enderecos.FindAsync(id);
            if (endereco == null)
            {
                return HttpNotFound();
            }            
          
			await ViewBags();
            return View(endereco);
        }

        //
        // POST: /Enderecos/Editar/5
        [HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(Endereco endereco)
        {
            if (ModelState.IsValid)
            {
                db.Entry(endereco).State = EntityState.Modified;
                await db.SaveChangesAsync();
				TempData["Mensagem"] = "Alteração realizada com sucesso!";
                return RedirectToAction("Indice");
            }

          
			await ViewBags();
            return View(endereco);
        }

        //
        // GET: /Enderecos/Excluir/5 
        public async Task<ActionResult> Excluir(System.Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Endereco endereco = await db.Enderecos.FindAsync(id);
            if (endereco == null)
            {
                return HttpNotFound();
            }   

          
			await ViewBags();
  
            return View(endereco);
        }

        //
        // POST: /Enderecos/Excluir/5
        [HttpPost, ActionName("Excluir")]
		[ValidateAntiForgeryToken]
        public async Task<ActionResult> ExcluirConfirmacao(System.Guid id)
        {
            Endereco endereco = await db.Enderecos.FindAsync(id);
            db.Enderecos.Remove(endereco);
            await db.SaveChangesAsync();
            return RedirectToAction("Indice");
        }
		private async Task ViewBags()
		{
            ViewBag.Usuarios = new SelectList(await db.Users.ToListAsync(), "Id", "");
            ViewBag.Logradouroes = new SelectList(await db.Logradouros.ToListAsync(), "LogradouroId", "Descricao");
    
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
