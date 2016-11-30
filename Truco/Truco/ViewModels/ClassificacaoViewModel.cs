using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Truco.ViewModels
{
    public class ClassificacaoViewModel
    {
        public Guid CompeticaoFaseId { get; internal set; }
        public List<ClassificacaoEquipeViewModel> Equipes { get; set; }
    }
}