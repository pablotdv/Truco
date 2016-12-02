using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Truco.Models
{
    [Table("CompeticoesFasesEquipes")]
    public class CompeticaoFaseEquipe
    {
        [Key]
        public Guid CompeticaoFaseEquipeId { get; set; }
        public Guid CompeticaoFaseId { get; set; }
        public Guid EquipeId { get; set; }
        public virtual CompeticaoFase CompeticaoFase { get; set; }
        public virtual Equipe Equipe { get; set; }
        public int Jogos { get; set; }
        public int Vitorias { get; set; }
        public int Sets { get; set; }
        public int Tentos { get; set; }
        public decimal Aproveitamento { get; set; }
        public int Numero { get; set; }

        public ICollection<CompeticaoFaseJogoEquipe> CompeticoesFasesJogosEquipes { get; set; }
    }
}