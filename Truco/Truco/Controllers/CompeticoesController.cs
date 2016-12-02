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
using Truco.Infraestrutura;

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
            Competicao competicao = await db.Competicoes
                .Include(a => a.CompeticoesEquipes)
                .FirstOrDefaultAsync(a => a.CompeticaoId == id);
            if (competicao == null)
            {
                return HttpNotFound();
            }


            return View(new CompeticaoSorteioViewModel
            {
                Competicao = competicao,
                SorteioModo = CompeticaoSorteioModo.Cidades,
                EquipesPorChave = 4
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

                while (numeroEquipes % model.EquipesPorChave != 0)
                {
                    grupos3++;
                    numeroEquipes = numeroEquipes - (model.EquipesPorChave - 1);
                }

                grupos4 = numeroEquipes / model.EquipesPorChave;

                var chaves = grupos3 + grupos4;

                var competicaoFase = new CompeticaoFase()
                {
                    CompeticaoFaseId = Guid.NewGuid(),
                    CompeticaoId = competicao.CompeticaoId,
                    Modo = CompeticaoFaseModo.Chaveamento,
                    Tipo = CompeticaoFaseTipo.Principal,
                    Fase = competicao.CompeticoesFases.Where(a => a.Tipo == CompeticaoFaseTipo.Principal).Count() + 1,
                    CompeticoesFasesGrupos = new HashSet<CompeticaoFaseGrupo>()

                };
                List<CompeticaoFaseGrupo> competicoesFasesGrupos = null;
                switch (model.SorteioModo)
                {
                    case CompeticaoSorteioModo.Cidades: competicoesFasesGrupos = SorteioCidade(competicao.CompeticoesEquipes.ToList(), chaves); break;
                    case CompeticaoSorteioModo.Geral: competicoesFasesGrupos = SorteioGeral(competicao.CompeticoesEquipes.ToList(), chaves); break;
                    case CompeticaoSorteioModo.Regioes: competicoesFasesGrupos = SorteioRegiao(competicao.CompeticoesEquipes.ToList(), chaves); break;
                }

                foreach (var grupo in ReorganizaChaves(competicoesFasesGrupos, model.EquipesPorChave))
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

        private List<CompeticaoFaseGrupo> ReorganizaChaves(List<CompeticaoFaseGrupo> competicoesFasesGrupos, int equipesPorChaves)
        {
            List<CompeticaoFaseGrupo> grupos = new List<CompeticaoFaseGrupo>();
            var chaves4 = competicoesFasesGrupos.Where(a => a.CompeticoesFasesGruposEquipes.Count == equipesPorChaves);

            foreach (var chave4 in chaves4)
            {
                grupos.Add(chave4);
            }

            var chaves3 = competicoesFasesGrupos.Where(a => a.CompeticoesFasesGruposEquipes.Count == equipesPorChaves - 1).ToList();

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
                        CompeticaoEquipeId = e.CompeticaoEquipeId,
                        Lado = Lado.LadoA,
                        Numero = e.Numero,
                    });
                }

                foreach (var e in chaves3[1].CompeticoesFasesGruposEquipes)
                {
                    chave6.CompeticoesFasesGruposEquipes.Add(new CompeticaoFaseGrupoEquipe()
                    {
                        CompeticaoFaseGrupoEquipeId = Guid.NewGuid(),
                        CompeticaoEquipeId = e.CompeticaoEquipeId,
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
                        CompeticaoEquipeId = e.CompeticaoEquipeId,
                        Lado = Lado.LadoA,
                        Numero = e.Numero,
                    });
                }

                foreach (var e in chaves3[1].CompeticoesFasesGruposEquipes)
                {
                    chave6.CompeticoesFasesGruposEquipes.Add(new CompeticaoFaseGrupoEquipe()
                    {
                        CompeticaoFaseGrupoEquipeId = Guid.NewGuid(),
                        CompeticaoEquipeId = e.CompeticaoEquipeId,
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
                        CompeticaoEquipeId = e.CompeticaoEquipeId,
                        Lado = Lado.LadoA,
                        Numero = e.Numero,
                    });
                }

                foreach (var e in chaves3[1].CompeticoesFasesGruposEquipes)
                {
                    chave6A.CompeticoesFasesGruposEquipes.Add(new CompeticaoFaseGrupoEquipe()
                    {
                        CompeticaoFaseGrupoEquipeId = Guid.NewGuid(),
                        CompeticaoEquipeId = e.CompeticaoEquipeId,
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
                        CompeticaoEquipeId = e.CompeticaoEquipeId,
                        Lado = Lado.LadoA,
                        Numero = e.Numero,
                    });
                }

                foreach (var e in chaves3[3].CompeticoesFasesGruposEquipes)
                {
                    chave6B.CompeticoesFasesGruposEquipes.Add(new CompeticaoFaseGrupoEquipe()
                    {
                        CompeticaoFaseGrupoEquipeId = Guid.NewGuid(),
                        CompeticaoEquipeId = e.CompeticaoEquipeId,
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
                else CompeticaoFaseGrupoJogosChave(grupo);
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

        private static void CompeticaoFaseGrupoJogosChave(CompeticaoFaseGrupo grupo)
        {
            var equipes = grupo.CompeticoesFasesGruposEquipes.ToList();
            RoundRobin roundRobin = new RoundRobin();

            int num_teams = equipes.Count();
            int[,] results = roundRobin.GenerateRoundRobin(num_teams);

            for (int round = 0; round <= results.GetUpperBound(1); round++)
            {
                var rodada = new CompeticaoFaseGrupoRodada()
                {
                    CompeticaoFaseGrupoRodadaId = Guid.NewGuid(),
                    Rodada = round + 1,
                    CompeticoesFasesGruposRodadasJogos = new HashSet<CompeticaoFaseGrupoRodadaJogo>()
                };
                for (int team = 0; team < num_teams; team++)
                {
                    if (team < results[team, round])
                    {
                        rodada.CompeticoesFasesGruposRodadasJogos.Add(new CompeticaoFaseGrupoRodadaJogo()
                        {
                            CompeticaoFaseGrupoRodadaJogoId = Guid.NewGuid(),
                            CompeticaoFaseGrupoRodadaJogoEquipeUm = new CompeticaoFaseGrupoRodadaJogoEquipe()
                            {
                                CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(),
                                CompeticaoFaseGrupoEquipeId = equipes[team].CompeticaoFaseGrupoEquipeId
                            },
                            CompeticaoFaseGrupoRodadaJogoEquipeDois = new CompeticaoFaseGrupoRodadaJogoEquipe()
                            {
                                CompeticaoFaseGrupoRodadaJogoEquipeId = Guid.NewGuid(),
                                CompeticaoFaseGrupoEquipeId = equipes[results[team, round]].CompeticaoFaseGrupoEquipeId
                            },
                        });
                    }
                }

                grupo.CompeticoesFasesGruposRodadas.Add(rodada);
            }
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
                    CompeticaoEquipeId = equipe.CompeticaoEquipeId,
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
                    CompeticaoEquipeId = equipe.CompeticaoEquipeId,
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

            var equipesRegioes = competicaoEquipes.GroupBy(a => a.RegiaoId).OrderByDescending(a => a.Count());

            int indiceChave = 1;

            foreach (var equipesRegiao in equipesRegioes)
            {
                var equipes = equipesRegiao.OrderByDescending(a => Guid.NewGuid());
                foreach (var equipe in equipes)
                {
                    competicoesFasesGrupos[indiceChave - 1].CompeticoesFasesGruposEquipes.Add(new CompeticaoFaseGrupoEquipe()
                    {
                        CompeticaoFaseGrupoEquipeId = Guid.NewGuid(),
                        CompeticaoEquipeId = equipe.CompeticaoEquipeId,
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

            var equipesCidades = competicaoEquipes.GroupBy(a => a.CidadeId).OrderByDescending(a => a.Count());

            int indiceChave = 1;

            foreach (var equipesCidade in equipesCidades)
            {
                var equipes = equipesCidade.OrderByDescending(a => Guid.NewGuid());
                foreach (var equipe in equipes)
                {
                    competicoesFasesGrupos[indiceChave - 1].CompeticoesFasesGruposEquipes.Add(new CompeticaoFaseGrupoEquipe()
                    {
                        CompeticaoFaseGrupoEquipeId = Guid.NewGuid(),
                        CompeticaoEquipeId = equipe.CompeticaoEquipeId,
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
            var competicaoFaseGrupos = await db.CompeticoesFases
                .Include(a => a.CompeticoesFasesGrupos.Select(b => b.CompeticoesFasesGruposEquipes))
                .Where(a => a.CompeticaoFaseId == id).FirstOrDefaultAsync();

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
            int sets = 3;
            if (jogo.CompeticaoFaseGrupoRodada.CompeticaoFaseGrupo.CompeticaoFase.Tipo == CompeticaoFaseTipo.Repescagem)
                sets = 1;
            if (jogo.CompeticaoFaseGrupoRodadaJogoEquipeUm.CompeticoesFasesGruposRodadasJogosEquipesSets.Count == 0)
            {
                for (int i = 1; i <= sets; i++)
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
                for (int i = 1; i <= sets; i++)
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

                var setsParavitoria = 2;
                if (jogo.CompeticaoFaseGrupoRodada.CompeticaoFaseGrupo.CompeticaoFase.Tipo == CompeticaoFaseTipo.Repescagem)
                    setsParavitoria = 1;

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
                    .Where(a => a.Count() >= setsParavitoria)
                    .Count();
                jogo.CompeticaoFaseGrupoRodadaJogoEquipeDois.CompeticaoFaseGrupoEquipe.Vitorias = jogosEquipeDois
                    .Where(a => a.Tentos == 24)
                    .GroupBy(a => a.CompeticaoFaseGrupoRodadaJogoEquipe.CompeticaoFaseGrupoRodadaJogoEquipeId)
                    .Where(a => a.Count() >= setsParavitoria)
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

                var setsJogados = 3;
                if (jogo.CompeticaoFaseGrupoRodada.CompeticaoFaseGrupo.CompeticaoFase.Tipo == CompeticaoFaseTipo.Repescagem)
                    setsJogados = 1;

                var equipe = jogo.CompeticaoFaseGrupoRodadaJogoEquipeUm.CompeticaoFaseGrupoEquipe;
                double vitorias = (double)(equipe.Vitorias * 100) / equipe.Jogos;
                double setes = (double)(equipe.Sets * 100) / (equipe.Jogos * setsJogados);
                double tentos = (double)(equipe.Tentos * 100) / (equipe.Jogos * 24 * setsJogados);
                double aproveitamento = (double)(vitorias + setes + tentos) / 3;
                jogo.CompeticaoFaseGrupoRodadaJogoEquipeUm.CompeticaoFaseGrupoEquipe.Aproveitamento = (decimal)aproveitamento;

                equipe = jogo.CompeticaoFaseGrupoRodadaJogoEquipeDois.CompeticaoFaseGrupoEquipe;
                vitorias = (double)(equipe.Vitorias * 100) / equipe.Jogos;
                setes = (double)(equipe.Sets * 100) / (equipe.Jogos * setsJogados);
                tentos = (double)(equipe.Tentos * 100) / (equipe.Jogos * 24 * setsJogados);
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

            ClassificacaoViewModel model = null;
            model = await Classificar(id, null);
            model.Modo = CompeticaoFaseModo.Chaveamento;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Classificar(ClassificacaoViewModel model)
        {
            model = await Classificar(model.CompeticaoFaseId, model.Principal);

            return View("Classificacao", model);
        }

        private async Task<ClassificacaoViewModel> Classificar(Guid id, int? principal)
        {

            var grupos = await db.CompeticoesFasesGrupos
                             .Include(a => a.CompeticoesFasesGruposEquipes)
                             .OrderBy(a => a.Grupo)
                             .Where(a => a.CompeticaoFaseId == id)
                             .ToListAsync();


            ClassificacaoViewModel model = new ClassificacaoViewModel()
            {
                CompeticaoFaseId = id,
                CompeticaoFase = grupos.FirstOrDefault().CompeticaoFase,
                Equipes = new List<ClassificacaoEquipeViewModel>()
            };

            var totalGrupos3 = grupos.Where(a => a.CompeticoesFasesGruposEquipes.Count() == 3).Count();
            var totalGrupos4 = grupos.Where(a => a.CompeticoesFasesGruposEquipes.Count() == 4).Count();
            var totalGrupos5 = grupos.Where(a => a.CompeticoesFasesGruposEquipes.Count() == 5).Count();
            var totalGrupos6 = grupos.Where(a => a.CompeticoesFasesGruposEquipes.Count() == 6).Count();
            var totalGrupos = totalGrupos3 + totalGrupos4 + totalGrupos5 + (totalGrupos6 * 2);

            if (!principal.HasValue)
            {
                principal = totalGrupos * 2;
            }

            var terceiros = new List<CompeticaoFaseGrupoEquipe>();
            //classifica os dois primeiros de cada chave e mais 4 da chave de 3X3
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

                    var classificacao = ViewModels.Enums.Classificacao.Principal;

                    if (((numeroEquipes == 4 || numeroEquipes == 3) && posicao > 2) || (numeroEquipes == 6 && posicao > 4))
                        classificacao = ViewModels.Enums.Classificacao.Repescagem;

                    if ((numeroEquipes == 3 || numeroEquipes == 4) && posicao == 3)
                        terceiros.Add(equipe);

                    model.Equipes.Add(new ClassificacaoEquipeViewModel()
                    {
                        Posicao = posicao,
                        CompeticaoFaseGrupoEquipe = equipe,
                        Classificacao = classificacao,
                        Aproveitamento = equipe.Aproveitamento,
                        CompeticaoEquipeId = equipe.CompeticaoEquipeId,
                        CompeticaoEquipe = equipe.CompeticaoEquipe,
                    });
                }
            }

            if (model.Equipes.Where(a => a.Classificacao == ViewModels.Enums.Classificacao.Principal).Count() < principal)
            {
                terceiros = terceiros.OrderByDescending(a => a.Aproveitamento).ThenBy(a => a.CompeticaoEquipe.Nome).ToList();
                int classificadosPrincipal = model.Equipes.Where(a => a.Classificacao == ViewModels.Enums.Classificacao.Principal).Count();
                foreach (var t in terceiros)
                {
                    var classificacao = ViewModels.Enums.Classificacao.Principal;
                    if (terceiros.IndexOf(t) >= (principal - (classificadosPrincipal)))
                        classificacao = ViewModels.Enums.Classificacao.Repescagem;
                    else
                    {
                        var terceiro = model.Equipes.Where(a => a.CompeticaoEquipeId == t.CompeticaoEquipeId).First();
                        model.Equipes.Remove(terceiro);
                    }

                    model.Equipes.Add(new ClassificacaoEquipeViewModel()
                    {
                        Posicao = 3,
                        CompeticaoFaseGrupoEquipe = t,
                        Classificacao = classificacao,
                        Aproveitamento = t.Aproveitamento,
                        CompeticaoEquipeId = t.CompeticaoEquipeId,
                        CompeticaoEquipe = t.CompeticaoEquipe,
                    });
                }

            }
            model.Principal = model.Equipes.Where(a => a.Classificacao == ViewModels.Enums.Classificacao.Principal).Count();

            return model;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Classificacao(ClassificacaoViewModel model)
        {

            if (model.Modo == CompeticaoFaseModo.Chaveamento)
            {
                return await ClassificarGrupos(model);
            }
            else
            {
                return await ClassificarMataMata(model);
            }

            //return RedirectToAction("Indice");
        }

        private async Task<ActionResult> ClassificarMataMata(ClassificacaoViewModel model)
        {
            var classificar = await Classificar(model.CompeticaoFaseId, model.Principal);

            var competicao = classificar.CompeticaoFase.Competicao;

            CompeticaoFase fase = new CompeticaoFase()
            {
                CompeticaoFaseId = Guid.NewGuid(),
                CompeticaoId = competicao.CompeticaoId,
                Tipo = classificar.CompeticaoFase.Tipo,
                Fase = competicao.CompeticoesFases.Where(a => a.Tipo == classificar.CompeticaoFase.Tipo).Count() + 1,
                Modo = CompeticaoFaseModo.MataMata,
                CompeticoesFasesJogos = new HashSet<CompeticaoFaseJogo>()
            };

            if (classificar.CompeticaoFase.Tipo == CompeticaoFaseTipo.Repescagem)
            {
                fase.CompeticaoFasePrincipalId = classificar.CompeticaoFase.CompeticaoFasePrincipalId;
            }

            var equipesPrincipal = classificar.Equipes
                .Where(a => a.Classificacao == ViewModels.Enums.Classificacao.Principal)
                .OrderByDescending(a => a.Aproveitamento)
                .ToList();

            int equipes = equipesPrincipal.Count();
            int jogos = equipes / 2;

            int i = 0;
            int j = equipes - 1;
            for (int jogo = 0; jogo < jogos; jogo++)
            {
                fase.CompeticoesFasesJogos.Add(new CompeticaoFaseJogo()
                {
                    CompeticaoFaseJogoId = Guid.NewGuid(),
                    Jogo = jogo + 1,
                    CompeticaoFaseJogoEquipeUm = new CompeticaoFaseJogoEquipe()
                    {
                        CompeticaoFaseJogoEquipeId = Guid.NewGuid(),
                        CompeticaoFaseEquipe = new CompeticaoFaseEquipe()
                        {
                            CompeticaoFaseEquipeId = Guid.NewGuid(),
                            CompeticaoFaseId = fase.CompeticaoFaseId,
                            CompeticaoEquipeId = equipesPrincipal[i].CompeticaoFaseGrupoEquipe.CompeticaoEquipeId
                        }
                    },
                    CompeticaoFaseJogoEquipeDois = new CompeticaoFaseJogoEquipe()
                    {
                        CompeticaoFaseJogoEquipeId = Guid.NewGuid(),
                        CompeticaoFaseEquipe = new CompeticaoFaseEquipe()
                        {
                            CompeticaoFaseEquipeId = Guid.NewGuid(),
                            CompeticaoFaseId = fase.CompeticaoFaseId,
                            CompeticaoEquipeId = equipesPrincipal[j].CompeticaoFaseGrupoEquipe.CompeticaoEquipeId
                        }
                    },
                });
                i++;
                j--;
            }

            db.CompeticoesFases.Add(fase);

            await db.SaveChangesAsync();

            return RedirectToAction("FaseMataMata", new { id = fase.CompeticaoFaseId });
        }

        public async Task<ActionResult> FaseMataMata(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var competicaoFase = await db.CompeticoesFases
                .Include(a => a.CompeticoesFasesJogos)
                .FirstOrDefaultAsync(a => a.CompeticaoFaseId == id);

            if (competicaoFase == null)
            {
                return HttpNotFound();
            }

            return View(competicaoFase);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("FaseMataMata")]
        public async Task<ActionResult> FaseMataMataConfirmacao(Guid id)
        {
            var competicaoFase = await db.CompeticoesFasesJogosEquipes
                .Where(a => a.CompeticaoFaseEquipe.CompeticaoFaseId == id)
                .ToListAsync();

            if (competicaoFase == null)
            {
                return HttpNotFound();
            }

            return View(competicaoFase);
        }

        private async Task<ActionResult> ClassificarGrupos(ClassificacaoViewModel model)
        {
            var classificar = await Classificar(model.CompeticaoFaseId, model.Principal);

            IList<CompeticaoEquipe> equipesPrincipal = new List<CompeticaoEquipe>();
            IList<CompeticaoEquipe> equipesRepescagem = new List<CompeticaoEquipe>();

            var equipesClassificacoes = classificar.Equipes.ToList();

            foreach (var e in equipesClassificacoes)
            {
                if (e.Classificacao == ViewModels.Enums.Classificacao.Principal)
                {
                    equipesPrincipal.Add(e.CompeticaoEquipe);
                }
                else if (e.Classificacao == ViewModels.Enums.Classificacao.Repescagem)
                {
                    equipesRepescagem.Add(e.CompeticaoEquipe);
                }
            }

            var competicao = classificar.CompeticaoFase.Competicao;

            CompeticaoFase competicaoFasePrincipal = new CompeticaoFase()
            {
                CompeticaoFaseId = Guid.NewGuid(),
                CompeticaoId = competicao.CompeticaoId,
                Modo = CompeticaoFaseModo.Chaveamento,
                Fase = competicao.CompeticoesFases.Where(a => a.Tipo == CompeticaoFaseTipo.Principal).Count() + 1,
                Tipo = CompeticaoFaseTipo.Principal,
                CompeticoesFasesGrupos = new HashSet<CompeticaoFaseGrupo>()
            };

            CompeticaoFase competicaoFaseRepescagem = new CompeticaoFase()
            {
                CompeticaoFaseId = Guid.NewGuid(),
                CompeticaoFasePrincipalId = competicaoFasePrincipal.CompeticaoFaseId,
                CompeticaoId = competicao.CompeticaoId,
                Modo = CompeticaoFaseModo.Chaveamento,
                Fase = competicao.CompeticoesFases.Where(a => a.Tipo == CompeticaoFaseTipo.Repescagem).Count() + 1,
                Tipo = CompeticaoFaseTipo.Repescagem,
                CompeticoesFasesGrupos = new HashSet<CompeticaoFaseGrupo>()
            };

            var gruposPrincipal = SortearPrincipal(equipesPrincipal.OrderByDescending(a => a.Aproveitamento).ToList());
            var gruposRepescagem = SortearRepescagem(equipesRepescagem.OrderByDescending(a => a.Aproveitamento).ToList());

            gruposPrincipal = ReorganizaChaves(gruposPrincipal, 4);
            gruposRepescagem = ReorganizaChaves(gruposRepescagem, 4);

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



        public async Task<ActionResult> JogoMataMata(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var jogo = await db.CompeticoesFasesJogos
                .Include(a => a.CompeticaoFaseJogoEquipeUm.CompeticoesFasesJogosEquipesSets)
                .Include(a => a.CompeticaoFaseJogoEquipeDois.CompeticoesFasesJogosEquipesSets)
                .FirstOrDefaultAsync(a => a.CompeticaoFaseJogoId == id);

            if (jogo == null)
            {
                return HttpNotFound();
            }
            int sets = 3;
            if (jogo.CompeticaoFase.Tipo == CompeticaoFaseTipo.Repescagem)
                sets = 1;
            if (jogo.CompeticaoFaseJogoEquipeUm.CompeticoesFasesJogosEquipesSets.Count == 0)
            {
                for (int i = 1; i <= sets; i++)
                {
                    jogo.CompeticaoFaseJogoEquipeUm.CompeticoesFasesJogosEquipesSets.Add(new CompeticaoFaseJogoEquipeSet()
                    {
                        CompeticaoFaseJogoEquipeSetId = Guid.NewGuid(),
                        Set = i
                    });
                }
            }

            if (jogo.CompeticaoFaseJogoEquipeDois.CompeticoesFasesJogosEquipesSets.Count == 0)
            {
                for (int i = 1; i <= sets; i++)
                {
                    jogo.CompeticaoFaseJogoEquipeDois.CompeticoesFasesJogosEquipesSets.Add(new CompeticaoFaseJogoEquipeSet()
                    {
                        CompeticaoFaseJogoEquipeSetId = Guid.NewGuid(),
                        Set = i
                    });
                }
            }

            return View(jogo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> JogoMataMata(CompeticaoFaseJogo model)
        {
            if (ModelState.IsValid)
            {
                var jogo = await db.CompeticoesFasesJogos
                    .Include(a => a.CompeticaoFaseJogoEquipeUm.CompeticoesFasesJogosEquipesSets)
                    .Include(a => a.CompeticaoFaseJogoEquipeDois.CompeticoesFasesJogosEquipesSets)
                    .FirstOrDefaultAsync(a => a.CompeticaoFaseJogoId == model.CompeticaoFaseJogoId);

                if (jogo.CompeticaoFaseJogoEquipeUm.CompeticoesFasesJogosEquipesSets.Count != 0)
                {
                    var sets = jogo.CompeticaoFaseJogoEquipeUm.CompeticoesFasesJogosEquipesSets.ToList();
                    foreach (var set in sets)
                    {
                        db.CompeticoesFasesJogosEquipesSets.Remove(set);
                        jogo.CompeticaoFaseJogoEquipeUm.CompeticoesFasesJogosEquipesSets.Remove(set);
                    }
                }

                if (jogo.CompeticaoFaseJogoEquipeDois.CompeticoesFasesJogosEquipesSets.Count != 0)
                {
                    var sets = jogo.CompeticaoFaseJogoEquipeDois.CompeticoesFasesJogosEquipesSets.ToList();
                    foreach (var set in sets)
                    {
                        db.CompeticoesFasesJogosEquipesSets.Remove(set);
                        jogo.CompeticaoFaseJogoEquipeDois.CompeticoesFasesJogosEquipesSets.Remove(set);
                    }
                }

                foreach (var set in model.CompeticaoFaseJogoEquipeUm.CompeticoesFasesJogosEquipesSets)
                {
                    jogo.CompeticaoFaseJogoEquipeUm.CompeticoesFasesJogosEquipesSets.Add(new CompeticaoFaseJogoEquipeSet()
                    {
                        CompeticaoFaseJogoEquipeSetId = Guid.NewGuid(),
                        Set = set.Set,
                        Tentos = set.Tentos
                    });
                }

                foreach (var set in model.CompeticaoFaseJogoEquipeDois.CompeticoesFasesJogosEquipesSets)
                {
                    jogo.CompeticaoFaseJogoEquipeDois.CompeticoesFasesJogosEquipesSets.Add(new CompeticaoFaseJogoEquipeSet()
                    {
                        CompeticaoFaseJogoEquipeSetId = Guid.NewGuid(),
                        Set = set.Set,
                        Tentos = set.Tentos
                    });
                }

                await db.SaveChangesAsync();

                var setsParavitoria = 2;
                if (jogo.CompeticaoFase.Tipo == CompeticaoFaseTipo.Repescagem)
                    setsParavitoria = 1;

                var jogosEquipeUm = db.CompeticoesFasesJogosEquipesSets
                    .Where(a => a.CompeticaoFaseJogoEquipe.CompeticaoFaseEquipeId == jogo.CompeticaoFaseJogoEquipeUm.CompeticaoFaseEquipeId);

                var jogosEquipeDois = db.CompeticoesFasesJogosEquipesSets
                    .Where(a => a.CompeticaoFaseJogoEquipe.CompeticaoFaseEquipeId == jogo.CompeticaoFaseJogoEquipeDois.CompeticaoFaseEquipeId);

                jogo.CompeticaoFaseJogoEquipeUm.CompeticaoFaseEquipe.Jogos = jogosEquipeUm
                    .GroupBy(a => a.CompeticaoFaseJogoEquipe.CompeticaoFaseJogoEquipeId)
                    .Count();
                jogo.CompeticaoFaseJogoEquipeDois.CompeticaoFaseEquipe.Jogos = jogosEquipeDois
                    .GroupBy(a => a.CompeticaoFaseJogoEquipe.CompeticaoFaseJogoEquipeId)
                    .Count();

                jogo.CompeticaoFaseJogoEquipeUm.CompeticaoFaseEquipe.Vitorias = jogosEquipeUm
                    .Where(a => a.Tentos == 24)
                    .GroupBy(a => a.CompeticaoFaseJogoEquipe.CompeticaoFaseJogoEquipeId)
                    .Where(a => a.Count() >= setsParavitoria)
                    .Count();
                jogo.CompeticaoFaseJogoEquipeDois.CompeticaoFaseEquipe.Vitorias = jogosEquipeDois
                    .Where(a => a.Tentos == 24)
                    .GroupBy(a => a.CompeticaoFaseJogoEquipe.CompeticaoFaseJogoEquipeId)
                    .Where(a => a.Count() >= setsParavitoria)
                    .Count();

                jogo.CompeticaoFaseJogoEquipeUm.CompeticaoFaseEquipe.Sets = jogosEquipeUm
                    .Where(a => a.Tentos == 24)
                    .Count();
                jogo.CompeticaoFaseJogoEquipeDois.CompeticaoFaseEquipe.Sets = jogosEquipeDois
                    .Where(a => a.Tentos == 24)
                    .Count();

                jogo.CompeticaoFaseJogoEquipeUm.CompeticaoFaseEquipe.Tentos = jogosEquipeUm
                    .Sum(a => a.Tentos);
                jogo.CompeticaoFaseJogoEquipeDois.CompeticaoFaseEquipe.Tentos = jogosEquipeDois
                    .Sum(a => a.Tentos);

                await db.SaveChangesAsync();

                return RedirectToAction("FaseMataMata", new { id = jogo.CompeticaoFase.CompeticaoFaseId });
            }
            return View(model);
        }


        //CADASTRO DE EQUIPES E VINCULO COM COMPETIÇÃO
        public async Task<ActionResult> CompeticaoEquipe(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var competicao = await db.Competicoes.Include(a => a.CompeticoesEquipes).FirstOrDefaultAsync(a => a.CompeticaoId == id);
            CompeticaoEquipe model = new CompeticaoEquipe()
            {
                CompeticaoId = competicao.CompeticaoId
            };
            await ViewBags();
            return View(model);

        }


        // POST: /Equipes/Criar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CompeticaoEquipe(CompeticaoEquipe model)
        {
            if (ModelState.IsValid)
            {
                //competicoesequipes.Equipe.EquipeId = Guid.NewGuid();
                //db.Equipes.Add(competicoesequipes.Equipe);
                //competicoesequipes.CompeticaoEquipe.CompeticaoEquipeId = Guid.NewGuid();
                //competicoesequipes.Competicao.CompeticaoId = competicoesequipes.CompeticaoId;
                //competicoesequipes.CompeticaoEquipe.EquipeId = competicoesequipes.Equipe.EquipeId;
                //db.CompeticoesEquipes.Add(competicoesequipes.CompeticaoEquipe);
                //await db.SaveChangesAsync();
                //TempData["Mensagem"] = "Operação realizada com sucesso!";
                return RedirectToAction("Indice");
            }

            await ViewBags();
            return View(model);
        }




        private async Task ViewBags()
        {
            ViewBag.Regiaos = new SelectList(await db.Regioes.ToListAsync(), "RegiaoId", "Numero");
            ViewBag.Cidades = new SelectList(await db.Cidades.ToListAsync(), "CidadeId", "Nome");

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
