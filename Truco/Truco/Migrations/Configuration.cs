namespace Truco.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Truco.Models.TrucoDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Truco.Models.TrucoDbContext context)
        {
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Launch();
            }

            var roleManager = new ApplicationRoleManager(new RoleStore<Grupo, Guid, UsuarioGrupo>(context));
            var roleNames = new string[] { "Administradores" };
            foreach (var roleName in roleNames)
                if (!roleManager.Roles.Any(r => r.Name == roleName))
                    roleManager.Create(new Grupo { Name = roleName });

            var userManager = new ApplicationUserManager(new UserStore<Usuario, Grupo, Guid, UsuarioLogin, UsuarioGrupo, UsuarioIdentidade>(context));

            var user1 = CreateUser(userManager, "pablotdv@gmail.com", "truco@123", "Administradores");

            CriarPaises(context, user1);
            CriarEstados(context, user1);
            CriarCidades(context, user1);
            CriarBairros(context, user1);
            CriarLogradouros(context, user1);
        }

        private Usuario CreateUser(ApplicationUserManager userManager, string userName, string userPassword, string userRole)
        {
            var user = userManager.FindByName(userName);
            if (user == null)
            {
                user = new Usuario
                {
                    Id = Guid.NewGuid(),
                    UserName = userName,
                    Email = userName,
                    Enderecos = new HashSet<Endereco>(),
                };

                var result = userManager.Create(user, userPassword);
                result = userManager.SetLockoutEnabled(user.Id, false);
                if (!String.IsNullOrEmpty(userRole))
                    userManager.AddToRole(user.Id, userRole);
            }
            return user;
        }

        private void CriarPaises(Truco.Models.TrucoDbContext context, Usuario usuario)
        {
            if (context.Paises.Any())
                return;

            var entidades = PostalMigrationUtils.LoadPaises(usuario);
            if (entidades != null && entidades.Length > 0)
            {
                foreach (var entidade in entidades)
                {
                    context.Paises.Add(entidade);
                }
                context.SaveChanges();
            }
        }

        private void CriarEstados(Truco.Models.TrucoDbContext context, Usuario usuario)
        {
            if (context.Estados.Any())
                return;

            var entidades = PostalMigrationUtils.LoadEstados(usuario);
            if (entidades != null && entidades.Length > 0)
            {
                foreach (var entidade in entidades)
                {
                    context.Estados.Add(entidade);
                }
                context.SaveChanges();
            }
        }

        private void CriarCidades(Truco.Models.TrucoDbContext context, Usuario usuario)
        {
            if (context.Cidades.Any())
                return;

            var dataset = PostalMigrationUtils.LoadCidades(usuario);
            var total = (int)Math.Ceiling((decimal)dataset.Length / 100);
            for (var pagina = 0; pagina < total; pagina++)
            {
                using (var _context = new Models.TrucoDbContext())
                {
                    _context.Configuration.AutoDetectChangesEnabled = false;
                    _context.Configuration.ValidateOnSaveEnabled = false;

                    var entidades = dataset.Skip(pagina * 100).Take(100);
                    if (entidades != null && entidades.Any())
                    {
                        foreach (var entidade in entidades)
                        {
                            _context.Cidades.Add(entidade);
                        }
                        _context.ChangeTracker.DetectChanges();
                        _context.SaveChanges();
                    }
                }
            }
        }

        private void CriarBairros(Truco.Models.TrucoDbContext context, Usuario usuario)
        {
            if (context.Bairros.Any())
                return;

            var dataset = PostalMigrationUtils.LoadBairros(usuario);
            var total = (int)Math.Ceiling((decimal)dataset.Length / 100);
            for (var pagina = 0; pagina < total; pagina++)
            {
                using (var _context = new Models.TrucoDbContext())
                {
                    _context.Configuration.AutoDetectChangesEnabled = false;
                    _context.Configuration.ValidateOnSaveEnabled = false;

                    var entidades = dataset.Skip(pagina * 100).Take(100);
                    if (entidades != null && entidades.Any())
                    {
                        foreach (var entidade in entidades)
                        {
                            _context.Bairros.Add(entidade);
                        }
                        _context.ChangeTracker.DetectChanges();
                        _context.SaveChanges();
                    }
                }
            }
        }

        private void CriarLogradouros(Truco.Models.TrucoDbContext context, Usuario usuario)
        {
            if (context.Logradouros.Any())
                return;

            var dataset = PostalMigrationUtils.LoadLogradouros(usuario);
            var total = (int)Math.Ceiling((decimal)dataset.Length / 100);
            for (var pagina = 0; pagina < total; pagina++)
            {
                using (var _context = new Models.TrucoDbContext())
                {
                    _context.Configuration.AutoDetectChangesEnabled = false;
                    _context.Configuration.ValidateOnSaveEnabled = false;

                    var entidades = dataset.Skip(pagina * 100).Take(100);
                    if (entidades != null && entidades.Any())
                    {
                        foreach (var entidade in entidades)
                        {
                            _context.Logradouros.Add(entidade);
                        }
                        _context.ChangeTracker.DetectChanges();
                        _context.SaveChanges();
                    }
                }
            }
        }

    }
}
