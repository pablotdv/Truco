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
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Truco.Models.TrucoDbContext context)
        {
            //if (!System.Diagnostics.Debugger.IsAttached)
            //{
            //    System.Diagnostics.Debugger.Launch();
            //}

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

            CriarRegioes(context, user1);
        }

        private void CriarRegioes(TrucoDbContext context, Usuario user1)
        {
            CriarRegiao01(context);
            CriarRegiao02(context);
            CriarRegiao03(context);
            CriarRegiao04(context);
            CriarRegiao05(context);
            CriarRegiao06(context);
            CriarRegiao07(context);
            CriarRegiao08(context);
            CriarRegiao09(context);
            CriarRegiao10(context);
            CriarRegiao11(context);
            CriarRegiao12(context);
            CriarRegiao13(context);
            CriarRegiao14(context);
            CriarRegiao15(context);
            CriarRegiao16(context);
            CriarRegiao17(context);
            CriarRegiao18(context);
            CriarRegiao19(context);
            CriarRegiao20(context);
            CriarRegiao21(context);
            CriarRegiao22(context);
            CriarRegiao23(context);
            CriarRegiao24(context);
            CriarRegiao25(context);
            CriarRegiao26(context);
            CriarRegiao27(context);
            CriarRegiao28(context);
            CriarRegiao29(context);
            CriarRegiao30(context);

            context.SaveChanges();


        }

        private static void CriarRegiao01(TrucoDbContext context)
        {
            if (!context.Regioes.Any(a => a.Numero == 1))
            {
                var regiao1 = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = 1,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){ RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("1B046E57-61B7-4EF1-A120-70B41395BF45") },
                        new RegiaoCidade(){ RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("4849FFB8-6EED-46AE-9385-AA0BF903D8A3")},
                        new RegiaoCidade(){ RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("CB0D94EB-548D-4B4B-B857-53A88C0BDE77")},
                        new RegiaoCidade(){ RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("198032C0-9571-40AC-8DC4-E19E88B6DF71")},
                        new RegiaoCidade(){ RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("35CE5D91-BE3E-4F05-872C-95FF7CCAE9E3")},
                        new RegiaoCidade(){ RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("88C5F42F-BFE7-4812-8E35-DE0590CDB263")},
                        new RegiaoCidade(){ RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("5BE34831-D217-421A-BA8E-B0B9D62203DE")},
                        new RegiaoCidade(){ RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("A106883C-B058-48B5-93D0-AACC3BEA4BED")},
                        new RegiaoCidade(){ RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("2F9A4729-7CD2-4DB7-8527-DA554F43E3B5")},
                        new RegiaoCidade(){ RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("A5D2EAB7-8B7B-492B-9110-CF5A8F51909F")},
                        new RegiaoCidade(){ RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("82D00B68-8B43-401F-A558-58CA5C8FFA51")},
                    }
                };

                context.Regioes.Add(regiao1);
            }
        }

        private static void CriarRegiao02(TrucoDbContext context)
        {
            int numero = 2;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("B12A74D6-51BB-41FD-A281-1519D29EAC99") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("37CDDA7D-272B-4031-8F23-21DD09152114") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("77A2B4DA-CFAE-4BBB-9AE8-24C80D12293B") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("EA8A3C4A-0239-44BB-B3B2-51F082615BA0") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("BA9CAE2C-2034-4ABC-98C6-5EFDDEAB96DB") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("AC0744DD-973C-45F1-867A-74F049C03CF9") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("0AFB9887-DCB2-481F-8964-DB7C6CF8E8AC") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("6065B88A-34D2-45F0-B818-EA2F5098C9BE") },
                    }
                };

                context.Regioes.Add(regiao);
            }
        }

        private static void CriarRegiao03(TrucoDbContext context)
        {
            int numero = 3;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("C2254222-C235-4094-9543-0022B99D3539") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("CCDBC78F-184C-42DA-9D21-1191943778DB") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("865FFDCD-CEDC-4FC0-818A-136354316398") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("70BF6D2B-559B-4D3C-BE90-160A935AF124") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("97A3D580-FF39-48FE-9608-1CD763E009CA") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("F5664E3D-5F91-40EE-8012-1F1462C6D859") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("581CF717-EA0E-4393-9CBD-2562BC24834C") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("21C78419-F641-49E0-9F76-3138B5A5CBD4") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("30D2E83A-9094-4AB5-BF07-4D7053E232FD") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("D4CB01E0-9C6A-4BCF-B857-5242AF4E3C5E") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("8A6A318B-DDC8-47E2-B5C4-5375B8AB1D9E") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("A00099EE-F698-4308-AAE7-56C19BA50EA8") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("5314D72D-12CC-4A79-A7BD-59FD329F3924") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("D73F23BE-3744-401D-B7E5-5C3EDC3EF925") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("A6CBC805-0245-47C4-BE73-6A54FBAF92C4") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("E0415A29-B8A1-48C5-AC3A-6E03D2DE9980") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("3C84973E-BF32-4666-B170-72660FCCCBA4") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("53512B47-ADFA-4FED-A6D5-7498E9EFBB2D") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("31AFCFAE-0270-4D8A-98C9-78106CDCFA5A") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("747A5FD4-C58D-49D0-8BAB-7A136530D477") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("884AF7B2-1A3D-4464-8CB0-83EC085E0B11") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("B35254E5-EA0A-481C-BB09-8CC8B4B710E0") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("F16BB21F-CBF6-4DE9-BAF8-9826892B58BD") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("B5857890-211B-447A-AC44-9BCF63AE3C1D") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("FA8B7CDE-E5E6-46E3-A087-9BDBC56105CD") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("48481347-2F63-4005-B7C3-A353F8A3F502") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("6CA9FCB5-1954-4516-B491-A5291DA19C31") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("490F7421-A980-445D-BAB6-AC78BB857BA4") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("4C1BB9CB-56A9-4A14-83DA-B1C5AFD04E93") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("ED2C167A-66FC-47EB-98B3-C058F25163BA") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("8AA8BAFE-B51B-4E18-BD36-CD041BFE5432") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("AFD44E8C-1DC4-4370-AA3C-DC2B35C42C35") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("CE7012F2-9F45-4E13-92DE-E36C69F36724") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("EF1F6730-B8F1-456D-B9C3-EB3032A01486") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("A71AC034-8012-4644-A5DA-EF14BDB25195") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("8CA472DD-E9C3-41EA-A3FC-EFC2A6CEB4AA") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("4F443186-14FF-4E48-8DBC-FA7C20304A29") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("677E2581-3193-4FA4-927A-FBE1FC5AF73D") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("82AADAAE-BAC8-49CB-AB0A-FC06B593284D") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("A109DE82-4A60-4D55-90C7-FC47366E882D") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("BD91B90E-74C9-43ED-AC8E-FDE028F2B886") },
                    }
                };

                context.Regioes.Add(regiao);
            }
        }

        private static void CriarRegiao04(TrucoDbContext context)
        {
            int numero = 4;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("C3500B81-6173-43DE-9F59-55FE214C93E6") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("8EEF6FCB-88CD-4435-A451-673CCE502345") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("FF7C35E6-73DF-422F-AA8C-8CA427C7F17A") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("7EA9246C-ADB9-4FE2-844C-94832AC6E0BA") },
                    }
                };

                context.Regioes.Add(regiao);
            }
        }

        private static void CriarRegiao05(TrucoDbContext context)
        {
            int numero = 5;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("8DD282F8-2F58-4B4D-81C5-08AC45A87DF4") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("DD9B5A75-0C7A-400E-A985-2F45B5D2C6E6") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("3CE354A1-73BD-4953-B396-3DD1AF7C7424") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("94CB1204-974A-4F46-8866-781BD5730E65") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("D7BB4C37-77E3-449D-9E8A-7D2B2DB9D659") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("26F851C5-68F8-47C2-BE37-7D976D8D50B8") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("B206CEE1-BD0E-490F-822A-7FA1D5686FF6") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("00B58978-60C5-4E77-8BE2-9B5133347FD9") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("3689D54A-BEB7-4207-9B5B-A186FBCEC6B7") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("09D3E866-9514-4F71-98A7-BAD720160A57") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("345E83BF-9C2F-4ACF-9551-C1894EDC358E") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("C08EA467-E0A8-4EA3-A6B9-C64BE6639658") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("69AAA555-0E19-4077-ABE8-E40DB9F2F58A") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("D816E6C7-3829-4FA0-97B8-F63CE89EC899") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("DAF4BD95-1B19-46B9-AFF5-FA2D674B9048") },
                    }
                };

                context.Regioes.Add(regiao);
            }
        }

        private static void CriarRegiao06(TrucoDbContext context)
        {
            int numero = 6;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("BBD6B9A1-5B5C-4B83-AC4D-065B399BE2B9") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("51F6A62D-9CD2-484E-A172-7F656F1E5DBC") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("D65F98F0-2B1F-4A45-832C-CC5989465974") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("6E475D85-46BA-45F2-8B26-F7AA02F61E35") },
                    }
                };

                context.Regioes.Add(regiao);
            }
        }

        private static void CriarRegiao07(TrucoDbContext context)
        {
            int numero = 7;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("D51F4026-C83D-4507-993B-4E19CB1C58EA") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("887AC47E-875F-4B3A-BE5B-7522557A6398") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("683DB0E5-C218-4ECE-83D4-E369A3F42BE6") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("743165D9-6DA0-4BDD-8DFA-20FA81297577") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("08F9E7AA-7EF7-488F-8E1D-B96151501A14") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("7633DED2-DA5F-4C7D-A20F-E236D40CE436") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("D182B181-499B-4021-A872-5BD4F581426C") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("C1E27F6D-02CC-4E2B-86E5-473C4CCFFF91") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("A6B99A3D-5746-4EA3-AC12-04AF40BDB203") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("C9E4A35D-7D85-4C1E-AE5C-7DCBB93FF3F1") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("3568C146-133D-406D-8845-8650F79F2C49") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("0241C182-3BB2-4DFA-A5C8-FF0740880DCD") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("AC51A8A8-508E-4E98-BE78-2403651FB789") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("3C348676-E21A-4DB8-8EA6-2F9A750F7855") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("A344E276-D09D-46A5-BEB8-FE762C0BB19C") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("5374E980-A6CD-4396-96DC-416BC742E4A4") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("D059AA80-A9CD-422E-982D-C3A2A730F19B") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("A9F0DF1C-2E6D-40E3-9A0B-63C4265D3A42") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("D35D8C10-1C94-468D-B953-4ECAB7703D90") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("A84F3277-9FE7-4E66-9776-BCCE2230FCCE") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("EB2BE7A6-AE6A-4176-9FF7-EDCF1631C689") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("F9007E64-69CF-4F84-A00C-CABB9043DA66") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("0E73F4B0-9187-4A99-9212-DD7381B7B92C") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("F777AC86-A44E-46EB-BE09-E98CB1533E3B") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("4069FACF-D399-4FF1-9C7E-96EA3D955DD7") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("05504B82-02B4-4D5C-8AF3-0495D2D0F26B") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("A671EFE0-3EFB-4C77-BFB6-84A5157F4968") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("EA045BE5-9ED5-4231-8397-FC883A31EDF5") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("ED2E0E4B-5E62-4038-99C4-6BD417C9B569") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("DC68154A-322F-46B6-BDD8-835C74C0E0CC") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("38D1A5E5-AAD3-47C1-A86F-BE43DF096196") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("F9516429-5F51-4583-AE6E-2960AE38B124") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("2FD25007-BB41-4B06-95CB-F9C20E952F9C") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("1F0EBA9F-A7EE-4CE8-818C-60BE719C80A5") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("E4ED171D-DC21-4DC9-BD46-5A94DA726861") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("AF66B35C-F3A7-4802-BF87-EE9B1E4AE714") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("B90D12BE-99F0-49DD-A70E-B221CB758665") },
                    }
                };

                context.Regioes.Add(regiao);
            }
        }

        private static void CriarRegiao08(TrucoDbContext context)
        {
            int numero = 8;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("8477B809-FDB2-4DFE-B351-0C2ABFDA6322") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("F35394ED-D5DA-404E-8FAC-E127005615C5") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("8BD673F1-80CD-42FF-BF84-42D69D16C6CE") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("6C0BE1F1-A0B0-49E2-9347-0E41E755B760") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("A6E1317C-A90C-4C24-949A-E8D59D9FD34C") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("0A5DAD34-ACD6-4645-A0D8-80943CDCDB43") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("E03A65A3-BD6D-4816-A2C1-DEC830358E31") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("49D4BF9F-C2E7-4406-94E1-8339CE02F0F5") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("4929E64C-890B-4D64-AE56-9FC5FEF693D9") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("314B3ECF-8090-4FC2-B59F-70B31C3FE83B") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("074BEF41-0944-4BCC-B1A2-034C225E8C70") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("EA419D90-3A71-42A5-A3CD-47C638786EE5") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("A0FDB107-A123-49C9-99C1-5BDD2C8562C8") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("88B3D7BC-A714-4015-AE32-E3BEF0B38FB8") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("71035F78-5C67-4812-9AFF-790FE249DC65") },
                    }
                };

                context.Regioes.Add(regiao);
            }
        }

        private static void CriarRegiao09(TrucoDbContext context)
        {
            int numero = 9;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("59721336-AB94-4F5E-B6BE-961B8974AF57") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("D12095ED-034B-46D6-AB65-05D2DECCC82B") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("549C2FEA-3B6E-4303-8034-6FB6A652B583") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("9EC02E42-1AC4-4BF5-935A-0A2FC415F58F") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("38DF4905-734F-44AC-9A1E-1972FA36B313") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("3763131C-857C-4F84-96FD-B648CF95100A") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("FFC6C49E-7B6D-4696-ADCE-64DDA5BC1B42") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("0641EDF8-06E7-4556-8996-2409F62E265E") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("5B4D29E6-B18C-422E-A74A-1062978D96F3") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("79CC86A0-1B3E-4FAC-82D6-024385CA741C") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("C07EB713-C704-449C-AD98-C94FF9DE714D") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("3582366D-71D5-4E56-86AB-EC76D5DE91D0") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("834830D8-CE15-4F3E-9804-2531F88DCECA") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("5B4C98C6-351F-45E2-B22F-8725DEFE0321") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("E9F9B10D-A0E7-48CD-AFE1-E653CEEBF865") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("3C76685F-87A2-48DE-B095-05411ADD9B83") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("50C2DB99-ACCB-40B4-A009-3E2E4D9BFD3C") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("33D3FF14-EA19-4D76-835E-0F8C7CD5B9C1") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("7E96299F-7FE4-45F7-BEC4-816EEA4AD7B0") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("899AA5F1-D3E4-427F-B3BA-FA2D10B02643") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("1CCE4C80-C493-401B-AA9A-B72DACD3D5BC") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("76E9AF00-A538-4036-94E1-176E5F9B31E9") },
                    }
                };

                context.Regioes.Add(regiao);
            }
        }

        private static void CriarRegiao10(TrucoDbContext context)
        {
            int numero = 10;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("32AF583C-8F25-4B19-AF64-4A2A4471FC7A") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("7A78520D-D1C5-416C-856F-EDB5344EB083") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("FB648276-4009-47A2-B992-F73A829F5DC3") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("4B567818-3096-44BC-8B1F-E2AC7D3DF62A") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("9A726FBA-7774-4933-803C-544E5BD68B09") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("466171B0-8B77-40D0-B492-9207DCB98315") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("AC3E56E5-BD0F-44E3-982F-A829C8D2A03E") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("9C35E954-6C61-4B15-9F0C-C7595FC4AD99") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("71DC2EA1-0893-4CB6-B1C2-E9D54E29646A") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("8C802CB4-B72E-48FB-924E-12B11AEA405D") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("B1882699-4F46-4C6F-B9FD-E79F841BDD2E") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("757EE50D-DBCD-41D1-84D3-5CC8BFD6BA83") },
                    }
                };

                context.Regioes.Add(regiao);
            }
        }

        private static void CriarRegiao11(TrucoDbContext context)
        {
            int numero = 11;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("5F06E45F-8FC4-4A14-A073-11D7DA6AC413") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("018B397B-16FA-4B4B-A5DF-D2D3B423F611") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("D3901501-01E2-4811-8144-039EB08D1F21") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("E8D3B507-6ED8-4EBB-A60F-DF6982CE1D7F") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("A2B076D6-1873-4D6E-A1CE-BAD32F435452") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("9A8CFCE3-D43D-4C9F-BA88-FEBA12172F32") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("A28A6DAA-062A-4261-9D9C-A71C905901A6") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("0565D8FC-9B57-4DE0-8631-547A7C130398") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("4C5BF56B-65FA-4291-BF9C-3EBBFF86870C") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("BFB3CB8D-0A18-44B9-AC03-E1E2AEBD2881") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("82D459E2-6F7D-470E-9638-F251A9A0FA8C") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("663FE440-EE33-46C7-A913-491BD255C963") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("E40F266B-6E40-4BBD-ADE5-A9606A0DDB41") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("07FDD3F6-BA7D-47F4-8883-AD7D678D3BD7") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("83A44BE8-B252-41C6-A90A-1D4083B4EEC3") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("309D09DC-0513-43C4-AEB9-AC68763DE277") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("7B9DFB79-C053-41CD-9BEA-EB8BBA8BFAFD") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("640C63F6-EA89-4B4E-AA2B-B1A9978F2C9E") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("40E91A3E-8BD6-4DA0-8014-BCDDE298C9A8") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("05889872-67A2-4304-B292-CBA41DDD4C23") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("605693B8-C677-45B8-ACA2-6FBBD5AEDA31") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("54BAC341-CC0B-4861-BA69-D9EC4118848E") },
                    }
                };

                context.Regioes.Add(regiao);
            }
        }

        private static void CriarRegiao12(TrucoDbContext context)
        {
            int numero = 12;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("F43B7FEA-442E-4127-B4E0-ECE68E34CE34") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("111F15E2-543C-4EB0-9ACF-85D008C83FED") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("4042F691-A1DD-449B-831E-D0E4F55BAF21") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("51F16F48-7868-44B7-83A3-4F405F967F4D") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("476D2FC6-2E74-4903-8D61-110CA493D1B2") },
                    }
                };

                context.Regioes.Add(regiao);
            }
        }

        private static void CriarRegiao13(TrucoDbContext context)
        {
            int numero = 13;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("38CBE57C-50FF-4D84-894E-B7D41746E00C") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("19A5D507-11BF-483C-A515-E1A47CA8BDE1") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("FC82D570-9F11-451F-8C52-48F2A83808A8") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("2D314F21-CE64-418D-80FD-DB0A4612A523") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("993004AE-F0F2-4568-8166-6C1DDD0BEF9E") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("3FF63D63-0A6E-40E4-8626-3895F33EC56E") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("6B9170CA-A6EB-44E6-B7B0-E7EB6FF2DAC0") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("D4465AAC-DF67-4E39-99A6-2DFA92AD9760") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("E1FAF1B4-E2D7-4FCA-9C72-FD44EDDB42A0") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("386344EC-09C1-4403-8713-B15BC032CD5D") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("5BF93ED1-E01F-4F84-89AB-E5CC35735092") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("D1EEDA51-11C6-493B-B970-5C99AA35AB61") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("E8CBD822-3036-4B7E-983E-8DB46AE5DA59") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("BF0A9E09-6383-44D2-9570-0951B2171C80") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("F03E0C49-868C-4907-B737-7E5329C48307") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("DCE143A6-16B6-49FD-A925-D1423B951C78") },
                    }
                };

                context.Regioes.Add(regiao);
            }
        }

        private static void CriarRegiao14(TrucoDbContext context)
        {
            int numero = 14;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("5A9F84E6-E055-41EA-A533-52B65BC9BE09") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("18546C89-6011-458C-93F4-C0755E0810C9") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("0DEEAC43-880E-4382-A87C-D27048673D85") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("1BA2565C-21B9-4E83-B2F2-35DDB07951E0") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("D73636AE-25D0-4751-B853-CA5CE2F7B707") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("CF989FA8-AB74-46B7-8B99-78F6AC38C72A") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("78710396-852C-4E07-8819-4420B1792E21") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("E9A36B8B-2C68-4649-A79C-485955E5753F") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("34FB63B7-BCBF-43CB-B066-EBA429F9B6E2") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("D7D22395-7E79-481D-AC7C-8285FA61AE24") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("667FF10C-B7F2-4831-980F-7550D2F0CE0E") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("FD30303C-D192-4E3E-B38E-58417EE360E4") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("7965ED2B-DABA-497D-966C-1325D2B69411") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("DE293EBC-8B34-49BA-9F00-9BEB5CA9F848") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("FC303C35-73D8-4D89-A65B-DDFD31AD4B0E") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("C4F34309-A5DF-4D79-A272-0F3A99BD276C") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("6F2FB7EB-A6DE-43C3-8007-C67B18A3A7A5") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("E36B03E4-53E2-4391-915F-3FEEF3956BEB") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("0088676B-5AC4-420B-A9A8-09F4CFD9CDE5") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("BFD356B5-B36A-4154-8855-B0A065F53920") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("564B5F4F-D772-4AEC-B616-8EB0F942F4E2") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("BA2D32CB-3BBE-4F8F-8D9B-199BADA83E35") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("E6FE0946-8FEF-4BAA-AF8E-6F7454EA6B03") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("363BA97A-E13D-4033-8FF8-BFDFAFDD768B") },
                    }
                };

                context.Regioes.Add(regiao);
            }
        }

        private static void CriarRegiao15(TrucoDbContext context)
        {
            int numero = 15;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("4317C28D-C2EF-45A8-8410-620224AA74C5") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("D5B54231-73C8-4CB9-A263-D5E52E8FA058") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("BB9A5F8F-3D0E-4800-8816-9D6E7D719E0A") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("EF3E3B26-65C9-4EC0-81BE-A64B7C46E626") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("17423E38-8979-45EC-85FB-C8C2FE2E1E35") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("A4099953-29EE-42ED-BE2D-985E36BE0246") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("12F493A8-9CF1-4528-B8A9-7B8A711FE5AB") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("CB164775-BDBD-4A18-A916-1E4BF0AFAB30") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("47DBC42E-8385-4438-8395-AB39A384B167") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("7BFE5545-CAD2-4088-8E8B-C6FBA3A61335") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("B1D7B5FF-05C6-4E52-99EF-FBB18364EEAA") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("3200C919-6651-4E67-9428-31FFE4440C2F") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("56648FBF-EF07-42D7-8ABA-F43D0826E8C7") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("A286FB8E-5EF1-4BAB-8497-6F6BFE2F66C4") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("5C3DE63C-07A9-4CA3-B12F-F71884247A0D") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("ED245F9F-396B-41B0-975D-A4AA211BE519") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("C097EDFE-62B9-4253-A47B-04415BB28AF4") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("C5C9E036-1467-4F78-A0F8-60C802CAA7F5") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("DD5CBBB7-913C-410D-95D5-6780360519A9") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("F12B3ECB-7E80-4029-BDB3-1299466E69BE") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("93922E3E-2AC9-46D1-8EA4-94A656EE62A5") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("11F4BFF1-24A3-4188-8DFC-70BF353B3724") },
                    }
                };

                context.Regioes.Add(regiao);
            }
        }

        private static void CriarRegiao16(TrucoDbContext context)
        {
            int numero = 16;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("4A76C0F1-1C5E-4855-90A5-ACC83E6BE21A") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("D0D7A78D-161C-40BA-9BAB-D78FD82CB320") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("D37EE664-52FC-4FDA-9980-18312FC5CEA5") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("2E5E4B21-E335-47DA-B2D4-AD16C3B3490D") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("9517C1FC-8D49-41B6-AD95-D9DA0DF0AE26") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("43E055B6-93A3-4DE8-84DB-598F2816C91A") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("B76BB435-28FA-4C7E-B85E-8C5A4A319663") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("EF110B5C-8A1F-4403-8CC5-B7524E6D174E") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("8A40677D-90BF-4805-A646-5C39A100B43D") },
                    }
                };

                context.Regioes.Add(regiao);
            }
        }

        private static void CriarRegiao17(TrucoDbContext context)
        {
            int numero = 17;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("01DDE80C-AB4B-4524-8D0B-43E9D28F6101") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("7C2761A4-5B13-46D3-8569-19CDEDB194A1") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("63A00EFF-571E-47C9-B27D-4FDD37FE3117") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("1823A823-584D-4DE6-86B3-88C0FF8BC459") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("A620343C-D197-49B5-9070-C377525056C4") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("8BCBF029-C80D-4B47-BB8F-FB8ED8EE4A79") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("595BC22F-1EBC-4596-B881-B1384D5B9BDD") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("A671ADAD-0865-4EE1-BB76-C146714068A4") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("D7037E66-B46D-421B-86DA-009608E0E259") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("EAE957DA-C37B-4C25-B383-33B3796E7477") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("1978CFB6-BC38-4206-B6C1-28C452E10D00") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("C8494001-4234-4757-A52C-16FD92EB55DC") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("FD34542E-D788-4731-8F78-5932F2C61DDB") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("DC32652D-346A-4135-913E-F0686A23DB60") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("EA71625D-B31D-4DA5-8740-EFD18BCE9FD0") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("E3946409-E1DD-48DC-8D7F-987B75D73CCC") },
                    }
                };

                context.Regioes.Add(regiao);
            }
        }

        private static void CriarRegiao18(TrucoDbContext context)
        {
            int numero = 18;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("13963F60-5952-40AD-BF07-A5D94208386F") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("6D81E6D1-EB2C-41C8-AB85-D1BBE2A51FBA") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("CB90DBF0-D014-4217-B095-D2FE65E05CDD") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("E9D09F27-BBFC-4FD2-977B-65B8DBD035D8") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("717A9B56-2E1F-484F-B013-F6FB272D33F0") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("C13AB989-2277-4AC6-AD52-ED51961F2D1D") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("B7B1FE50-85ED-43A8-BE3D-FF1287093FCB") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("C9415FD6-95DC-4BEC-9C1D-DDF7BFD528DD") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("4B7334B0-CC4F-4157-AF29-D2A1A9D4FCF2") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("B0CF1C51-70F7-40CB-8718-6B39DF14C00E") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("E6FEE3ED-0BB4-47BB-8264-2CBC2B8DCA5D") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("D5EB1D99-6DEA-418E-80DB-09630284ED56") },
                    }
                };

                context.Regioes.Add(regiao);
            }
        }

        private static void CriarRegiao19(TrucoDbContext context)
        {
            int numero = 19;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("FCF84284-D187-4551-97E8-7002B308AFF6") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("D92324FA-CFD2-4ADC-9453-40098AA0A723") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("5C80CE13-0B83-4ECC-B15C-75BAEEE13920") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("34D42964-64F4-49DE-95DF-54D017056778") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("BED05949-17AE-40C1-8CDE-6C6624553C5B") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("AC4DB397-F732-484A-8B96-91A4BC5FEB67") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("4B331366-A68D-45DA-93B7-41877B1D95BD") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("D762595D-2409-40DB-920F-0A594EE1A8EF") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("8D99DC17-50E4-4421-9E5D-8C038330B5B5") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("A849B3BE-903C-49E7-A247-1CB7A611FD84") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("6A0F306F-D5FC-41AB-85C6-D1C2CDAF6AD4") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("1834E5B1-448E-4207-A809-0C708F22A705") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("A457FF84-E08D-438F-AE2E-DA1F00A3F5A5") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("066E334A-7933-4505-B317-E51BB762C0FE") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("B61EF747-970F-4FA6-B1F8-7769200DA7C9") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("73992951-8C42-49A9-A98B-7076BC9EFE45") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("CF731BC7-E264-4F37-971D-61640714D2FF") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("299CEE82-C60F-47BD-9236-8290BC4DE968") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("A5191C26-327A-401E-89F0-47D6D6FF6669") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("F4CB1FD0-5E89-4133-9A7C-5335466257B3") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("93601068-6F71-4760-8057-B996C4F95F6D") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("C5E49CE7-EFF5-4B70-BC60-0978113A45AD") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("12315B28-F408-481B-B721-BF8CF8CF4D58") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("0D2417EC-AA6B-49D7-91A2-810CC205AAE2") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("0155DBFB-E33C-47D6-8058-54B78002507D") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("A1F7C8A4-A4D5-48DB-9212-159DE4C2D299") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("BBDB4B3F-C4B7-4887-94DB-14C982C68EDA") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("28F326F4-5FF6-4106-8C9A-F635F8A4D2BA") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("22632649-BF57-4A43-86C2-BC603C49CB11") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("4188EF67-F96A-453C-A019-7A44D8C19DFD") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("0B150130-F3B8-4B99-AC81-B653C951E98C") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("072DFB95-957A-45D6-91DD-95D558ACD0BA") },
                    }
                };

                context.Regioes.Add(regiao);
            }
        }

        private static void CriarRegiao20(TrucoDbContext context)
        {
            int numero = 20;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("70E6E85D-D204-4598-9729-C0BF44A32270") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("FB16ACD3-E2B2-4DD6-8B9B-A1330D08FFCB") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("05198FB1-832D-4C8E-9DCF-9A6AFB0B2EFC") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("53F9D5E7-7AAC-40B0-9DF7-63D6238587A8") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("30A319AB-6135-499B-AFCF-1C65886F653D") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("B2EA292F-588E-4E0E-AD7D-7C6F4EA43608") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("AC4983F3-718D-453A-8BB4-CD7384DA2C58") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("192A84FF-546A-427F-B1A8-973ACCBE0244") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("A3BCE601-CC18-437C-94FE-79BF8CDAF649") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("5B1E3E7F-4B5D-4565-B55D-25E973E7DC38") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("B5D1526D-18E8-4421-9F48-C882730361E2") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("1C7861FC-3D08-41CC-9BB4-C4BCE18CABC9") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("9AF913DB-3F5A-4064-8C1D-A514AAF30BEC") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("F6F785DA-8587-4A07-8B7F-FE99CA601DD4") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("72E9A36C-38D2-454E-94BC-969A52FEFD86") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("9DD66548-2AC0-43A9-B5EF-3EA36019F830") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("75D80E83-B7F7-44BB-9326-46892264D2AB") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("B829961E-F4C8-4391-8E75-4ED66DD37AC2") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("4F65562A-8132-421D-8937-160484CE4C00") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("FBF29D69-797D-429F-94BE-9BEBE7CC3C57") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("11F2C9B6-B507-4176-BBF3-D642719461C9") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("7CC0DCE8-E2E3-4AFD-861B-D8A7D267D816") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("EE9B8923-2699-4424-886B-E8D776DCCEF5") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("F902A3C7-D789-41E2-BCC4-09BCADA12BE0") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("59CD219E-E426-492F-85C8-B85ADAF85462") },
                    }
                };

                context.Regioes.Add(regiao);
            }
        }

        private static void CriarRegiao21(TrucoDbContext context)
        {
            int numero = 21;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("49AE7201-C6B4-4ACB-B228-6B1EA9D3F442") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("6381817B-2277-4B6D-82C9-6B202F469814") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("63A21EF1-5604-4E1E-9E31-2186A169E215") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("1CA4B86D-45F0-4718-9347-1519486A3018") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("462886EA-1604-4964-8574-33000243CB5C") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("0E4E8FD5-B3AE-4BBB-9EB3-3C4F69D28F57") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("EB2D77A7-CFEC-4F5C-9454-8D055106AEF1") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("5C14601F-5B61-4FE5-AF01-D047D081B8CB") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("55FD9131-72F9-4AA3-9E67-BC37C0A37764") },
                    }
                };

                context.Regioes.Add(regiao);
            }
        }

        private static void CriarRegiao22(TrucoDbContext context)
        {
            int numero = 22;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("58675C13-D630-418D-80C5-0779EE964BF9") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("4587505F-72F3-40C1-B5DB-33BC8542C028") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("8CCC6008-9A33-41B1-ABCB-783E8345AB00") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("9AC1B65A-7CCE-424A-92B7-1F2E9F74DD08") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("4DD8E39D-FF21-471B-A595-09F7A302CC6B") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("128C7D8B-147A-45CA-A066-1071C6A10CB7") },
                    }
                };

                context.Regioes.Add(regiao);
            }
        }

        private static void CriarRegiao23(TrucoDbContext context)
        {
            int numero = 23;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("5884CC6E-8EEC-4239-BAFC-D236B92FD578") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("5DD1DF94-4E54-4B61-8DB6-577AD6A824F6") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("25D32B7F-5897-4C7B-9883-965C1227405A") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("43AA5862-7690-4156-99AE-6199B2D816AF") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("C2F42124-08E5-4A09-B4AF-18A77606458A") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("2232B26E-EB84-492D-9406-EE7FE04B3DCA") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("2F0F1B7B-A3EB-4CBB-9C41-686BA2344AC0") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("F1B246B8-A020-41FD-9BF5-E4F805BC8311") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("C10BD613-B6F0-4684-928B-915AD54DBC07") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("791C52FF-4182-438B-AE42-1A30E3DD441B") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("47019DE5-4020-4A88-8BDB-FECD3161662D") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("849D9DD0-3B31-4A71-93B8-B74B6E8C199E") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("60359550-2679-43B3-B563-68B6C033C122") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("2B612F34-9CA8-4523-AD29-651F23EC1C69") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("E8770280-3CFF-4DAB-B207-4ECDAD2C628E") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("91BB3396-064A-44F0-894A-411366B9F0D3") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("C896F9FD-0C58-48F0-AD18-EFB960969042") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("388806F5-BA2C-4962-A5FF-A0625618DCC2") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("949C85F5-B00A-429D-B74A-C10AC2CB7260") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("B6EE8C1D-C87C-4BDD-B360-824A8F677E1C") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("3BCD65B3-8914-4238-878D-FA27CB318D86") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("3DABF02A-B3A6-4C52-B7F8-72F4F0E3E3D7") },
                    }
                };

                context.Regioes.Add(regiao);
            }
        }

        private static void CriarRegiao24(TrucoDbContext context)
        {
            int numero = 24;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("482C2254-B472-4AAD-A3B7-C1C30898AB2D") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("BF9D7E19-1631-4C2B-8029-AC7DDE3D16CE") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("3651F4E0-333B-4D4D-97C1-89869F944539") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("BF9D4E99-2297-4DBC-9F24-6936AEE70075") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("57745C5D-159B-455A-88B0-3FFD61CE852D") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("D1906F9A-45DB-4B74-9A8D-DE2231B22167") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("7BA5C9E4-EF74-434F-80DA-E92145E441E0") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("61531BC5-3DFF-479B-B4EE-AEBE22FE5D3C") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("5FD778F3-A2A1-4198-A008-F8E9B2509381") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("D460AFF4-B5BA-4F8A-A058-C4DA7E7AB27C") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("F56B0AB4-006F-4493-A969-8FC10EE022D6") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("FC26CA02-FD5D-41F2-B0EF-73425155A594") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("E23E0575-A51C-432B-8EAC-731AAEC49B28") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("887A9C39-B994-4D55-A791-E4F915A73B53") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("28613C2B-0E48-4721-AF17-48E0F93B67F5") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("7DBE15FC-B976-41BF-ADCE-BB4A6D9937B1") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("B8D96E46-7036-4D15-B575-B7702E1D7221") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("9DFCA1E8-78A4-45E4-8682-44E003D2C1D4") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("82FA1E74-88B5-44F5-929F-DB9AAF8B0CDF") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("F580B89E-866B-49FC-AA4A-54F114C2E363") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("59031857-1C8C-4B52-BF4A-D6B009FA3E7A") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("F934518C-6219-48C1-90EB-5F0B1E60413D") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("A9ED59A8-7331-4D15-96AE-454E6675A60D") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("7B16F0DF-42AC-4F9A-9C39-C8A4E2B0E812") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("5358A2F8-00E5-4739-BFE1-D8503B9AFB75") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("2EAE4AE8-BB05-4EC6-BE5F-428E566108CA") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("CA00D80B-24ED-44ED-8C10-574690110700") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("4839726B-20A1-441E-9B16-5C3D0E1E5795") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("9D2A5360-537F-4F4C-B769-159D5633E5D0") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("7E638243-C246-4312-9AED-8944A3DBDEDF") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("6C777F2A-9E65-4436-B57A-E30C9ED57CA7") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("27AAC053-A2FB-4C6D-98B8-5A665C32774B") },
                    }
                };

                context.Regioes.Add(regiao);
            }
        }

        private static void CriarRegiao25(TrucoDbContext context)
        {
            int numero = 25;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("58EAB6E9-E0D2-4268-8AA8-7677E3D0F0FA") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("526CDA01-2A1F-4932-86D7-B83452C00AAF") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("2BB53CD8-FB54-4A3A-B889-8E9794DFC58E") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("E98840ED-6D94-4633-B357-9A210884FF7C") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("5666AE86-2162-4713-B0F0-03A06C66D0A3") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("D75C1BD6-E60D-45E8-8FC7-A881C90E5272") },
                    }
                };

                context.Regioes.Add(regiao);
            }
        }

        private static void CriarRegiao26(TrucoDbContext context)
        {
            int numero = 26;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("22333F9A-5C77-412C-A5D1-30CCF55A2A23") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("973478DA-63DA-4D32-8720-36F2A96DED18") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("DCD13341-6B25-4B4F-BB07-2B41FC2082F1") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("DBABB7D5-67EE-42F7-9902-0F0D9412F3F7") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("FAAF26F0-558E-49BA-B1D9-A5408868CE0F") },
                    }
                };

                context.Regioes.Add(regiao);
            }
        }

        private static void CriarRegiao27(TrucoDbContext context)
        {
            int numero = 27;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("BF7E1B5E-73FA-4563-80A0-D46E1335160A") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("194D7756-77CA-49F2-8208-6E10BE67432D") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("52DB0B8E-2D6C-48A6-9283-FC62D8811910") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("260F4F29-51DD-4C2C-87FE-ED36769F04BC") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("53678C84-C0CF-45CF-88DD-3D8E20A3FFA8") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("872CFAF9-D93A-4116-B6EB-6048B9BB356E") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("13F6C6ED-EC27-4D23-9EEC-69FA36EEE6D7") },
                    }
                };

                context.Regioes.Add(regiao);
            }
        }

        private static void CriarRegiao28(TrucoDbContext context)
        {
            int numero = 28;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("3BA7F4A8-70E2-4C5B-8E98-989B179749D6") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("153ECF03-BB3D-4382-9ED7-58B3D6A16C5C") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("BE5F4BEA-F09D-4C09-94AF-9C903679D865") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("A30582D2-7509-4FB7-82A0-204DD7165B4F") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("07A6DB40-4956-4657-A80C-C151E6014319") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("D24A446E-7D23-4C19-974A-1C4B45CC4C8B") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("64D01E7A-6FE3-4427-A361-4562E28F21F4") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("76301FE9-A253-43C0-8595-3A57BFF5525E") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("4F97C41C-D436-4803-8C37-87EC7AEE5C90") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("6AD59C49-D282-4B7F-A1D6-30957201120A") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("FB7C465C-44A9-4FD1-A2A3-6BD9B8710199") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("33AE848E-2AFF-44ED-98CC-32C2898ADA91") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("2060EA46-843B-47F3-A822-5AB7F6D22C65") },
                    }
                };

                context.Regioes.Add(regiao);
            }
        }

        private static void CriarRegiao29(TrucoDbContext context)
        {
            int numero = 29;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("FEEF09F3-52F6-4AC2-8884-1A9B9132E007") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("72D0EEA5-5988-45F1-8A28-F21102B312CC") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("34A95F31-7F04-4509-A091-BFC50D97E1FC") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("8ABAF8F2-834A-47E3-AF37-75E9021B6474") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("B7BE8F85-169E-444D-B643-F5F57CA0F08D") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("46F218FF-2B1B-482B-8806-E06C543D164A") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("003BAAB6-DDB0-426B-9E91-ECC544F7C27C") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("B8C667C5-AD83-4313-A8A1-43D3E338A0F9") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("3EC3835D-BCCC-4A9B-BAE8-9BF4D2C765EB") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("ECC86452-8A4E-4A1F-9064-B3B58D13C72A") },
                    }
                };

                context.Regioes.Add(regiao);
            }
        }

        private static void CriarRegiao30(TrucoDbContext context)
        {
            int numero = 30;
            if (!context.Regioes.Any(a => a.Numero == numero))
            {
                var regiao = new Regiao()
                {
                    RegiaoId = Guid.NewGuid(),
                    Numero = numero,
                    Cidades = new List<RegiaoCidade>()
                    {
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("B80A2E08-D78F-49AE-B6ED-932382395E5C") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("CF5E850D-BD6D-4FD7-BD48-7A5D54DDDD25") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("CF043763-22C1-4D1B-B265-FFDFFFC32A2F") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("555587F2-0667-4DD3-AD0A-6A9758EA9B42") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("BBE1F213-837A-48EB-B9BE-430F7946B48C") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("4DC09721-07A9-46AF-8AFC-12270E5A30E5") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("9739F98B-80EF-406D-80FC-7FC6CB4A1B62") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("39906A1B-3C89-4E05-8424-F72E6DB82752") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("11E6CB9C-C8BA-4248-ACF3-60AE8152B500") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("0784420F-A1BA-4B83-85EC-B077D0163A76") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("221839EC-32BE-4841-BDFB-584DDC1306E8") },
                        new RegiaoCidade(){RegiaoCidadeId = Guid.NewGuid(), CidadeId = new Guid("C83D9E95-3271-474E-AA69-E69555919E6E") },
                    }
                };

                context.Regioes.Add(regiao);
            }
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
