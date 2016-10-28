using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Truco.Models
{
    public interface IEntity
    {
        string UsuarioCad { get; set; }
        DateTime DataHoraCad { get; set; }
    }
}