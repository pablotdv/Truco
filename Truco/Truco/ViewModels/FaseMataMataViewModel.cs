using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Truco.Models;
using Truco.ViewModels.Enums;

namespace Truco.ViewModels
{
    public class FaseMataMataViewModel
    {
        public Guid CompeticaoFaseId { get; set; }
        public CompeticaoFase CompeticaoFase { get; set; }
        public FaseMataMataSubirDescerRepescagem SubirDescerRepescagem { get; set; }
    }
}