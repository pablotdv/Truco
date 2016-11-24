using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Truco.Models;

namespace Truco.Infraestrutura.Helpers
{
    public static class PesquisaModelStore
    {
        public static async Task AddAsync(string key, object viewModel)
        {
            using (TrucoDbContext context = new TrucoDbContext())
            {
                Guid usuarioId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());

                PesquisaModel model = await context.PesquisasModels.FirstOrDefaultAsync(a => a.UsuarioId == usuarioId && a.Key == key);
                bool novo = model == null;
                if (novo)
                    model = new PesquisaModel()
                    {
                        PesquisaModelId = Guid.NewGuid(),
                        Key = key
                    };

                model.UsuarioId = usuarioId;
                model.Filtro = JsonConvert.SerializeObject(viewModel);

                if (novo)
                    context.PesquisasModels.Add(model);

                await context.SaveChangesAsync();
            }
        }

        public static async Task<string> GetAsync(string key)
        {
            using (TrucoDbContext context = new TrucoDbContext())
            {
                Guid usuarioId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
                PesquisaModel model = await context.PesquisasModels.FirstOrDefaultAsync(a => a.UsuarioId == usuarioId && a.Key == key);

                if (model == null)
                    return "";

                return model.Filtro;
            }
        }
    }
}