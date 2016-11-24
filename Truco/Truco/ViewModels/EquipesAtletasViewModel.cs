using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Truco.Models;

namespace Truco.ViewModels
{ 
    public class EquipesAtletasViewModel : PagedListViewModel<EquipeAtleta>
    {
		//TODO: adicionar filtros de pesquisa
		public string Atleta { get; set; }
	}
}
