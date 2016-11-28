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
        public Guid CompeticaoFaseGrupoRodadaId { get; set; }
        public Guid CompeticaoFaseGrupoRodadaJogoEquipeUmId { get; set; }
        public Guid CompeticaoFaseGrupoRodadaJogoEquipeDoisId { get; set; }
        [ForeignKey("CompeticaoFaseGrupoRodadaJogoEquipeUmId")]
        public virtual CompeticaoFaseGrupoRodadaJogoEquipe CompeticaoFaseGrupoRodadaJogoEquipeUm { get; set; }
        [ForeignKey("CompeticaoFaseGrupoRodadaJogoEquipeDoisId")]
        public virtual CompeticaoFaseGrupoRodadaJogoEquipe CompeticaoFaseGrupoRodadaJogoEquipeDois { get; set; }
        public virtual CompeticaoFaseGrupoRodada CompeticaoFaseGrupoRodada { get; set; }
        
    }
}