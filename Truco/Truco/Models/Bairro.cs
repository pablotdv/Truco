using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Truco.Models
{
    [Table("Bairros")]
    [DisplayColumn("Nome")]
    public class Bairro //: Entity
    {
        [Key]
        public Guid BairroId { get; set; }

        [Required]
        [Index("IX_Bairro_CidadeId")]
        public Guid CidadeId { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Bairro")]
        public string Nome { get; set; }

        public string NomeAbreviado { get; set; }

        public string NomeFonetizado { get; set; }

        [ForeignKey("CidadeId")]
        [InverseProperty("Bairros")]
        public virtual Cidade Cidade { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<Logradouro> Logradouros { get; set; }

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