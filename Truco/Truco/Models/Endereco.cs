using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Truco.Models
{
    [Table("Enderecos")]
    public class Endereco : Entity
    {
        [Key]
        public Guid EnderecoId { get; set; }

        [Required]
        [Index("IXU_Endereco_UsuarioId_TipoEnderecoId", Order = 1, IsUnique = true)]
        public Guid UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        [InverseProperty("Enderecos")]
        public virtual Usuario Usuario { get; set; }

        [Required]
        [Index("IX_Endereco_LogradouroId")]
        public Guid LogradouroId { get; set; }

        public int? Numero { get; set; }

        public string PontoReferencia { get; set; }

        [Required]
        public bool IsEntrega { get; set; }

        [Required]
        public bool IsCobranca { get; set; }

        [ForeignKey("LogradouroId")]
        [InverseProperty("Enderecos")]
        public virtual Logradouro Logradouro { get; set; }

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