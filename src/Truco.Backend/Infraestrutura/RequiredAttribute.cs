using System;
using Truco.Backend.Models.Resources;

namespace Truco.Backend
{
    public class RequiredAttribute : System.ComponentModel.DataAnnotations.RequiredAttribute
    {
        public RequiredAttribute()
            :base()
        {
            ErrorMessageResourceType = typeof(PadraoResource);
            ErrorMessageResourceName = nameof(PadraoResource.Required);
        }
    }
}