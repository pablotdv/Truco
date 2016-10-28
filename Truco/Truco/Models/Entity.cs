using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Truco.Models
{
    public class Entity : IEntity
    {        
        public string UsuarioCad { get; set; }

        [Required]
        public DateTime DataHoraCad { get; set; }        
    }
}