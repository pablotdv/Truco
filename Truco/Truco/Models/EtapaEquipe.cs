using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Truco.Models
{
    [Table("EtapasEquipes")]
    public class EtapaEquipe
    {
        [Key]
        [Column(Order = 1)]
        public Guid EtapaId { get; set; }
        [Key]
        [Column(Order = 2)]
        public Guid EquipeId { get; set; }

        public virtual Etapa Etapa { get; set; }
        public virtual Equipe Equipe { get; set; }

    }
}