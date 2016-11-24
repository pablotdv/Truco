using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Truco.Models
{
    [Table("PesquisasModels")]
    public class PesquisaModel
    {
        [Key]
        public Guid PesquisaModelId { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public Guid UsuarioId { get; set; }

        [Required]
        [MaxLength(8000)]
        public string Filtro { get; set; }
    }
}