using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Truco.Models;

namespace Truco.ViewModels
{ 
    public class CompeticoesViewModel : PagedListViewModel<Competicao>
    {
		//TODO: adicionar filtros de pesquisa
		public string Nome { get; set; }
	}
}
