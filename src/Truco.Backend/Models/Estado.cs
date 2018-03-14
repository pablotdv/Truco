using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Truco.Backend.Models;
using Truco.Backend.Models.Resources;

namespace Truco.Backend.Models
{
    [Table("Estados")]
    [DisplayColumn("Nome")]
    public class Estado : IEntity
    {
        [Key]
        public Guid EstadoId { get; set; }

        [Required(ErrorMessageResourceType = typeof(PadraoResource), ErrorMessageResourceName = nameof(PadraoResource.Required))]
        public Guid PaisId { get; set; }

        [Required(ErrorMessageResourceType = typeof(PadraoResource), ErrorMessageResourceName = nameof(PadraoResource.Required))]
        [StringLength(200, ErrorMessageResourceType = typeof(PadraoResource), ErrorMessageResourceName = nameof(PadraoResource.StringLength))]
        [Display(Name = "Estado")]
        public String Nome { get; set; }

        [StringLength(2, ErrorMessageResourceType = typeof(PadraoResource), ErrorMessageResourceName = nameof(PadraoResource.StringLength))]
        [Required(ErrorMessageResourceType = typeof(PadraoResource), ErrorMessageResourceName = nameof(PadraoResource.Required))]
        public String Sigla { get; set; }

        [ForeignKey(nameof(PaisId))]        
        public virtual Pais Pais { get; set; }
    }
}