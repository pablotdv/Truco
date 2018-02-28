﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Truco.Backend.Models;
using Xunit;

namespace Truco.Backend.Tests.Models
{
    public class PaisTest
    {
        [Fact]
        public void Pais_Nome_Obrigatorio()
        {
            var pais = new Pais
            {
                PaisId = Guid.NewGuid(),
                Nome = "",
                Sigla = "BR",
            };

            var result = ValidateModel(pais);

            Assert.True(result.Count == 1);
            Assert.Equal("O campo País é obrigatório", result.First().ErrorMessage);
        }

        [Fact]
        public void Pais_Nome_Maximo()
        {
            var pais = new Pais()
            {
                PaisId = Guid.NewGuid(),
                Nome = new string('*', 201),
                Sigla = "BR",
            };

            var result = ValidateModel(pais);
            Assert.True(result.Count == 1);
            Assert.Equal("O campo País deve ser uma string com um comprimento máximo de 200", result.First().ErrorMessage);
        }

        [Fact]
        public void Pais_Sigla_Obrigatorio()
        {
            var pais = new Pais
            {
                PaisId = Guid.NewGuid(),
                Nome = "Brasil",
                Sigla = "",
            };

            var result = ValidateModel(pais);

            Assert.True(result.Count == 1);
            Assert.Equal("O campo Sigla é obrigatório", result.First().ErrorMessage);
        }

        [Fact]
        public void Pais_Sigla_Maximo()
        {
            var pais = new Pais()
            {
                PaisId = Guid.NewGuid(),
                Nome = "Brasil",
                Sigla = new string('*', 4),
            };

            var result = ValidateModel(pais);
            Assert.True(result.Count == 1);
            Assert.Equal("O campo Sigla deve ser uma string com um comprimento máximo de 3", result.First().ErrorMessage);
        }

        [Fact]
        public void Pais_Id_Vazio()
        {
            var pais = new Pais()
            {                
                Nome = "Brasil",
                Sigla = "BR",
            };
            
            Assert.Equal(Guid.Empty, pais.PaisId);
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}
