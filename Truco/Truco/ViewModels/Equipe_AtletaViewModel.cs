using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Truco.ViewModels
{
    public class Equipe_AtletaViewModel
    {
        public Guid EquipeId { get; set; }

        public Guid AtletaId { get; set; }

        public Guid EquipeAtletaId { get; set; }

        
        public string NomeAtleta { get; set; }
    }
}