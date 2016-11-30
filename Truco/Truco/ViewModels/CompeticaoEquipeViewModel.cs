using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Truco.Models;

namespace Truco.ViewModels
{
    public class CompeticaoEquipeViewModel
    {
        public Guid CompeticaoId { get; set; }

        public Equipe Equipe { get; set; }

        public Competicao Competicao { get; set; }

        public CompeticaoEquipe CompeticaoEquipe { get; set; }

        public CompeticaoEquipeViewModel()
        {
            CompeticaoEquipes = new List<CompeticaoEquipe>();
           
        }
        public List<CompeticaoEquipe> CompeticaoEquipes { get; set; }

        public List<CompeticaoEquipe> CompeticaoEquipeList { get; set; }
    }
}