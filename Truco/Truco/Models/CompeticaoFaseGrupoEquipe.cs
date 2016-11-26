using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Truco.Models
{
    [Table("CompeticoesFasesGruposEquipes")]
    public class CompeticaoFaseGrupoEquipe
    {
        [Key]
        public Guid CompeticaoFaseGrupoEquipeId { get; set; }
        public Guid CompeticaoFaseGrupoId { get; set; }
        public Guid EquipeId { get; set; }
        public virtual CompeticaoFaseGrupo CompeticaoFaseGrupo { get; set; }
        public virtual Equipe Equipe { get; set; }
        public int Numero { get; set; }
    }
}