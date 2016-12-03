using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

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
            // Database.SetInitializer<TrucoDbContext>(new ApplicationDbInitializer());
        }

        public static TrucoDbContext Create()
        {
            return new TrucoDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompeticaoFaseGrupoRodadaJogo>()
                .HasRequired(a => a.CompeticaoFaseGrupoRodadaJogoEquipeUm)
                .WithMany(a => a.CompeticoesFasesGruposRodadasJogosUm)
                .HasForeignKey(a => a.CompeticaoFaseGrupoRodadaJogoEquipeUmId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompeticaoFaseGrupoRodadaJogo>()
                .HasRequired(a => a.CompeticaoFaseGrupoRodadaJogoEquipeDois)
                .WithMany(a => a.CompeticoesFasesGruposRodadasJogosDois)
                .HasForeignKey(a => a.CompeticaoFaseGrupoRodadaJogoEquipeDoisId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompeticaoFaseJogo>()
                .HasRequired(a => a.CompeticaoFaseJogoEquipeUm)
                .WithMany(a => a.CompeticoesFasesJogosUm)
                .HasForeignKey(a => a.CompeticaoFaseJogoEquipeUmId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompeticaoFaseJogo>()
                .HasRequired(a => a.CompeticaoFaseJogoEquipeDois)
                .WithMany(a => a.CompeticoesFasesJogosDois)
                .HasForeignKey(a => a.CompeticaoFaseJogoEquipeDoisId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompeticaoFase>()
                .HasOptional(a => a.CompeticaoFasePrincipal)
                .WithMany(a => a.CompeticoesFesesRepescagem)
                .HasForeignKey(a => a.CompeticaoFasePrincipalId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompeticaoFase>()
                .HasRequired(a => a.Competicao)
                .WithMany(a => a.CompeticoesFases)
                .HasForeignKey(a => a.CompeticaoId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            Auditar();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            Auditar();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void Auditar()
        {
            var states = new List<EntityState>() { EntityState.Added, EntityState.Deleted, EntityState.Modified };

            var entidades = ChangeTracker.Entries().Where(e => e.Entity != null && states.Contains(e.State) && typeof(IEntity).IsAssignableFrom(e.Entity.GetType()));

            foreach (var entry in entidades)
            {
                var currentTime = DateTime.Now;

                if (entry.Property(nameof(IEntity.DataHoraCad)) != null)
                {
                    entry.Property(nameof(IEntity.DataHoraCad)).CurrentValue = currentTime;
                }
                if (entry.Property(nameof(IEntity.UsuarioCad)) != null && HttpContext.Current != null)
                {
                    entry.Property(nameof(IEntity.UsuarioCad)).CurrentValue = HttpContext.Current?.User.Identity.GetUserName() ?? "_";
                }
            }
        }

        public DbSet<Atleta> Atletas { get; set; }
        public DbSet<Bairro> Bairros { get; set; }
        public DbSet<Cidade> Cidades { get; set; }
        public DbSet<Competicao> Competicoes { get; set; }
        public DbSet<CompeticaoEquipe> CompeticoesEquipes { get; set; }
        public DbSet<CompeticaoFase> CompeticoesFases { get; set; }
        public DbSet<CompeticaoFaseGrupo> CompeticoesFasesGrupos { get; set; }
        public DbSet<CompeticaoFaseGrupoEquipe> CompeticoesFasesGruposEquipes { get; set; }
        public DbSet<CompeticaoFaseGrupoRodada> CompeticoesFasesGruposRodadas { get; set; }
        public DbSet<CompeticaoFaseGrupoRodadaJogo> CompeticoesFasesGruposRodadasJogos { get; set; }
        public DbSet<CompeticaoFaseGrupoRodadaJogoEquipeSet> CompeticoesFasesGruposRodadasJogosEquipesSets { get; set; }
        public DbSet<CompeticaoFaseJogo> CompeticoesFasesJogos { get; set; }
        public DbSet<CompeticaoFaseJogoEquipe> CompeticoesFasesJogosEquipes { get; set; }
        public DbSet<CompeticaoFaseJogoEquipeSet> CompeticoesFasesJogosEquipesSets { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Entidade> Entidades { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Logradouro> Logradouros { get; set; }
        public DbSet<Pais> Paises { get; set; }
        public DbSet<Regiao> Regioes { get; set; }
        public DbSet<RegiaoCidade> RegioesCidades { get; set; }
        public DbSet<PesquisaModel> PesquisasModels { get; set; }
        public DbSet<CompeticaoFaseGrupoRodadaJogoEquipe> CompeticoesFasesGruposRodadasJogosEquipes { get; set; }
        public DbSet<CompeticaoFaseEquipe> CompeticoesFasesEquipes { get; set; }        
    }
}