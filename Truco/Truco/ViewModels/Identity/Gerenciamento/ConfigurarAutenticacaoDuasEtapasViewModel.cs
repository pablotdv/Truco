using System.Collections.Generic;

namespace Truco.ViewModels
{
    public class ConfigurarAutenticacaoDuasEtapasViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}