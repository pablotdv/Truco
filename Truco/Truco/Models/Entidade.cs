using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Truco.Models
{
    [Table("Entidades")]
    public class Entidade
    {
        [Key]
        public Guid EntidadeId { get; set; }

        public string Nome { get; set; }

        public Guid RegiaoId { get; set; }

        public virtual Regiao Regiao { get; set; }

        public ICollection<Equipe> Equipes { get; set; }
    }
}