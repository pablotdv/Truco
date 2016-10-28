using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Truco.Models
{
    public class TrucoDbContext : IdentityDbContext<Usuario, Grupo, Guid, UsuarioLogin, UsuarioGrupo, UsuarioIdentidade>
    {
        public TrucoDbContext()
            : base("DefaultConnection")
        {
        }

        static TrucoDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<TrucoDbContext>(new ApplicationDbInitializer());
        }       

        public static TrucoDbContext Create()
        {
            return new TrucoDbContext();
        }

        public DbSet<Pais> Paises { get; set; }

        public DbSet<Estado> Estados { get; set; }

        public DbSet<Cidade> Cidades { get; set; }

        public DbSet<Bairro> Bairros { get; set; }

        public DbSet<Logradouro> Logradouros { get; set; }

        public DbSet<Endereco> Enderecos { get; set; }
    }
}