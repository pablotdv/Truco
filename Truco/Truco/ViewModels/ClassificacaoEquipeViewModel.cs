using System;
using Truco.Models;
using Truco.ViewModels.Enums;

namespace Truco.ViewModels
{
    public class ClassificacaoEquipeViewModel
    {
        public int Posicao { get; set; }
        public CompeticaoFaseGrupoEquipe CompeticaoFaseGrupoEquipe { get; set; }
        public Classificacao Classificacao { get;set;}
        public decimal Aproveitamento { get; internal set; }
        public Guid CompeticaoEquipeId { get; internal set; }
    }
}