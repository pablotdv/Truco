using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Truco.Models;

namespace Truco.ViewModels
{ 
    public class LogradourosViewModel : PagedListViewModel<Logradouro>
    {
		//TODO: adicionar filtros de pesquisa
		public string Descricao { get; set; }
	}
}
