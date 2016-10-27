using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace Truco.Models
{
    public class ApplicationRole : IdentityRole<Guid, ApplicationUserRole>
    {
        public ApplicationRole()
        {
            Id = Guid.NewGuid();
        }
    }
}