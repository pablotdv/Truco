using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Truco.Models
{
    [Table("Regioes")]
    public class Regiao
    {
        [Key]
        public Guid RegiaoId { get; set; }

        public int Numero { get; set; }

        public ICollection<Cidade> Cidades { get; set; }
    }
}