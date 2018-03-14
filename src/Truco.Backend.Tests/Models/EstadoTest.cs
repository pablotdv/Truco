using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Truco.Backend.Models;
using Truco.Backend.Tests.Infraestrutura;
using Xunit;

namespace Truco.Backend.Tests.Models
{
    public class EstadoTest
    {
        [Fact]
        public void Estado_Nome_Obrigatorio()
        {
            var estado = new Estado
            {
                EstadoId = Guid.NewGuid(),
                Nome = "",
                Sigla = "RS",
            };

            var result = estado.ValidateModel();

            Assert.True(result.Count == 1);
            Assert.Equal("O campo Estado é obrigatório", result.First().ErrorMessage);
        }

        [Fact]
        public void Estado_Nome_Maximo()
        {
            var estado = new Estado()
            {
                EstadoId = Guid.NewGuid(),
                Nome = new string('*', 201),
                Sigla = "RS",
            };

            var result = estado.ValidateModel();
            Assert.True(result.Count == 1);
            Assert.Equal("O campo Estado deve ser uma string com um comprimento máximo de 200", result.First().ErrorMessage);
        }

        [Fact]
        public void Estado_Sigla_Obrigatorio()
        {
            var estado = new Estado
            {
                EstadoId = Guid.NewGuid(),
                Nome = "Rio Grade do Sul",
                Sigla = "",
            };

            var result = estado.ValidateModel();

            Assert.True(result.Count == 1);
            Assert.Equal("O campo Sigla é obrigatório", result.First().ErrorMessage);
        }

        [Fact]
        public void Estado_Sigla_Maximo()
        {
            var estado = new Estado()
            {
                EstadoId = Guid.NewGuid(),
                Nome = "Rio Grade do Sul",
                Sigla = new string('*', 3),
            };

            var result = estado.ValidateModel();
            Assert.True(result.Count == 1);
            Assert.Equal("O campo Sigla deve ser uma string com um comprimento máximo de 2", result.First().ErrorMessage);
        }

        [Fact]
        public void Estado_Id_Vazio()
        {
            var estado = new Estado()
            {                
                Nome = "Rio Grade do Sul",
                Sigla = "RS",
            };
            
            Assert.Equal(Guid.Empty, estado.EstadoId);
        }        
    }
}
