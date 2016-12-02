using System;
using System.Collections.Generic;
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

        public string Nome { get; set; }
        public Guid? RegiaoId { get; set; }
        public Guid? CidadeId { get; set; }
        public virtual Regiao Regiao { get; set; }
        public virtual Cidade Cidade { get; set; }

        public virtual Competicao Competicao { get; set; }
        
        public decimal? Aproveitamento { get; set; }        

        public ICollection<CompeticaoFaseGrupoEquipe> CompeticoesFasesGruposEquipes { get; set; }
    }
}