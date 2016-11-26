using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Truco.Models
{
    [Table("CompeticoesFasesGruposRodadasJogos")]
    public class CompeticaoFaseGrupoRodadaJogo
    {
        public Guid CompeticaoFaseGrupoRodadaJogoId { get; set; }
        public Guid EquipeUmId { get; set; }
        public Guid EquipeDoisId { get; set; }
        public virtual Equipe EquipeUm { get; set; }
        public virtual Equipe EquipeDois { get; set; }
        
        public ICollection<CompeticaoFaseGrupoRodadaJogoSet> CompeticoesFasesGruposRodadasJogosSets { get; set; }
    }
}