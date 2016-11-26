using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Truco.Models
{
    [Table("Paises")]
    [DisplayColumn("Nome")]
    public class Pais //: Entity
    {
        [Key]
        public Guid PaisId { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Pais")]
        public string Nome { get; set; }

        [Required]
        [StringLength(3)]
        public string Sigla { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<Estado> Estados { get; set; }

        [DisplayName("Data da Última Modificação")]
        public DateTime DataUltimaModificacao { get; set; }
        [DisplayName("Data de Criação")]
        public DateTime DataCriacao { get; set; }
        [DisplayName("Usuário da Criação")]
        public string UsuarioCriacao { get; set; }
        [DisplayName("Usuário da Última Modificação")]
        public string UsuarioUltimaModificacao { get; set; }
    }
}