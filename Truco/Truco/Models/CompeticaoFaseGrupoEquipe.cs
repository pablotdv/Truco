﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Truco.Models.Enums;

namespace Truco.Models
{
    [Table("CompeticoesFasesGruposEquipes")]
    public class CompeticaoFaseGrupoEquipe
    {
        [Key]
        public Guid CompeticaoFaseGrupoEquipeId { get; set; }
        public Guid CompeticaoFaseGrupoId { get; set; }
        public Guid CompeticaoEquipeId { get; set; }
        public virtual CompeticaoFaseGrupo CompeticaoFaseGrupo { get; set; }
        public virtual CompeticaoEquipe CompeticaoEquipe { get; set; }
        public int Jogos { get; set; }
        public int Vitorias { get; set; }
        public int Sets { get; set; }
        public int Tentos { get; set; }
        public decimal Aproveitamento { get; set; }        
        public int Numero { get; set; }
        public Lado Lado { get; set; } = Lado.LadoA;  

        public ICollection<CompeticaoFaseGrupoRodadaJogoEquipe> CompeticoesFasesGruposRodadasJogosEquipes { get; set; }        
    }
}