using Newtonsoft.Json;
using PagedList;
using System.ComponentModel.DataAnnotations;

namespace Truco.ViewModels
{
    public class PagedListViewModel<T> : IPagedListViewModel
    {
        public PagedListViewModel()
        {
            Pagina = 1;
            TamanhoPagina = 20;
        }
                
        [Display(Name = "Página")]
        public int Pagina { get; set; }
                
        [Display(Name = "Itens por página")]
        public int TamanhoPagina { get; set; }

        [JsonIgnore]
        public IPagedList<T> Resultados { get; set; }
    }
}