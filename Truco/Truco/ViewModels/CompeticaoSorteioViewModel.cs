using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Truco.Models;
using Truco.ViewModels.Enums;

namespace Truco.ViewModels
{
    public class CompeticaoSorteioViewModel
    {
        public Competicao Competicao { get; set; }
        public CompeticaoSorteioModo SorteioModo { get; set; }
    }
}