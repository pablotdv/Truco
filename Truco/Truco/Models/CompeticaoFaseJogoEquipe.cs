using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Truco.Models
{
    [Table("CompeticoesFasesJogosEquipes")]
    public class CompeticaoFaseJogoEquipe
    {
        [Key]
        public Guid CompeticaoFaseJogoEquipeId { get; set; }

        public Guid CompeticaoFaseEquipeId { get; set; }
        public virtual CompeticaoFaseEquipe CompeticaoFaseEquipe { get; set; }
        public ICollection<CompeticaoFaseJogoEquipeSet> CompeticoesFasesJogosEquipesSets { get; set; }
                
        public ICollection<CompeticaoFaseJogo> CompeticoesFasesJogosUm { get; set; }
        public ICollection<CompeticaoFaseJogo> CompeticoesFasesJogosDois { get; set; }
    }
}