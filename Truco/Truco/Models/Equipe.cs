using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Truco.Models
{
    [Table("Equipes")]
    [DisplayColumn("Nome")]
    public class Equipe //: Entity
    {
        [Key]
        public Guid EquipeId { get; set; }
        public string Nome { get; set; }
        public Guid? RegiaoId { get; set; }
        public Guid? CidadeId { get; set; }
        public virtual Regiao Regiao { get; set; }
        public virtual Cidade Cidade { get; set; }
        public ICollection<EquipeAtleta> Atletas { get; set; }
        
    }
}