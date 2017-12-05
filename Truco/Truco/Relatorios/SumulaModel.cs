using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Truco.Relatorios
{
    public class SumulaModel
    {
        public string Torneio { get; set; }
        public string Chave { get; set; }        
        public string Rodada { get; set; }
        public string TrioA { get; set; }
        public string TrioB { get; set; }
        public string Fase { get; set; }
    }
}