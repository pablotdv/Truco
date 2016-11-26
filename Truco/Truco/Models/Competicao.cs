using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Truco.Models.Enums;

namespace Truco.Models
{
    [Table("Competicoes")]
    [DisplayColumn("Nome")]
    public class Competicao : Entity
    {
        [Key]
        public Guid CompeticaoId { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public CompeticaoTipo Tipo { get; set; }

        [Required]
        public CompeticaoModalidade Modalidade { get; set; }

        public ICollection<CompeticaoEquipe> CompeticoesEquipes { get; set; }

        public ICollection<CompeticaoFase> CompeticoesFases { get; set; }
        public bool Sorteada { get; set; }
    }
}