using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Truco.Backend.Models.Resources;

namespace Truco.Backend.Models
{
    [Table("Paises")]
    [DisplayColumn("Nome")]
    public class Pais
    {
        [Key]
        public Guid PaisId { get; set; }

        [Required(ErrorMessageResourceType = typeof(PadraoResource), ErrorMessageResourceName = nameof(PadraoResource.Required))]
        [StringLength(200, ErrorMessageResourceType = typeof(PadraoResource), ErrorMessageResourceName = nameof(PadraoResource.StringLength))]
        [Display(Name = "País")]
        public string Nome { get; set; }

        [Required(ErrorMessageResourceType = typeof(PadraoResource), ErrorMessageResourceName = nameof(PadraoResource.Required))]
        [StringLength(3, ErrorMessageResourceType = typeof(PadraoResource), ErrorMessageResourceName = nameof(PadraoResource.StringLength))]
        [Display(Name = "Sigla")]
        public string Sigla { get; set; }
    }
}