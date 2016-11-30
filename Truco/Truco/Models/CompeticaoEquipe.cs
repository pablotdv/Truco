using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Truco.Models
{
    [Table("CompeticoesEquipes")]
    [DisplayColumn("Equipe")]
    public class CompeticaoEquipe
    {
        [Key]
        public Guid CompeticaoEquipeId { get; set; }
        [Required]
        public Guid CompeticaoId { get; set; }
        [Required]
        public Guid EquipeId { get; set; }
        public virtual Competicao Competicao { get; set; }
        public virtual Equipe Equipe { get; set; }
        public decimal? Aproveitamento { get; internal set; }
    }
}