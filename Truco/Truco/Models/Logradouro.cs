using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Truco.Models
{
    [Table("Logradouros")]
    [DisplayColumn("Descricao")]
    public class Logradouro : Entity
    {
        [Key]
        public Guid LogradouroId { get; set; }

        [Index("IX_Logradouro_BairroId")]
        public Guid BairroId { get; set; }

        public String Descricao { get; set; }

        public String DescricaoFonetizado { get; set; }

        public int? Cep { get; set; }

        [ForeignKey("BairroId")]
        [InverseProperty("Logradouros")]
        public virtual Bairro Bairro { get; set; }

        [JsonIgnore]
        public ICollection<Endereco> Enderecos { get; set; }

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