using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Truco.Models
{
    [Table("RegioesCidades")]
    public class RegiaoCidade
    {
        [Key]
        [Column(Order = 0)]
        public Guid RegiaoId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid CidadeId { get; set; }

        public virtual Regiao Regiao { get; set; }

        public virtual Cidade Cidade { get; set; }
    }
}