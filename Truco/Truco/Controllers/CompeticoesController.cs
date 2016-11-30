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
using Truco.ViewModels.Enums;
using Truco.Models.Enums;

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

            var query = db.Competicoes
                .Include(a => a.CompeticoesEquipes)
                .Include(a => a.CompeticoesFases)
                .AsQueryable();

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
            Competicao competicao = await db.Competicoes
                .Include(a => a.CompeticoesFases)
                .FirstOrDefaultAsync(a => a.CompeticaoId == id);
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

        public async Task<ActionResult> Sorteio(Guid id)
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


            return View(new CompeticaoSorteioViewModel
            {
                Competicao = competicao,
                SorteioModo = CompeticaoSorteioModo.Cidades
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Sorteio(CompeticaoSorteioViewModel model)
        {
            var competicao = await db.Competicoes
                .Include(a => a.CompeticoesEquipes)
                .Include(a => a.CompeticoesFases)
                .FirstOrDefaultAsync(a => a.CompeticaoId == model.Competicao.CompeticaoId);

            if (competicao.Sorteada)
                ModelState.AddModelError("", $"A competição {competicao.Nome} já foi sorteada!");

            if (ModelState.IsValid)
            {
                int grupos3 = 0;
                int grupos4 = 0;
                int numeroEquipes = competicao.CompeticoesEquipes.Count;

                while (numeroEquipes % 4 != 0)
                {
                    grupos3++;
                    numeroEquipes = numeroEquipes - 3;
                }

                grupos4 = numeroEquipes / 4;

                var chaves = grupos3 + grupos4;

                var competicaoFase = new CompeticaoFase()
                {
                    CompeticaoFaseId = Guid.NewGuid(),
                    CompeticaoId = competicao.CompeticaoId,
                    Modo = CompeticaoFaseModo.Chaveamento,
                    Tipo = CompeticaoFaseTipo.Principal,
                    Nome = "1ª Fase",
                    CompeticoesFasesGrupos = new HashSet<CompeticaoFaseGrupo>()

                };
                List<CompeticaoFaseGrupo> competicoesFasesGrupos = null;
                switch (model.SorteioModo)
                {
                    case CompeticaoSorteioModo.Cidades: competicoesFasesGrupos = SorteioCidade(competicao.CompeticoesEquipes.ToList(), chaves); break;
                    case CompeticaoSorteioModo.Geral: competicoesFasesGrupos = SorteioGeral(competicao.CompeticoesEquipes.ToList(), chaves); break;
                    case CompeticaoSorteioModo.Regioes: competicoesFasesGrupos = SorteioRegiao(competicao.CompeticoesEquipes.ToList(), chaves); break;
                }

                foreach (var grupo in ReorganizaChaves(competicoesFasesGrupos))
                    competicaoFase.CompeticoesFasesGrupos.Add(grupo);

                CompeticaoFaseGrupoJogos(competicaoFase);

                competicao.CompeticoesFases.Add(competicaoFase);
                competicao.Sorteada = true;
                await db.SaveChangesAsync();
                return RedirectToAction("Fase", new { id = competicaoFase.CompeticaoFaseId });
            }
            model.Competicao = competicao;
            return View(model);
        }

        private List<CompeticaoFaseGrupo> ReorganizaChaves(List<CompeticaoFaseGrupo> competicoesFasesGrupos)
        {
            List<CompeticaoFaseGrupo> grupos = new List<CompeticaoFaseGrupo>();
            var chaves4 = competicoesFasesGrupos.Where(a => a.CompeticoesFasesGruposEquipes.Count == 4);

            foreach (var chave4 in chaves4)
            {
                grupos.Add(chave4);
            }

            var chaves3 = competicoesFasesGrupos.Where(a => a.CompeticoesFasesGruposEquipes.Count == 3).ToList();

            if (chaves3.Count == 1)
            {
                grupos.Add(chaves3.FirstOrDefault());
            }
            else if (chaves3.Count == 2)
            {
                var chave6 = new CompeticaoFaseGrupo()
                {
                    CompeticaoFaseGrupoId = Guid.NewGuid(),
                    Grupo = chaves3[0].Grupo,
                    Nome = $"Chave {chaves3[0].Grupo} & {chaves3[1].Grupo}",
                    CompeticoesFasesGruposEquipes = new HashSet<CompeticaoFaseGrupoEquipe>()
                };

                foreach (var e in chaves3[0].CompeticoesFasesGruposEquipes)
                {
                    chave6.CompeticoesFasesGruposEquipes.Add(new CompeticaoFaseGrupoEquipe()
                    {
                        CompeticaoFaseGrupoEquipeId = Guid.NewGuid(),
                        EquipeId = e.EquipeId,
                        Lado = Lado.LadoA,
                        Numero = e.Numero,
                    });
                }

                foreach (var e in chaves3[1].CompeticoesFasesGruposEquipes)
                {
                    chave6.CompeticoesFasesGruposEquipes.Add(new CompeticaoFaseGrupoEquipe()
                    {
                        CompeticaoFaseGrupoEquipeId = Guid.NewGuid(),
                        EquipeId = e.EquipeId,
                        Lado = Lado.LadoB,
                        Numero = e.Numero,
                    });
                }
                grupos.Add(chave6);
            }
            else if (chaves3.Count == 3)
            {
                var chave6 = new CompeticaoFaseGrupo()
                {
                    CompeticaoFaseGrupoId = Guid.NewGuid(),
                    Grupo = chaves3[0].Grupo,
                    Nome = $"Chave {chaves3[0].Grupo} & {chaves3[1].Grupo}",
                    CompeticoesFasesGruposEquipes = new HashSet<CompeticaoFaseGrupoEquipe>()
                };

                foreach (var e in chaves3[0].CompeticoesFasesGruposEquipes)
                {
                    chave6.CompeticoesFasesGruposEquipes.Add(new CompeticaoFaseGrupoEquipe()
                    {
                        CompeticaoFaseGrupoEquipeId = Guid.NewGuid(),
                        EquipeId = e.EquipeId,
                        Lado = Lado.LadoA,
                        Numero = e.Numero,
                    });
                }

                foreach (var e in chaves3[1].CompeticoesFasesGruposEquipes)
                {
                    chave6.CompeticoesFasesGruposEquipes.Add(new CompeticaoFaseGrupoEquipe()
                    {
                        CompeticaoFaseGrupoEquipeId = Guid.NewGuid(),
                        EquipeId = e.EquipeId,
                        Lado = Lado.LadoB,
                        Numero = e.Numero,
                    });
                }
                grupos.Add(chave6);
                grupos.Add(chaves3[2]);

            }
            else if (chaves3.Count == 4)
            {
                var chave6A = new CompeticaoFaseGrupo()
                {
                    CompeticaoFaseGrupoId = Guid.NewGuid(),
                    Grupo = chaves3[0].Grupo,
                    Nome = $"Chave {chaves3[0].Grupo} & {chaves3[1].Grupo}",
                    CompeticoesFasesGruposEquipes = new HashSet<CompeticaoFaseGrupoEquipe>()
                };

                foreach (var e in chaves3[0].CompeticoesFasesGruposEquipes)
                {
                    chave6A.CompeticoesFasesGruposEquipes.Add(new CompeticaoFaseGrupoEquipe()
                    {
                        CompeticaoFaseGrupoEquipeId = Guid.NewGuid(),
                        EquipeId = e.EquipeId,
                        Lado = Lado.LadoA,
                        Numero = e.Numero,
                    });
                }

                foreach (var e in chaves3[1].CompeticoesFasesGruposEquipes)
                {
                    chave6A.CompeticoesFasesGruposEquipes.Add(new CompeticaoFaseGrupoEquipe()
                    {
                        CompeticaoFaseGrupoEquipeId = Guid.NewGuid(),
                        EquipeId = e.EquipeId,
                        Lado = Lado.LadoB,
                        Numero = e.Numero,
                    });
                }

                var chave6B = new CompeticaoFaseGrupo()
                {
                    CompeticaoFaseGrupoId = Guid.NewGuid(),
                    Grupo = chaves3[0].Grupo,
                    Nome = $"Chave {chaves3[2].Grupo} & {chaves3[3].Grupo}",
                    CompeticoesFasesGruposEquipes = new HashSet<CompeticaoFaseGrupoEquipe>()
                };

                foreach (var e in chaves3[2].CompeticoesFasesGruposEquipes)
                {
                    chave6B.CompeticoesFasesGruposEquipes.Add(new CompeticaoFaseGrupoEquipe()
                    {
                        CompeticaoFaseGrupoEquipeId = Guid.NewGuid(),
                        EquipeId = e.EquipeId,
                        Lado = Lado.LadoA,
                        Numero = e.Numero,
                    });
                }

                foreach (var e in chaves3[3].CompeticoesFasesGruposEquipes)
                {
                    chave6B.CompeticoesFasesGruposEquipes.Add(new CompeticaoFaseGrupoEquipe()
                    {
                        CompeticaoFaseGrupoEquipeId = Guid.NewGuid(),
                        EquipeId = e.EquipeId,
                        Lado = Lado.LadoB,
                        Numero = e.Numero,
                    });
                }

                grupos.Add(chave6A);
                grupos.Add(chave6B);
            }

            return grupos;
        }

        private void CompeticaoFaseGrupoJogos(CompeticaoFase competicaoFase)
        {
            foreach (var grupo in competicaoFase.CompeticoesFasesGrupos)
            {
                if (grupo.CompeticoesFasesGruposRodadas == null)
                    grupo.CompeticoesFasesGruposRodadas = new HashSet<CompeticaoFaseGrupoRodada>();

                if (grupo.CompeticoesFasesGruposEquipes.Count == 6)
                {
                    CompeticaoFaseGrupoJogosChave6(grupo);
                }
                else if (grupo.CompeticoesFasesGruposEquipes.Count == 4)
                {
                    CompeticaoFaseGrupoJogosChave4(grupo);
                }
                else if (grupo.CompeticoesFasesGruposEquipes.Count == 3)
                {
                    CompeticaoFaseGrupoJogosChave3(grupo);
                }
            }

        }

        private void CompeticaoFaseGrupoJogosChave6(CompeticaoFaseGrupo grupo)
        {
            var equipeA1 = grupo.CompeticoesFasesGruposEquipes.FirstOrDefault(a => a.Numero == 1 && a.Lado == Lado.LadoA);
            var equipeA2 = grupo.CompeticoesFasesGruposEquipes.FirstOrDefault(a => a.Numero == 2 && a.Lado == Lado.LadoA);
            var equipeA3 = grupo.CompeticoesFasesGruposEquipes.FirstOrDefault(a => a.Numero == 3 && a.Lado == Lado.LadoA);

            var equipeB1 = grupo.CompeticoesFasesGruposEquipes.FirstOrDefault(a => a.Numero == 1 && a.Lado == Lado.LadoB);
            var equipeB2 = grupo.CompeticoesFasesGruposEquipes.FirstOrDefault(a => a.Numero == 2 && a.Lado == Lado.LadoB);
            var equipeB3 = grupo.CompeticoesFasesGruposEquipes.FirstOrDefault(a => a.Numero == 3 && a.Lado == Lado.LadoB);

            grupo.CompeticoesFasesGruposRodadas.Add(new CompeticaoFaseGrupoRodada()
            {
                CompeticaoFaseGrupoRodadaId = Guid.NewGuid(),
                Rodada = 1,
                CompeticoesFasesGruposRodadasJogos = new HashSet<CompeticaoFaseGrupoRodadaJogo>()
                {
                    new CompeticaoFaseGrupoRodadaJogo() {
                        CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(),
                        CompeticaoFaseGrupoRodadaJogoEquipeUm = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipeA1.CompeticaoFaseGrupoEquipeId },
                        CompeticaoFaseGrupoRodadaJogoEquipeDois = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipeB1.CompeticaoFaseGrupoEquipeId },
                    },
                    new CompeticaoFaseGrupoRodadaJogo() {
                        CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(),
                        CompeticaoFaseGrupoRodadaJogoEquipeUm = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipeA2.CompeticaoFaseGrupoEquipeId },
                        CompeticaoFaseGrupoRodadaJogoEquipeDois = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipeB2.CompeticaoFaseGrupoEquipeId },
                    },
                    new CompeticaoFaseGrupoRodadaJogo() {
                        CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(),
                        CompeticaoFaseGrupoRodadaJogoEquipeUm = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipeA3.CompeticaoFaseGrupoEquipeId },
                        CompeticaoFaseGrupoRodadaJogoEquipeDois = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipeB3.CompeticaoFaseGrupoEquipeId },
                    }
                }
            });

            grupo.CompeticoesFasesGruposRodadas.Add(new CompeticaoFaseGrupoRodada()
            {
                CompeticaoFaseGrupoRodadaId = Guid.NewGuid(),
                Rodada = 2,
                CompeticoesFasesGruposRodadasJogos = new HashSet<CompeticaoFaseGrupoRodadaJogo>()
                {
                    new CompeticaoFaseGrupoRodadaJogo() {
                        CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(),
                        CompeticaoFaseGrupoRodadaJogoEquipeUm = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipeA1.CompeticaoFaseGrupoEquipeId },
                        CompeticaoFaseGrupoRodadaJogoEquipeDois = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipeB2.CompeticaoFaseGrupoEquipeId },
                    },
                    new CompeticaoFaseGrupoRodadaJogo() {
                        CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(),
                        CompeticaoFaseGrupoRodadaJogoEquipeUm = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipeA2.CompeticaoFaseGrupoEquipeId },
                        CompeticaoFaseGrupoRodadaJogoEquipeDois = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipeB3.CompeticaoFaseGrupoEquipeId },
                    },
                    new CompeticaoFaseGrupoRodadaJogo() {
                        CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(),
                        CompeticaoFaseGrupoRodadaJogoEquipeUm = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipeA3.CompeticaoFaseGrupoEquipeId },
                        CompeticaoFaseGrupoRodadaJogoEquipeDois = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipeB1.CompeticaoFaseGrupoEquipeId },
                    }
                }
            });

            grupo.CompeticoesFasesGruposRodadas.Add(new CompeticaoFaseGrupoRodada()
            {
                CompeticaoFaseGrupoRodadaId = Guid.NewGuid(),
                Rodada = 3,
                CompeticoesFasesGruposRodadasJogos = new HashSet<CompeticaoFaseGrupoRodadaJogo>()
                {
                    new CompeticaoFaseGrupoRodadaJogo() {
                        CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(),
                        CompeticaoFaseGrupoRodadaJogoEquipeUm = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipeA1.CompeticaoFaseGrupoEquipeId },
                        CompeticaoFaseGrupoRodadaJogoEquipeDois = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipeB3.CompeticaoFaseGrupoEquipeId },
                    },
                    new CompeticaoFaseGrupoRodadaJogo() {
                        CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(),
                        CompeticaoFaseGrupoRodadaJogoEquipeUm = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipeA2.CompeticaoFaseGrupoEquipeId },
                        CompeticaoFaseGrupoRodadaJogoEquipeDois = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipeB1.CompeticaoFaseGrupoEquipeId },
                    },
                    new CompeticaoFaseGrupoRodadaJogo() {
                        CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(),
                        CompeticaoFaseGrupoRodadaJogoEquipeUm = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipeA3.CompeticaoFaseGrupoEquipeId },
                        CompeticaoFaseGrupoRodadaJogoEquipeDois = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipeB2.CompeticaoFaseGrupoEquipeId },
                    }
                }
            });
        }

        private void CompeticaoFaseGrupoJogosChave3(CompeticaoFaseGrupo grupo)
        {
            var equipe1 = grupo.CompeticoesFasesGruposEquipes.FirstOrDefault(a => a.Numero == 1);
            var equipe2 = grupo.CompeticoesFasesGruposEquipes.FirstOrDefault(a => a.Numero == 2);
            var equipe3 = grupo.CompeticoesFasesGruposEquipes.FirstOrDefault(a => a.Numero == 3);

            grupo.CompeticoesFasesGruposRodadas.Add(new CompeticaoFaseGrupoRodada()
            {
                CompeticaoFaseGrupoRodadaId = Guid.NewGuid(),
                Rodada = 1,
                CompeticoesFasesGruposRodadasJogos = new HashSet<CompeticaoFaseGrupoRodadaJogo>()
                {
                    new CompeticaoFaseGrupoRodadaJogo() {
                        CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(),
                        CompeticaoFaseGrupoRodadaJogoEquipeUm = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipe1.CompeticaoFaseGrupoEquipeId },
                        CompeticaoFaseGrupoRodadaJogoEquipeDois = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipe2.CompeticaoFaseGrupoEquipeId },
                    },
                }
            });
            grupo.CompeticoesFasesGruposRodadas.Add(new CompeticaoFaseGrupoRodada()
            {
                CompeticaoFaseGrupoRodadaId = Guid.NewGuid(),
                Rodada = 2,
                CompeticoesFasesGruposRodadasJogos = new HashSet<CompeticaoFaseGrupoRodadaJogo>()
                {
                    new CompeticaoFaseGrupoRodadaJogo() {
                        CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(),
                        CompeticaoFaseGrupoRodadaJogoEquipeUm = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipe1.CompeticaoFaseGrupoEquipeId },
                        CompeticaoFaseGrupoRodadaJogoEquipeDois = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipe3.CompeticaoFaseGrupoEquipeId },
                    },
                }
            });
            grupo.CompeticoesFasesGruposRodadas.Add(new CompeticaoFaseGrupoRodada()
            {
                CompeticaoFaseGrupoRodadaId = Guid.NewGuid(),
                Rodada = 3,
                CompeticoesFasesGruposRodadasJogos = new HashSet<CompeticaoFaseGrupoRodadaJogo>()
                {
                    new CompeticaoFaseGrupoRodadaJogo() {
                        CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(),
                        CompeticaoFaseGrupoRodadaJogoEquipeUm = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipe2.CompeticaoFaseGrupoEquipeId },
                        CompeticaoFaseGrupoRodadaJogoEquipeDois = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipe3.CompeticaoFaseGrupoEquipeId },
                    },
                }
            });
        }

        private static void CompeticaoFaseGrupoJogosChave4(CompeticaoFaseGrupo grupo)
        {
            var equipe1 = grupo.CompeticoesFasesGruposEquipes.FirstOrDefault(a => a.Numero == 1);
            var equipe2 = grupo.CompeticoesFasesGruposEquipes.FirstOrDefault(a => a.Numero == 2);
            var equipe3 = grupo.CompeticoesFasesGruposEquipes.FirstOrDefault(a => a.Numero == 3);
            var equipe4 = grupo.CompeticoesFasesGruposEquipes.FirstOrDefault(a => a.Numero == 4);

            grupo.CompeticoesFasesGruposRodadas.Add(new CompeticaoFaseGrupoRodada()
            {
                CompeticaoFaseGrupoRodadaId = Guid.NewGuid(),
                Rodada = 1,
                CompeticoesFasesGruposRodadasJogos = new HashSet<CompeticaoFaseGrupoRodadaJogo>()
                {
                    new CompeticaoFaseGrupoRodadaJogo() {
                        CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(),
                        CompeticaoFaseGrupoRodadaJogoEquipeUm = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipe1.CompeticaoFaseGrupoEquipeId },
                        CompeticaoFaseGrupoRodadaJogoEquipeDois = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipe2.CompeticaoFaseGrupoEquipeId },
                    },
                    new CompeticaoFaseGrupoRodadaJogo() {
                        CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(),
                        CompeticaoFaseGrupoRodadaJogoEquipeUm = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipe3.CompeticaoFaseGrupoEquipeId },
                        CompeticaoFaseGrupoRodadaJogoEquipeDois = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipe4.CompeticaoFaseGrupoEquipeId },
                    },
                }
            });
            grupo.CompeticoesFasesGruposRodadas.Add(new CompeticaoFaseGrupoRodada()
            {
                CompeticaoFaseGrupoRodadaId = Guid.NewGuid(),
                Rodada = 2,
                CompeticoesFasesGruposRodadasJogos = new HashSet<CompeticaoFaseGrupoRodadaJogo>()
                {
                    new CompeticaoFaseGrupoRodadaJogo() {
                        CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(),
                        CompeticaoFaseGrupoRodadaJogoEquipeUm = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipe1.CompeticaoFaseGrupoEquipeId },
                        CompeticaoFaseGrupoRodadaJogoEquipeDois = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipe3.CompeticaoFaseGrupoEquipeId },
                    },
                    new CompeticaoFaseGrupoRodadaJogo() {
                        CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(),
                        CompeticaoFaseGrupoRodadaJogoEquipeUm = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipe4.CompeticaoFaseGrupoEquipeId },
                        CompeticaoFaseGrupoRodadaJogoEquipeDois = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipe2.CompeticaoFaseGrupoEquipeId },
                    },
                }
            });
            grupo.CompeticoesFasesGruposRodadas.Add(new CompeticaoFaseGrupoRodada()
            {
                CompeticaoFaseGrupoRodadaId = Guid.NewGuid(),
                Rodada = 3,
                CompeticoesFasesGruposRodadasJogos = new HashSet<CompeticaoFaseGrupoRodadaJogo>()
                {
                    new CompeticaoFaseGrupoRodadaJogo() {
                        CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(),
                        CompeticaoFaseGrupoRodadaJogoEquipeUm = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipe1.CompeticaoFaseGrupoEquipeId },
                        CompeticaoFaseGrupoRodadaJogoEquipeDois = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipe4.CompeticaoFaseGrupoEquipeId },
                    },
                    new CompeticaoFaseGrupoRodadaJogo() {
                        CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(),
                        CompeticaoFaseGrupoRodadaJogoEquipeUm = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipe2.CompeticaoFaseGrupoEquipeId },
                        CompeticaoFaseGrupoRodadaJogoEquipeDois = new CompeticaoFaseGrupoRodadaJogoEquipe() { CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeId = equipe3.CompeticaoFaseGrupoEquipeId },
                    },
                }
            });
        }

        private static List<CompeticaoFaseGrupo> SorteioGeral(IList<CompeticaoEquipe> competicaoEquipes, int chaves)
        {
            List<CompeticaoFaseGrupo> competicoesFasesGrupos = MontarChaveamento(chaves);

            var equipes = competicaoEquipes.OrderByDescending(a => Guid.NewGuid());

            int indiceChave = 1;

            foreach (var equipe in equipes)
            {
                competicoesFasesGrupos[indiceChave - 1].CompeticoesFasesGruposEquipes.Add(new CompeticaoFaseGrupoEquipe()
                {
                    CompeticaoFaseGrupoEquipeId = Guid.NewGuid(),
                    EquipeId = equipe.Equipe.EquipeId,
                    Numero = competicoesFasesGrupos[indiceChave - 1].CompeticoesFasesGruposEquipes.Count() + 1
                });
                indiceChave++;

                if (indiceChave > chaves)
                    indiceChave = 1;
            }

            return competicoesFasesGrupos;
        }

        private static List<CompeticaoFaseGrupo> SorteioPosicao(IList<CompeticaoEquipe> competicaoEquipes, int chaves)
        {
            List<CompeticaoFaseGrupo> competicoesFasesGrupos = MontarChaveamento(chaves);

            int indiceChave = 1;

            foreach (var equipe in competicaoEquipes)
            {
                competicoesFasesGrupos[indiceChave - 1].CompeticoesFasesGruposEquipes.Add(new CompeticaoFaseGrupoEquipe()
                {
                    CompeticaoFaseGrupoEquipeId = Guid.NewGuid(),
                    EquipeId = equipe.EquipeId,
                    Numero = competicoesFasesGrupos[indiceChave - 1].CompeticoesFasesGruposEquipes.Count() + 1
                });
                indiceChave++;

                if (indiceChave > chaves)
                    indiceChave = 1;
            }

            return competicoesFasesGrupos;
        }

        private static List<CompeticaoFaseGrupo> SorteioRegiao(IList<CompeticaoEquipe> competicaoEquipes, int chaves)
        {
            List<CompeticaoFaseGrupo> competicoesFasesGrupos = MontarChaveamento(chaves);

            var equipesRegioes = competicaoEquipes.GroupBy(a => a.Equipe.RegiaoId).OrderByDescending(a => a.Count());

            int indiceChave = 1;

            foreach (var equipesRegiao in equipesRegioes)
            {
                var equipes = equipesRegiao.OrderByDescending(a => Guid.NewGuid());
                foreach (var equipe in equipes)
                {
                    competicoesFasesGrupos[indiceChave - 1].CompeticoesFasesGruposEquipes.Add(new CompeticaoFaseGrupoEquipe()
                    {
                        CompeticaoFaseGrupoEquipeId = Guid.NewGuid(),
                        EquipeId = equipe.EquipeId,
                        Numero = competicoesFasesGrupos[indiceChave - 1].CompeticoesFasesGruposEquipes.Count() + 1
                    });
                    indiceChave++;

                    if (indiceChave > chaves)
                        indiceChave = 1;
                }
            }

            return competicoesFasesGrupos;
        }

        private static List<CompeticaoFaseGrupo> SorteioCidade(IList<CompeticaoEquipe> competicaoEquipes, int chaves)
        {
            List<CompeticaoFaseGrupo> competicoesFasesGrupos = MontarChaveamento(chaves);

            var equipesCidades = competicaoEquipes.GroupBy(a => a.Equipe.CidadeId).OrderByDescending(a => a.Count());

            int indiceChave = 1;

            foreach (var equipesCidade in equipesCidades)
            {
                var equipes = equipesCidade.OrderByDescending(a => Guid.NewGuid());
                foreach (var equipe in equipes)
                {
                    competicoesFasesGrupos[indiceChave - 1].CompeticoesFasesGruposEquipes.Add(new CompeticaoFaseGrupoEquipe()
                    {
                        CompeticaoFaseGrupoEquipeId = Guid.NewGuid(),
                        EquipeId = equipe.EquipeId,
                        Numero = competicoesFasesGrupos[indiceChave - 1].CompeticoesFasesGruposEquipes.Count() + 1
                    });
                    indiceChave++;

                    if (indiceChave > chaves)
                        indiceChave = 1;
                }
            }

            return competicoesFasesGrupos;
        }

        private static List<CompeticaoFaseGrupo> MontarChaveamento(int chaves)
        {
            var competicoesFasesGrupos = new List<CompeticaoFaseGrupo>();
            for (int i = 1; i <= chaves; i++)
            {
                competicoesFasesGrupos.Add(new CompeticaoFaseGrupo()
                {
                    CompeticaoFaseGrupoId = Guid.NewGuid(),
                    Nome = $"Chave {i}",
                    Grupo = i,
                    CompeticoesFasesGruposEquipes = new HashSet<CompeticaoFaseGrupoEquipe>()
                });
            }

            return competicoesFasesGrupos;
        }

        public async Task<ActionResult> Fase(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var competicaoFaseGrupos = await db.CompeticoesFasesGrupos
                .Include(a => a.CompeticoesFasesGruposEquipes)
                .Where(a => a.CompeticaoFaseId == id)
                .OrderBy(a => a.Grupo)
                .ToListAsync();

            return View(competicaoFaseGrupos);
        }

        public async Task<ActionResult> Jogos(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var competicaoFaseGruposRodadasJogos = await db.CompeticoesFasesGruposRodadasJogos

                .Where(a => a.CompeticaoFaseGrupoRodada.CompeticaoFaseGrupoId == id)
                .OrderBy(a => a.CompeticaoFaseGrupoRodada.Rodada)
                .ThenBy(a => a.CompeticaoFaseGrupoRodadaJogoEquipeUm.CompeticaoFaseGrupoEquipe.Numero)
                .ToListAsync();

            return View(competicaoFaseGruposRodadasJogos);
        }

        public async Task<ActionResult> Jogo(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var jogo = await db.CompeticoesFasesGruposRodadasJogos
                .Include(a => a.CompeticaoFaseGrupoRodadaJogoEquipeUm.CompeticoesFasesGruposRodadasJogosEquipesSets)
                .Include(a => a.CompeticaoFaseGrupoRodadaJogoEquipeDois.CompeticoesFasesGruposRodadasJogosEquipesSets)
                .FirstOrDefaultAsync(a => a.CompeticaoFaseGrupoRodadaJogoId == id);

            if (jogo == null)
            {
                return HttpNotFound();
            }

            if (jogo.CompeticaoFaseGrupoRodadaJogoEquipeUm.CompeticoesFasesGruposRodadasJogosEquipesSets.Count == 0)
            {
                for (int i = 1; i <= 3; i++)
                {
                    jogo.CompeticaoFaseGrupoRodadaJogoEquipeUm.CompeticoesFasesGruposRodadasJogosEquipesSets.Add(new CompeticaoFaseGrupoRodadaJogoEquipeSet()
                    {
                        CompeticaoFaseGrupoRodadaJogoEquipeSetId = Guid.NewGuid(),
                        Set = i
                    });
                }
            }

            if (jogo.CompeticaoFaseGrupoRodadaJogoEquipeDois.CompeticoesFasesGruposRodadasJogosEquipesSets.Count == 0)
            {
                for (int i = 1; i <= 3; i++)
                {
                    jogo.CompeticaoFaseGrupoRodadaJogoEquipeDois.CompeticoesFasesGruposRodadasJogosEquipesSets.Add(new CompeticaoFaseGrupoRodadaJogoEquipeSet()
                    {
                        CompeticaoFaseGrupoRodadaJogoEquipeSetId = Guid.NewGuid(),
                        Set = i
                    });
                }
            }

            return View(jogo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Jogo(CompeticaoFaseGrupoRodadaJogo model)
        {
            if (ModelState.IsValid)
            {
                var jogo = await db.CompeticoesFasesGruposRodadasJogos
                    .Include(a => a.CompeticaoFaseGrupoRodadaJogoEquipeUm.CompeticoesFasesGruposRodadasJogosEquipesSets)
                    .Include(a => a.CompeticaoFaseGrupoRodadaJogoEquipeDois.CompeticoesFasesGruposRodadasJogosEquipesSets)
                    .FirstOrDefaultAsync(a => a.CompeticaoFaseGrupoRodadaJogoId == model.CompeticaoFaseGrupoRodadaJogoId);

                if (jogo.CompeticaoFaseGrupoRodadaJogoEquipeUm.CompeticoesFasesGruposRodadasJogosEquipesSets.Count != 0)
                {
                    var sets = jogo.CompeticaoFaseGrupoRodadaJogoEquipeUm.CompeticoesFasesGruposRodadasJogosEquipesSets.ToList();
                    foreach (var set in sets)
                    {
                        db.CompeticoesFasesGruposRodadasJogosEquipesSets.Remove(set);
                        jogo.CompeticaoFaseGrupoRodadaJogoEquipeUm.CompeticoesFasesGruposRodadasJogosEquipesSets.Remove(set);
                    }
                }

                if (jogo.CompeticaoFaseGrupoRodadaJogoEquipeDois.CompeticoesFasesGruposRodadasJogosEquipesSets.Count != 0)
                {
                    var sets = jogo.CompeticaoFaseGrupoRodadaJogoEquipeDois.CompeticoesFasesGruposRodadasJogosEquipesSets.ToList();
                    foreach (var set in sets)
                    {
                        db.CompeticoesFasesGruposRodadasJogosEquipesSets.Remove(set);
                        jogo.CompeticaoFaseGrupoRodadaJogoEquipeDois.CompeticoesFasesGruposRodadasJogosEquipesSets.Remove(set);
                    }
                }

                foreach (var set in model.CompeticaoFaseGrupoRodadaJogoEquipeUm.CompeticoesFasesGruposRodadasJogosEquipesSets)
                {
                    jogo.CompeticaoFaseGrupoRodadaJogoEquipeUm.CompeticoesFasesGruposRodadasJogosEquipesSets.Add(new CompeticaoFaseGrupoRodadaJogoEquipeSet()
                    {
                        CompeticaoFaseGrupoRodadaJogoEquipeSetId = Guid.NewGuid(),
                        Set = set.Set,
                        Tentos = set.Tentos
                    });
                }

                foreach (var set in model.CompeticaoFaseGrupoRodadaJogoEquipeDois.CompeticoesFasesGruposRodadasJogosEquipesSets)
                {
                    jogo.CompeticaoFaseGrupoRodadaJogoEquipeDois.CompeticoesFasesGruposRodadasJogosEquipesSets.Add(new CompeticaoFaseGrupoRodadaJogoEquipeSet()
                    {
                        CompeticaoFaseGrupoRodadaJogoEquipeSetId = Guid.NewGuid(),
                        Set = set.Set,
                        Tentos = set.Tentos
                    });
                }

                await db.SaveChangesAsync();

                var jogosEquipeUm = db.CompeticoesFasesGruposRodadasJogosEquipesSets
                    .Where(a => a.CompeticaoFaseGrupoRodadaJogoEquipe.CompeticaoFaseGrupoEquipeId == jogo.CompeticaoFaseGrupoRodadaJogoEquipeUm.CompeticaoFaseGrupoEquipeId);

                var jogosEquipeDois = db.CompeticoesFasesGruposRodadasJogosEquipesSets
                    .Where(a => a.CompeticaoFaseGrupoRodadaJogoEquipe.CompeticaoFaseGrupoEquipeId == jogo.CompeticaoFaseGrupoRodadaJogoEquipeDois.CompeticaoFaseGrupoEquipeId);

                jogo.CompeticaoFaseGrupoRodadaJogoEquipeUm.CompeticaoFaseGrupoEquipe.Jogos = jogosEquipeUm
                    .GroupBy(a => a.CompeticaoFaseGrupoRodadaJogoEquipe.CompeticaoFaseGrupoRodadaJogoEquipeId)
                    .Count();
                jogo.CompeticaoFaseGrupoRodadaJogoEquipeDois.CompeticaoFaseGrupoEquipe.Jogos = jogosEquipeDois
                    .GroupBy(a => a.CompeticaoFaseGrupoRodadaJogoEquipe.CompeticaoFaseGrupoRodadaJogoEquipeId)
                    .Count();

                jogo.CompeticaoFaseGrupoRodadaJogoEquipeUm.CompeticaoFaseGrupoEquipe.Vitorias = jogosEquipeUm
                    .Where(a => a.Tentos == 24)
                    .GroupBy(a => a.CompeticaoFaseGrupoRodadaJogoEquipe.CompeticaoFaseGrupoRodadaJogoEquipeId)
                    .Where(a => a.Count() >= 2)
                    .Count();
                jogo.CompeticaoFaseGrupoRodadaJogoEquipeDois.CompeticaoFaseGrupoEquipe.Vitorias = jogosEquipeDois
                    .Where(a => a.Tentos == 24)
                    .GroupBy(a => a.CompeticaoFaseGrupoRodadaJogoEquipe.CompeticaoFaseGrupoRodadaJogoEquipeId)
                    .Where(a => a.Count() >= 2)
                    .Count();

                jogo.CompeticaoFaseGrupoRodadaJogoEquipeUm.CompeticaoFaseGrupoEquipe.Sets = jogosEquipeUm
                    .Where(a => a.Tentos == 24)
                    .Count();
                jogo.CompeticaoFaseGrupoRodadaJogoEquipeDois.CompeticaoFaseGrupoEquipe.Sets = jogosEquipeDois
                    .Where(a => a.Tentos == 24)
                    .Count();


                jogo.CompeticaoFaseGrupoRodadaJogoEquipeUm.CompeticaoFaseGrupoEquipe.Tentos = jogosEquipeUm
                    .Sum(a => a.Tentos);
                jogo.CompeticaoFaseGrupoRodadaJogoEquipeDois.CompeticaoFaseGrupoEquipe.Tentos = jogosEquipeDois
                    .Sum(a => a.Tentos);

                var equipe = jogo.CompeticaoFaseGrupoRodadaJogoEquipeUm.CompeticaoFaseGrupoEquipe;
                double vitorias = (double)(equipe.Vitorias * 100) / equipe.Jogos;
                double setes = (double)(equipe.Sets * 100) / (equipe.Jogos * 3);
                double tentos = (double)(equipe.Tentos * 100) / (equipe.Jogos * 72);
                double aproveitamento = (double)(vitorias + setes + tentos) / 3;
                jogo.CompeticaoFaseGrupoRodadaJogoEquipeUm.CompeticaoFaseGrupoEquipe.Aproveitamento = (decimal)aproveitamento;

                equipe = jogo.CompeticaoFaseGrupoRodadaJogoEquipeDois.CompeticaoFaseGrupoEquipe;
                vitorias = (double)(equipe.Vitorias * 100) / equipe.Jogos;
                setes = (double)(equipe.Sets * 100) / (equipe.Jogos * 3);
                tentos = (double)(equipe.Tentos * 100) / (equipe.Jogos * 72);
                aproveitamento = (double)(vitorias + setes + tentos) / 3;
                jogo.CompeticaoFaseGrupoRodadaJogoEquipeDois.CompeticaoFaseGrupoEquipe.Aproveitamento = (decimal)aproveitamento;

                await db.SaveChangesAsync();

                return RedirectToAction("Jogos", new { id = jogo.CompeticaoFaseGrupoRodada.CompeticaoFaseGrupoId });
            }
            return View(model);
        }

        public async Task<ActionResult> Classificacao(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var grupos = await db.CompeticoesFasesGrupos
                .Include(a => a.CompeticoesFasesGruposEquipes)
                .OrderBy(a => a.Grupo)
                .Where(a => a.CompeticaoFaseId == id)
                .ToListAsync();

            if (grupos == null)
            {
                return HttpNotFound();
            }

            ClassificacaoViewModel model = new ClassificacaoViewModel()
            {
                CompeticaoFaseId = id,
                Equipes = new List<ClassificacaoEquipeViewModel>()
            };

            foreach (var grupo in grupos)
            {
                var numeroEquipes = grupo.CompeticoesFasesGruposEquipes.Count();
                var equipes = grupo.CompeticoesFasesGruposEquipes
                    .OrderByDescending(a => a.Vitorias)
                    .ThenByDescending(a => a.Sets)
                    .ThenByDescending(a => a.Tentos)
                    .ToList();
                foreach (var equipe in equipes)
                {
                    var posicao = equipes.IndexOf(equipe) + 1;

                    var classificacao = Truco.ViewModels.Enums.Classificacao.Principal;

                    if (((numeroEquipes == 4 || numeroEquipes == 3) && posicao > 2) || (numeroEquipes == 6 && posicao > 4))
                        classificacao = ViewModels.Enums.Classificacao.Repescagem;

                    model.Equipes.Add(new ClassificacaoEquipeViewModel()
                    {
                        Posicao = posicao,
                        CompeticaoFaseGrupoEquipe = equipe,
                        Classificacao = classificacao,
                    });
                }
            }


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Classificacao")]
        public async Task<ActionResult> ClassificacaoConfirmacao(Guid id)
        {
            var grupos = await db.CompeticoesFasesGrupos
                .Include(a => a.CompeticoesFasesGruposEquipes)
                .OrderBy(a => a.Grupo)
                .Where(a => a.CompeticaoFaseId == id)
                .ToListAsync();

            IList<CompeticaoEquipe> equipesPrincipal = new List<CompeticaoEquipe>();
            IList<CompeticaoEquipe> equipesRepescagem = new List<CompeticaoEquipe>();
            foreach (var grupo in grupos)
            {
                var numeroEquipes = grupo.CompeticoesFasesGruposEquipes.Count();
                var equipes = grupo.CompeticoesFasesGruposEquipes
                    .OrderByDescending(a => a.Vitorias)
                    .ThenByDescending(a => a.Sets)
                    .ThenByDescending(a => a.Tentos)
                    .ToList();
                foreach (var equipe in equipes)
                {
                    var posicao = equipes.IndexOf(equipe) + 1;

                    //var classificacao = Truco.ViewModels.Enums.Classificacao.Principal;

                    if (((numeroEquipes == 4 || numeroEquipes == 3) && posicao > 2) || (numeroEquipes == 6 && posicao > 4))
                    {
                        equipesRepescagem.Add(new CompeticaoEquipe()
                        {
                            CompeticaoEquipeId = Guid.NewGuid(),
                            EquipeId = equipe.EquipeId,
                            Aproveitamento = equipe.Aproveitamento
                        });
                    }
                    else
                    {
                        equipesPrincipal.Add(new CompeticaoEquipe()
                        {
                            CompeticaoEquipeId = Guid.NewGuid(),
                            EquipeId = equipe.EquipeId,
                            Aproveitamento = equipe.Aproveitamento
                        });
                    }
                }
            }

            var competicao = grupos.FirstOrDefault().CompeticaoFase.Competicao;

            CompeticaoFase competicaoFasePrincipal = new CompeticaoFase()
            {
                CompeticaoFaseId = Guid.NewGuid(),
                CompeticaoId = competicao.CompeticaoId,
                Modo = CompeticaoFaseModo.Chaveamento,
                Nome = "2ª Fase Principal",
                Tipo = CompeticaoFaseTipo.Principal,
                CompeticoesFasesGrupos = new HashSet<CompeticaoFaseGrupo>()
            };

            CompeticaoFase competicaoFaseRepescagem = new CompeticaoFase()
            {
                CompeticaoFaseId = Guid.NewGuid(),
                CompeticaoId = competicao.CompeticaoId,
                Modo = CompeticaoFaseModo.Chaveamento,
                Nome = "1ª Fase Repescagem",
                Tipo = CompeticaoFaseTipo.Repescagem,
                CompeticoesFasesGrupos = new HashSet<CompeticaoFaseGrupo>()
            };

            var gruposPrincipal = SortearPrincipal(equipesPrincipal.OrderByDescending(a=>a.Aproveitamento).ToList());
            var gruposRepescagem = SortearRepescagem(equipesRepescagem.OrderByDescending(a=>a.Aproveitamento).ToList());

            gruposPrincipal = ReorganizaChaves(gruposPrincipal);
            gruposRepescagem = ReorganizaChaves(gruposRepescagem);

            foreach (var p in gruposPrincipal)
                competicaoFasePrincipal.CompeticoesFasesGrupos.Add(p);

            foreach (var r in gruposRepescagem)
                competicaoFaseRepescagem.CompeticoesFasesGrupos.Add(r);

            CompeticaoFaseGrupoJogos(competicaoFasePrincipal);
            CompeticaoFaseGrupoJogos(competicaoFaseRepescagem);

            db.CompeticoesFases.Add(competicaoFasePrincipal);
            db.CompeticoesFases.Add(competicaoFaseRepescagem);

            await db.SaveChangesAsync();

            return RedirectToAction("Indice");
        }

        private static List<CompeticaoFaseGrupo> SortearPrincipal(IList<CompeticaoEquipe> equipes)
        {
            int grupos3 = 0;
            int grupos4 = 0;
            int numeroEquipes = equipes.Count();
            while (numeroEquipes % 4 != 0)
            {
                grupos3++;
                numeroEquipes = numeroEquipes - 3;
            }

            grupos4 = numeroEquipes / 4;

            var chaves = grupos3 + grupos4;
            return SorteioPosicao(equipes, chaves);
        }

        private static List<CompeticaoFaseGrupo> SortearRepescagem(IList<CompeticaoEquipe> equipes)
        {
            int grupos3 = 0;
            int grupos4 = 0;
            int numeroEquipes = equipes.Count();
            while (numeroEquipes % 4 != 0)
            {
                grupos3++;
                numeroEquipes = numeroEquipes - 3;
            }

            grupos4 = numeroEquipes / 4;

            var chaves = grupos3 + grupos4;
            return SorteioPosicao(equipes, chaves);
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
