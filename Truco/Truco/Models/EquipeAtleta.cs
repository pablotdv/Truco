﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Truco.Models
{
    [Table("EquipesAtletas")]
    public class EquipeAtleta
    {
        [Key]
        [Column(Order = 1)]
        public Guid EquipeId { get; set; }
        [Key]
        [Column(Order = 2)]
        public Guid AtletaId { get; set; }
        public virtual Equipe Equipe { get; set; }
        public virtual Atleta Atleta { get; set; }
    }
}