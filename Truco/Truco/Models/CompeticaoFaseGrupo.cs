﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Truco.Models
{
    [Table("CompeticoesFasesGrupos")]
    [DisplayColumn("Nome")]
    public class CompeticaoFaseGrupo
    {
        [Key]
        public Guid CompeticaoFaseGrupoId { get; set; }
        public string Nome { get; set; }
        public int Grupo { get; set; }
        public ICollection<CompeticaoFaseGrupoEquipe> CompeticoesFasesGruposEquipes { get; set; }
    }
}