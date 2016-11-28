using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Truco.Models
{
    [Table("CompeticoesFasesGruposRodadasJogosEquipesSets")]
    public class CompeticaoFaseGrupoRodadaJogoEquipeSet
    {
        [Key]
        public Guid CompeticaoFaseGrupoRodadaJogoEquipeSetId { get; set; }
        public Guid CompeticaoFaseGrupoRodadaJogoEquipeId { get; set; }
        public int Tentos { get; set; }        
        public int Set { get; set; }
        public virtual CompeticaoFaseGrupoRodadaJogoEquipe CompeticaoFaseGrupoRodadaJogoEquipe { get; set; }
    }
}