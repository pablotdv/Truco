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
                    case CompeticaoSorteioModo.Cidades: competicoesFasesGrupos = SorteioCidade(competicao, chaves); break;
                    case CompeticaoSorteioModo.Geral: competicoesFasesGrupos = SorteioGeral(competicao, chaves); break;
                    case CompeticaoSorteioModo.Regioes: competicoesFasesGrupos = SorteioRegiao(competicao, chaves); break;
                }

                var chaves4 = competicoesFasesGrupos.Where(a => a.CompeticoesFasesGruposEquipes.Count == 4);

                foreach (var chave4 in chaves4)
                {
                    competicaoFase.CompeticoesFasesGrupos.Add(chave4);
                }

                var chaves3 = competicoesFasesGrupos.Where(a => a.CompeticoesFasesGruposEquipes.Count == 3).ToList();

                if (chaves3.Count == 1)
                {
                    competicaoFase.CompeticoesFasesGrupos.Add(chaves3.FirstOrDefault());
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
                    competicaoFase.CompeticoesFasesGrupos.Add(chave6);
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
                    competicaoFase.CompeticoesFasesGrupos.Add(chave6);
                    competicaoFase.CompeticoesFasesGrupos.Add(chaves3[2]);

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

                    competicaoFase.CompeticoesFasesGrupos.Add(chave6A);
                    competicaoFase.CompeticoesFasesGrupos.Add(chave6B);
                }

                CompeticaoFaseGrupoJogos(competicaoFase);

                competicao.CompeticoesFases.Add(competicaoFase);
                competicao.Sorteada = true;
                await db.SaveChangesAsync();
                return RedirectToAction("Fase", new { id = competicaoFase.CompeticaoFaseId });
            }
            model.Competicao = competicao;
            return View(model);
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
                    new CompeticaoFaseGrupoRodadaJogo() { CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeUmId = equipeA1.CompeticaoFaseGrupoEquipeId, CompeticaoFaseGrupoEquipeDoisId = equipeB1.CompeticaoFaseGrupoEquipeId },
                    new CompeticaoFaseGrupoRodadaJogo() { CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeUmId = equipeA2.CompeticaoFaseGrupoEquipeId, CompeticaoFaseGrupoEquipeDoisId = equipeB2.CompeticaoFaseGrupoEquipeId },
                    new CompeticaoFaseGrupoRodadaJogo() { CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeUmId = equipeA3.CompeticaoFaseGrupoEquipeId, CompeticaoFaseGrupoEquipeDoisId = equipeB3.CompeticaoFaseGrupoEquipeId },
                }
            });

            grupo.CompeticoesFasesGruposRodadas.Add(new CompeticaoFaseGrupoRodada()
            {
                CompeticaoFaseGrupoRodadaId = Guid.NewGuid(),
                Rodada = 2,
                CompeticoesFasesGruposRodadasJogos = new HashSet<CompeticaoFaseGrupoRodadaJogo>()
                {
                    new CompeticaoFaseGrupoRodadaJogo() { CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeUmId = equipeA1.CompeticaoFaseGrupoEquipeId, CompeticaoFaseGrupoEquipeDoisId = equipeB2.CompeticaoFaseGrupoEquipeId },
                    new CompeticaoFaseGrupoRodadaJogo() { CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeUmId = equipeA2.CompeticaoFaseGrupoEquipeId, CompeticaoFaseGrupoEquipeDoisId = equipeB3.CompeticaoFaseGrupoEquipeId },
                    new CompeticaoFaseGrupoRodadaJogo() { CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeUmId = equipeA3.CompeticaoFaseGrupoEquipeId, CompeticaoFaseGrupoEquipeDoisId = equipeB1.CompeticaoFaseGrupoEquipeId },
                }
            });

            grupo.CompeticoesFasesGruposRodadas.Add(new CompeticaoFaseGrupoRodada()
            {
                CompeticaoFaseGrupoRodadaId = Guid.NewGuid(),
                Rodada = 3,
                CompeticoesFasesGruposRodadasJogos = new HashSet<CompeticaoFaseGrupoRodadaJogo>()
                {
                    new CompeticaoFaseGrupoRodadaJogo() { CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeUmId = equipeA1.CompeticaoFaseGrupoEquipeId, CompeticaoFaseGrupoEquipeDoisId = equipeB3.CompeticaoFaseGrupoEquipeId },
                    new CompeticaoFaseGrupoRodadaJogo() { CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeUmId = equipeA2.CompeticaoFaseGrupoEquipeId, CompeticaoFaseGrupoEquipeDoisId = equipeB1.CompeticaoFaseGrupoEquipeId },
                    new CompeticaoFaseGrupoRodadaJogo() { CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeUmId = equipeA3.CompeticaoFaseGrupoEquipeId, CompeticaoFaseGrupoEquipeDoisId = equipeB2.CompeticaoFaseGrupoEquipeId },
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
                    new CompeticaoFaseGrupoRodadaJogo() { CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeUmId = equipe1.CompeticaoFaseGrupoEquipeId, CompeticaoFaseGrupoEquipeDoisId = equipe2.CompeticaoFaseGrupoEquipeId },
                }
            });
            grupo.CompeticoesFasesGruposRodadas.Add(new CompeticaoFaseGrupoRodada()
            {
                CompeticaoFaseGrupoRodadaId = Guid.NewGuid(),
                Rodada = 2,
                CompeticoesFasesGruposRodadasJogos = new HashSet<CompeticaoFaseGrupoRodadaJogo>()
                {
                    new CompeticaoFaseGrupoRodadaJogo() { CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeUmId = equipe1.CompeticaoFaseGrupoEquipeId, CompeticaoFaseGrupoEquipeDoisId = equipe3.CompeticaoFaseGrupoEquipeId },
                }
            });
            grupo.CompeticoesFasesGruposRodadas.Add(new CompeticaoFaseGrupoRodada()
            {
                CompeticaoFaseGrupoRodadaId = Guid.NewGuid(),
                Rodada = 3,
                CompeticoesFasesGruposRodadasJogos = new HashSet<CompeticaoFaseGrupoRodadaJogo>()
                {
                    new CompeticaoFaseGrupoRodadaJogo() { CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeUmId = equipe2.CompeticaoFaseGrupoEquipeId, CompeticaoFaseGrupoEquipeDoisId = equipe3.CompeticaoFaseGrupoEquipeId },
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
                    new CompeticaoFaseGrupoRodadaJogo() { CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeUmId = equipe1.CompeticaoFaseGrupoEquipeId, CompeticaoFaseGrupoEquipeDoisId = equipe2.CompeticaoFaseGrupoEquipeId },
                    new CompeticaoFaseGrupoRodadaJogo() { CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeUmId = equipe3.CompeticaoFaseGrupoEquipeId, CompeticaoFaseGrupoEquipeDoisId = equipe4.CompeticaoFaseGrupoEquipeId },
                }
            });
            grupo.CompeticoesFasesGruposRodadas.Add(new CompeticaoFaseGrupoRodada()
            {
                CompeticaoFaseGrupoRodadaId = Guid.NewGuid(),
                Rodada = 2,
                CompeticoesFasesGruposRodadasJogos = new HashSet<CompeticaoFaseGrupoRodadaJogo>()
                {
                    new CompeticaoFaseGrupoRodadaJogo() { CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeUmId = equipe1.CompeticaoFaseGrupoEquipeId, CompeticaoFaseGrupoEquipeDoisId = equipe3.CompeticaoFaseGrupoEquipeId },
                    new CompeticaoFaseGrupoRodadaJogo() { CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeUmId = equipe4.CompeticaoFaseGrupoEquipeId, CompeticaoFaseGrupoEquipeDoisId = equipe2.CompeticaoFaseGrupoEquipeId },
                }
            });
            grupo.CompeticoesFasesGruposRodadas.Add(new CompeticaoFaseGrupoRodada()
            {
                CompeticaoFaseGrupoRodadaId = Guid.NewGuid(),
                Rodada = 3,
                CompeticoesFasesGruposRodadasJogos = new HashSet<CompeticaoFaseGrupoRodadaJogo>()
                {
                    new CompeticaoFaseGrupoRodadaJogo() { CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeUmId = equipe1.CompeticaoFaseGrupoEquipeId, CompeticaoFaseGrupoEquipeDoisId = equipe4.CompeticaoFaseGrupoEquipeId },
                    new CompeticaoFaseGrupoRodadaJogo() { CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(), CompeticaoFaseGrupoEquipeUmId = equipe2.CompeticaoFaseGrupoEquipeId, CompeticaoFaseGrupoEquipeDoisId = equipe3.CompeticaoFaseGrupoEquipeId },
                }
            });
        }

        private static List<CompeticaoFaseGrupo> SorteioGeral(Competicao competicao, int chaves)
        {
            List<CompeticaoFaseGrupo> competicoesFasesGrupos = MontarChaveamento(chaves);

            var equipes = competicao.CompeticoesEquipes.OrderByDescending(a => Guid.NewGuid());

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

        private static List<CompeticaoFaseGrupo> SorteioRegiao(Competicao competicao, int chaves)
        {
            List<CompeticaoFaseGrupo> competicoesFasesGrupos = MontarChaveamento(chaves);

            var equipesRegioes = competicao.CompeticoesEquipes.GroupBy(a => a.Equipe.RegiaoId).OrderByDescending(a => a.Count());

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

        private static List<CompeticaoFaseGrupo> SorteioCidade(Competicao competicao, int chaves)
        {
            List<CompeticaoFaseGrupo> competicoesFasesGrupos = MontarChaveamento(chaves);

            var equipesCidades = competicao.CompeticoesEquipes.GroupBy(a => a.Equipe.CidadeId).OrderByDescending(a => a.Count());

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
                .ThenBy(a => a.CompeticaoFaseGrupoEquipeUm.Numero)
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
                .Include(a => a.CompeticoesFasesGruposRodadasJogosSets)
                .FirstOrDefaultAsync(a => a.CompeticaoFaseGrupoRodadaJogoId == id);

            if (jogo == null)
            {
                return HttpNotFound();
            }

            if (jogo.CompeticoesFasesGruposRodadasJogosSets.Count == 0)
            {
                jogo.CompeticoesFasesGruposRodadasJogosSets.Add(new CompeticaoFaseGrupoRodadaJogoSet()
                {
                    CompeticaoFaseGrupoRodadaJogoSetId = Guid.NewGuid(),
                    Set = 1
                });

                jogo.CompeticoesFasesGruposRodadasJogosSets.Add(new CompeticaoFaseGrupoRodadaJogoSet()
                {
                    CompeticaoFaseGrupoRodadaJogoSetId = Guid.NewGuid(),
                    Set = 2
                });

                jogo.CompeticoesFasesGruposRodadasJogosSets.Add(new CompeticaoFaseGrupoRodadaJogoSet()
                {
                    CompeticaoFaseGrupoRodadaJogoSetId = Guid.NewGuid(),
                    Set = 3
                });
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
                    .Include(a => a.CompeticoesFasesGruposRodadasJogosSets)
                    .FirstOrDefaultAsync(a => a.CompeticaoFaseGrupoRodadaJogoId == model.CompeticaoFaseGrupoRodadaJogoId);

                if (jogo.CompeticoesFasesGruposRodadasJogosSets.Count != 0)
                {
                    var sets = jogo.CompeticoesFasesGruposRodadasJogosSets.ToList();
                    foreach (var set in sets)
                    {
                        db.CompeticoesFasesGruposRodadasJogosSets.Remove(set);
                        jogo.CompeticoesFasesGruposRodadasJogosSets.Remove(set);
                    }
                }

                foreach (var set in model.CompeticoesFasesGruposRodadasJogosSets)
                {
                    jogo.CompeticoesFasesGruposRodadasJogosSets.Add(new CompeticaoFaseGrupoRodadaJogoSet()
                    {
                        CompeticaoFaseGrupoRodadaJogoSetId = Guid.NewGuid(),
                        Set = set.Set,
                        EquipeUmTentos = set.EquipeUmTentos,
                        EquipeDoisTentos = set.EquipeDoisTentos,
                    });
                }

                await db.SaveChangesAsync();

                //var equipeUm = jogo.CompeticaoFaseGrupoEquipeUm;
                //var jogosEquipeUm = db.CompeticoesFasesGruposRodadasJogos
                //    .Include(a => a.CompeticaoFaseGrupoEquipeUm)
                //    .Include(a => a.CompeticaoFaseGrupoEquipeDois)
                //    .Where(a => a.CompeticaoFaseGrupoEquipeUm.CompeticaoFaseGrupoEquipeId == equipeUm.CompeticaoFaseGrupoEquipeId || a.CompeticaoFaseGrupoEquipeDois.CompeticaoFaseGrupoEquipeId == equipeUm.CompeticaoFaseGrupoEquipeId)
                //    .ToList();

                //var equipeDois = jogo.CompeticaoFaseGrupoEquipeDois;
                //var jogosEquipeDois = db.CompeticoesFasesGruposRodadasJogos
                //    .Include(a => a.CompeticaoFaseGrupoEquipeUm)
                //    .Include(a => a.CompeticaoFaseGrupoEquipeDois)
                //    .Where(a => a.CompeticaoFaseGrupoEquipeUm.CompeticaoFaseGrupoEquipeId == equipeDois.CompeticaoFaseGrupoEquipeId || a.CompeticaoFaseGrupoEquipeDois.CompeticaoFaseGrupoEquipeId == equipeDois.CompeticaoFaseGrupoEquipeId)
                //    .ToList();

                //await db.SaveChangesAsync();

                return RedirectToAction("Jogos", new { id = jogo.CompeticaoFaseGrupoRodada.CompeticaoFaseGrupoId });
            }
            return View(model);
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
