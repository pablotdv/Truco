using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Truco.Models;
using Truco.Models.Enums;

namespace Truco.ViewModels
{
    public class ClassificacaoViewModel
    {
        public Guid CompeticaoFaseId { get; set; }
        public int Principal { get; set; }        
        public CompeticaoFaseModo Modo { get; set; }
        public List<ClassificacaoEquipeViewModel> Equipes { get; set; }
        public CompeticaoFase CompeticaoFase { get; internal set; }
    }
}