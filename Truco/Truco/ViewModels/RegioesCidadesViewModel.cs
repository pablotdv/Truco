using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Truco.Models;

namespace Truco.ViewModels
{ 
    public class RegioesCidadesViewModel : PagedListViewModel<RegiaoCidade>
    {
		//TODO: adicionar filtros de pesquisa
		public string Cidade { get; set; }
	}
}
