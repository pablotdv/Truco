using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Truco.Models
{
    [Table("Estados")]
    [DisplayColumn("Nome")]
    public class Estado : Entity
    {
        [Key]
        public Guid EstadoId { get; set; }

        [Index("IX_Estado_PaisId")]
        public Guid PaisId { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Estado")]
        public String Nome { get; set; }
        
        [StringLength(2)]
        public String Sigla { get; set; }

        [ForeignKey("PaisId")]
        [InverseProperty("Estados")]
        public virtual Pais Pais { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<Cidade> Cidades { get; set; }

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