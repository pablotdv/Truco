using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Truco.Backend.Models;

namespace Truco.Backend.Data
{
    public class TrucoDbContext : DbContext
    {
        public DbSet<Pais> Paises { get; set; }
    }
}
