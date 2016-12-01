using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Truco.Models.Enums;

namespace Truco.Models
{
    [Table("CompeticoesFases")]
    public class CompeticaoFase
    {
        [Key]
        public Guid CompeticaoFaseId { get; set; }
        public Guid CompeticaoId { get; set; }
        public Guid? CompeticaoFasePrincipalId { get; set; }
        [Required]
        public string Nome { get; set; }
        public CompeticaoFaseModo Modo { get; set; }
        public CompeticaoFaseTipo Tipo { get; set; }
        public virtual Competicao Competicao { get; set; }
        [ForeignKey("CompeticaoFasePrincipalId")]
        public virtual CompeticaoFase CompeticaoFasePrincipal { get; set; }
        [InverseProperty("CompeticaoFasePrincipal")]
        public ICollection<CompeticaoFase> CompeticoesFesesRepescagem { get; set; }
        public ICollection<CompeticaoFaseGrupo> CompeticoesFasesGrupos { get; set; }
    }
}