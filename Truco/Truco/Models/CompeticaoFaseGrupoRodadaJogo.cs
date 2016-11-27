using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Truco.Models
{
    [Table("CompeticoesFasesGruposRodadasJogos")]
    public class CompeticaoFaseGrupoRodadaJogo
    {
        [Key]
        public Guid CompeticaoFaseGrupoRodadaJogoId { get; set; }
        public Guid CompeticaoFaseGrupoEquipeUmId { get; set; }
        public Guid CompeticaoFaseGrupoEquipeDoisId { get; set; }
        public virtual CompeticaoFaseGrupoEquipe CompeticaoFaseGrupoEquipeUm { get; set; }
        public virtual CompeticaoFaseGrupoEquipe CompeticaoFaseGrupoEquipeDois { get; set; }
        public ICollection<CompeticaoFaseGrupoRodadaJogoSet> CompeticoesFasesGruposRodadasJogosSets { get; set; }
    }
}