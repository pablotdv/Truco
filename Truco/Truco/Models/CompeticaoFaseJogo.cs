using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Truco.Models
{
    [Table("CompeticoesFasesJogos")]
    public class CompeticaoFaseJogo
    {
        [Key]
        public Guid CompeticaoFaseJogoId { get; set; }
        public Guid CompeticaoFaseId { get; set; }

        public Guid CompeticaoFaseJogoEquipeUmId { get; set; }
        public Guid CompeticaoFaseJogoEquipeDoisId { get; set; }
        public int Jogo { get; set; }
        [ForeignKey("CompeticaoFaseJogoEquipeUmId")]
        public virtual CompeticaoFaseJogoEquipe CompeticaoFaseJogoEquipeUm { get; set; }
        [ForeignKey("CompeticaoFaseJogoEquipeDoisId")]
        public virtual CompeticaoFaseJogoEquipe CompeticaoFaseJogoEquipeDois { get; set; }

        public virtual CompeticaoFase CompeticaoFase { get; set; }
    }
}