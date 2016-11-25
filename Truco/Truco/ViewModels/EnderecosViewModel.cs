using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Truco.Models;

namespace Truco.ViewModels
{ 
    public class EnderecosViewModel : PagedListViewModel<Endereco>
    {
		//TODO: adicionar filtros de pesquisa
		public string Numero { get; set; }
	}
}
