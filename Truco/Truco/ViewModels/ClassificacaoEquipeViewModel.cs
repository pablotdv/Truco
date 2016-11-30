using Truco.Models;
using Truco.ViewModels.Enums;

namespace Truco.ViewModels
{
    public class ClassificacaoEquipeViewModel
    {
        public int Posicao { get; set; }
        public CompeticaoFaseGrupoEquipe CompeticaoFaseGrupoEquipe { get; set; }
        public Classificacao Classificacao { get;set;}
    }
}