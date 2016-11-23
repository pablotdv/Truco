using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Truco.Models
{
    [Table("Etapas")]
    public class Etapa
    {
        [Key]
        public Guid EtapaId { get; set; }
        public Guid CompeticaoId { get; set; }
        public int Numero { get; set; }
        public DateTime Data { get; set; }
        public virtual Competicao Competicao { get; set; }
        public ICollection<EtapaEquipe> Equipes { get; set; }
    }
}