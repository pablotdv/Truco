using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Truco.Models;

namespace Truco.Models
{
    [Table("Cidades")]
    [DisplayColumn("Nome")]
    public class Cidade //: Entity
    {
        [Key]
        public Guid CidadeId { get; set; }
                
        [Required]
        [Index("IX_Cidade_EstadoId")]
        public Guid EstadoId { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Cidade")]
        public String Nome { get; set; }

        public int? Cep { get; set; }

        [ForeignKey("EstadoId")]
        [InverseProperty("Cidades")]
        public virtual Estado Estado { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<Bairro> Bairros { get; set; }

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