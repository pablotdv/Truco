using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Truco.Models
{
    [Table("CompeticoesFasesJogosEquipesSets")]
    public class CompeticaoFaseJogoEquipeSet
    {
        [Key]
        public Guid CompeticaoFaseJogoEquipeSetId { get; set; }
        public Guid CompeticaoFaseJogoEquipeId { get; set; }
        public int Tentos { get; set; }
        public int Set { get; set; }
        public virtual CompeticaoFaseJogoEquipe CompeticaoFaseJogoEquipe { get; set; }
    }
}