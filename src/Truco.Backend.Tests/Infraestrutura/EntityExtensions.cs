using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Truco.Backend.Models;

namespace Truco.Backend.Tests.Infraestrutura
{
    internal static class EntityExtensions
    {
        internal static IList<ValidationResult> ValidateModel(this IEntity model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}
