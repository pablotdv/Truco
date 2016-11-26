using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Truco.Models
{
    [Table("CompeticoesFasesGruposRodadas")]
    public class CompeticaoFaseGrupoRodada
    {
        [Key]
        public Guid CompeticaoFaseGrupoRodadaId { get; set; }
        public Guid CompeticaoFaseGrupoId { get; set; }
        public int Rodada { get; set; }
        public virtual CompeticaoFaseGrupo CompeticaoFaseGrupo { get; set; }
        public ICollection<CompeticaoFaseGrupoRodadaJogo> CompeticoesFasesGruposRodadasJogos { get; set; }
    }
}