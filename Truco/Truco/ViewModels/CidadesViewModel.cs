using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Truco.Models;

namespace Truco.ViewModels
{ 
    public class CidadesViewModel : PagedListViewModel<Cidade>
    {
		//TODO: adicionar filtros de pesquisa
		public string Nome { get; set; }
	}
}
