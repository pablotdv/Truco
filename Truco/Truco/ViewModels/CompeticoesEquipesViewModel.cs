using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Truco.Models;

namespace Truco.ViewModels
{ 
    public class CompeticoesEquipesViewModel : PagedListViewModel<CompeticaoEquipe>
    {
		//TODO: adicionar filtros de pesquisa
		public string Equipe { get; set; }

        public Guid CompeticaoId { get; set; }
	}
}
