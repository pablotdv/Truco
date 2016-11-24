using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Truco.Models
{
    [Table("EquipesAtletas")]
    [DisplayColumn("Atleta")]
    public class EquipeAtleta
    {
        [Key]
        public Guid EquipeAtletaId { get; set; }
        public Guid EquipeId { get; set; }
        public Guid AtletaId { get; set; }
        public virtual Equipe Equipe { get; set; }
        public virtual Atleta Atleta { get; set; }
    }
}