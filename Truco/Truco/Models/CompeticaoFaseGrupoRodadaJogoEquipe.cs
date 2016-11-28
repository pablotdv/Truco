using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Truco.Models
{
    [Table("CompeticoesFasesGruposRodadasJogosEquipes")]
    public class CompeticaoFaseGrupoRodadaJogoEquipe
    {
        [Key]
        public Guid CompeticaoFaseGrupoRodadaJogoEquipeId { get; set; }


        public Guid CompeticaoFaseGrupoEquipeId { get; set; }
        public virtual CompeticaoFaseGrupoEquipe CompeticaoFaseGrupoEquipe { get; set; }     
        
        public ICollection<CompeticaoFaseGrupoRodadaJogoEquipeSet> CompeticoesFasesGruposRodadasJogosEquipesSets { get; set; }

        public ICollection<CompeticaoFaseGrupoRodadaJogo> CompeticoesFasesGruposRodadasJogosUm { get; set; }
        public ICollection<CompeticaoFaseGrupoRodadaJogo> CompeticoesFasesGruposRodadasJogosDois { get; set; }
    }
}