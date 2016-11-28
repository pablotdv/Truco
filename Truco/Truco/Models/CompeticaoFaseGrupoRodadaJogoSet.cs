using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Truco.Models
{
    [Table("CompeticoesFasesGruposRodadasJogosSets")]
    public class CompeticaoFaseGrupoRodadaJogoSet
    {
        [Key]
        public Guid CompeticaoFaseGrupoRodadaJogoSetId { get; set; }
        public Guid CompeticaoFaseGrupoRodadaJogoId { get; set; }
        public int EquipeUmTentos { get; set; }
        public int EquipeDoisTentos { get; set; }
        public int Set { get; set; }
        public virtual CompeticaoFaseGrupoRodadaJogo CompeticaoFaseGrupoRodadaJogo { get; set; }
    }
}