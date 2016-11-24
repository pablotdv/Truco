using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Truco.Models.Enums;

namespace Truco.Models
{
    [Table("Competicoes")]
    [DisplayColumn("Nome")]
    public class Competicao : Entity
    {
        public Guid CompeticaoId { get; set; }

        public string Nome { get; set; }

        public CompeticaoTipo Tipo { get; set; }

        public CompeticaoModalidade Modalidade { get; set; }

        public ICollection<Etapa> Etapas { get; set; }        
    }
}