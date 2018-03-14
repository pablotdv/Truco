using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Truco.Backend.Models.Resources;

namespace Truco.Backend
{
    public class StringLengthAttribute : System.ComponentModel.DataAnnotations.StringLengthAttribute
    {
        public StringLengthAttribute(int maximumLength)
            :base(maximumLength)
        {
            ErrorMessageResourceType = typeof(PadraoResource);
            ErrorMessageResourceName = nameof(PadraoResource.StringLength);
        }
    }
}
