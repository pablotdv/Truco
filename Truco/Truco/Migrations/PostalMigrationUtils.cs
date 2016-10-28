using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Truco.Migrations
{
    public static class PostalMigrationUtils
    {
        public class LogradouroModel
        {
            public Guid LogradouroId { get; set; }
            public Guid BairroId { get; set; }
            public string Descricao { get; set; }
            public string DescricaoFonetizado { get; set; }
            public string Cep { get; set; }
        }

        public class BairroModel
        {
            public Guid BairroId { get; set; }
            public Guid CidadeId { get; set; }
            public string Nome { get; set; }
            public string NomeAbreviado { get; set; }
            public string NomeFonetizado { get; set; }
        }

        public class CidadeModel
        {
            public Guid CidadeId { get; set; }
            public Guid EstadoId { get; set; }
            public string Nome { get; set; }
            public string Cep { get; set; }
        }

        public class EstadoModel
        {
            public Guid EstadoId { get; set; }
            public Guid PaisId { get; set; }
            public string Nome { get; set; }
            public string Sigla { get; set; }
        }

        public class PaisModel
        {
            public Guid PaisId { get; set; }
            public string Nome { get; set; }
            public string Sigla { get; set; }
        }

        public static Models.Logradouro[] LoadLogradouros(Models.Usuario usuario)
        {
            var leitor = new CsvHelper.CsvReader(new StringReader(Properties.Resources.Logradouros));
            leitor.Configuration.Delimiter = ";";
            return leitor.GetRecords<LogradouroModel>().Select(entity => new Models.Logradouro
            {
                LogradouroId = entity.LogradouroId,
                BairroId = entity.BairroId,
                Descricao = entity.Descricao,
                DescricaoFonetizado = entity.DescricaoFonetizado,
                Cep = entity.Cep == "NULL" ? default(int?) : Int32.Parse(entity.Cep),
                DataUltimaModificacao = DateTime.Now,
                DataCriacao = DateTime.Now,
                UsuarioCriacao = usuario.UserName,
                UsuarioUltimaModificacao = usuario.UserName
            }).ToArray();
        }

        public static Models.Bairro[] LoadBairros(Models.Usuario usuario)
        {
            var leitor = new CsvHelper.CsvReader(new StringReader(Properties.Resources.Bairros));
            leitor.Configuration.Delimiter = ";";
            return leitor.GetRecords<BairroModel>().Select(entity => new Models.Bairro
            {
                BairroId = entity.BairroId,
                CidadeId = entity.CidadeId,
                Nome = entity.Nome,
                NomeAbreviado = entity.NomeAbreviado,
                NomeFonetizado = entity.NomeFonetizado,
                DataUltimaModificacao = DateTime.Now,
                DataCriacao = DateTime.Now,
                UsuarioCriacao = usuario.UserName,
                UsuarioUltimaModificacao = usuario.UserName
            }).ToArray();
        }

        public static Models.Cidade[] LoadCidades(Models.Usuario usuario)
        {
            var cidades = new List<Models.Cidade>();
            var leitor = new CsvHelper.CsvReader(new StringReader(Properties.Resources.Cidades));
            leitor.Configuration.Delimiter = ";";
            return leitor.GetRecords<CidadeModel>().Select(entity => new Models.Cidade
            {
                CidadeId = entity.CidadeId,
                EstadoId = entity.EstadoId,
                Nome = entity.Nome,
                Cep = entity.Cep == "NULL" ? default(int?) : Int32.Parse(entity.Cep),
                DataUltimaModificacao = DateTime.Now,
                DataCriacao = DateTime.Now,
                UsuarioCriacao = usuario.UserName,
                UsuarioUltimaModificacao = usuario.UserName
            }).ToArray();
        }

        public static Models.Estado[] LoadEstados(Models.Usuario usuario)
        {
            var leitor = new CsvHelper.CsvReader(new StringReader(Properties.Resources.Estados));
            leitor.Configuration.Delimiter = ";";
            return leitor.GetRecords<EstadoModel>().Select(entity => new Models.Estado
            {
                EstadoId = entity.EstadoId,
                PaisId = entity.PaisId,
                Nome = entity.Nome,
                Sigla = entity.Sigla,
                DataUltimaModificacao = DateTime.Now,
                DataCriacao = DateTime.Now,
                UsuarioCriacao = usuario.UserName,
                UsuarioUltimaModificacao = usuario.UserName
            }).ToArray();
        }

        public static Models.Pais[] LoadPaises(Models.Usuario usuario)
        {
            var leitor = new CsvHelper.CsvReader(new StringReader(Properties.Resources.Pais));
            leitor.Configuration.Delimiter = ";";
            return leitor.GetRecords<PaisModel>().Select(entity => new Models.Pais
            {
                PaisId = entity.PaisId,
                Nome = entity.Nome,
                Sigla = entity.Sigla,
                DataUltimaModificacao = DateTime.Now,
                DataCriacao = DateTime.Now,
                UsuarioCriacao = usuario.UserName,
                UsuarioUltimaModificacao = usuario.UserName
            }).ToArray();
        }
    }
}