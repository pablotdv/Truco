using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Truco.Models
{
    [Table("Atletas")]
    public class Atleta
    {
        [Key]
        public Guid AtletaId { get; set; }
        public string Nome { get; set; }
    }
}