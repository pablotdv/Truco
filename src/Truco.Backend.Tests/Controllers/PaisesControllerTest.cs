using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truco.Backend.Controllers;
using Truco.Backend.Data;
using Truco.Backend.Models;
using Xunit;

namespace Truco.Backend.Tests.Controllers
{
    public class PaisesControllerTest
    {
        public PaisesControllerTest()
        {

        }

        [Fact]
        public async Task GetPaises()
        {
            var options = Options;

            var pais = await AdicionarPais(options);

            using (TrucoDbContext context = new TrucoDbContext(options))
            {

                PaisesController controller = new PaisesController(context);

                var result = controller.GetPaises();
                var paisResult = result.First();

                Assert.IsAssignableFrom<IEnumerable<Pais>>(result);
                Assert.True(result.Count() == await context.Paises.CountAsync());
                Assert.Equal(pais.PaisId, paisResult.PaisId);
            }
        }

        [Fact]
        public async Task GetPais_BadRequest()
        {
            var options = Options;

            using (TrucoDbContext context = new TrucoDbContext(options))
            {
                PaisesController controller = new PaisesController(context);

                controller.ModelState.AddModelError("key1", "Error");

                var result = await controller.GetPais(Guid.Empty);

                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var valueResult = Assert.IsType<SerializableError>(badRequestResult.Value);
                var value = Assert.IsType<string[]>(valueResult["key1"]);
                Assert.Equal("Error", value[0]);
            }
        }

        [Fact]
        public async Task GetPais_NotFound()
        {
            var options = Options;

            using (TrucoDbContext context = new TrucoDbContext(options))
            {

                PaisesController controller = new PaisesController(context);

                var result = await controller.GetPais(Guid.NewGuid());

                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetPais_Ok()
        {
            DbContextOptions<TrucoDbContext> options = Options;

            Pais pais = await AdicionarPais(options);

            using (TrucoDbContext context = new TrucoDbContext(options))
            {
                PaisesController controller = new PaisesController(context);

                var result = await controller.GetPais(pais.PaisId);

                var okResult = Assert.IsType<OkObjectResult>(result);
                var paisResult = Assert.IsType<Pais>(okResult.Value);
                Assert.Equal(pais.PaisId, paisResult.PaisId);
            }
        }

        [Fact]
        public async Task PutPais_BadRequest_ModelState_Invalid()
        {
            var options = Options;

            using (TrucoDbContext context = new TrucoDbContext(options))
            {
                PaisesController controller = new PaisesController(context);

                controller.ModelState.AddModelError("key1", "Error");

                var result = await controller.PutPais(Guid.NewGuid(), new Pais());

                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var valueResult = Assert.IsType<SerializableError>(badRequestResult.Value);
                var value = Assert.IsType<string[]>(valueResult["key1"]);
                Assert.Equal("Error", value[0]);
            }
        }

        [Fact]
        public async Task PutPais_BadRequest_Id_Diferente()
        {
            var options = Options;

            using (TrucoDbContext context = new TrucoDbContext(options))
            {

                PaisesController controller = new PaisesController(context);

                var result = await controller.PutPais(Guid.NewGuid(), new Pais());

                var badRequestResult = Assert.IsType<BadRequestResult>(result);
                Assert.Equal(400, badRequestResult.StatusCode);
            }
        }

        [Fact]
        public async Task PutPais_NoContent()
        {
            var options = Options;

            var pais = await AdicionarPais(options);

            using (TrucoDbContext context = new TrucoDbContext(options))
            {
                pais.Nome = "Brazil";
                PaisesController controller = new PaisesController(context);

                var result = await controller.PutPais(pais.PaisId, pais);

                var noContentResult = Assert.IsType<NoContentResult>(result);
                Assert.Equal(204, noContentResult.StatusCode);
            }

            using (TrucoDbContext context = new TrucoDbContext(options))
            {
                PaisesController controller = new PaisesController(context);
                var result = await controller.GetPais(pais.PaisId);
                var okResult = Assert.IsType<OkObjectResult>(result);
                var paisResult = Assert.IsType<Pais>(okResult.Value);
                Assert.Equal(pais.PaisId, paisResult.PaisId);
                Assert.Equal(pais.Nome, paisResult.Nome);
            }
        }

        [Fact]
        public async Task PutPais_ConcurrencyException_NotExists_NotFound()
        {
            var options = Options;

            var pais = await AdicionarPais(options);

            using (TrucoDbContext context = new TrucoDbContext(options))
            {
                context.Paises.Remove(pais);
                await context.SaveChangesAsync();
            }

            using (TrucoDbContext context = new TrucoDbContext(options))
            {
                pais.Nome = "Brazil";
                PaisesController controller = new PaisesController(context);

                var result = await controller.PutPais(pais.PaisId, pais);

                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task PutPais_ConcurrencyException_Throw()
        {
            var options = Options;

            var pais = await AdicionarPais(options);

            using (TrucoDbContext context = new TrucoDbContext(options))
            {
                pais.Nome = "Alterado";
                await context.SaveChangesAsync();
            }

            using (TrucoDbContext context = new TrucoDbContext(options))
            {
                pais.Nome = "Brazil";
                PaisesController controller = new PaisesController(context);

                var result = await controller.PutPais(pais.PaisId, pais);

                var noContentResult = Assert.IsType<NoContentResult>(result);
                Assert.Equal(204, noContentResult.StatusCode);
            }

            using (TrucoDbContext context = new TrucoDbContext(options))
            {
                PaisesController controller = new PaisesController(context);
                var result = await controller.GetPais(pais.PaisId);
                var okResult = Assert.IsType<OkObjectResult>(result);
                var paisResult = Assert.IsType<Pais>(okResult.Value);
                Assert.Equal(pais.PaisId, paisResult.PaisId);
                Assert.Equal(pais.Nome, paisResult.Nome);
            }
        }

        [Fact]
        public async Task PostPais_BadRequest_ModelState_Invalid()
        {
            var options = Options;

            using (TrucoDbContext context = new TrucoDbContext(options))
            {
                PaisesController controller = new PaisesController(context);

                controller.ModelState.AddModelError("key1", "Error");

                var result = await controller.PostPais(new Pais());

                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var valueResult = Assert.IsType<SerializableError>(badRequestResult.Value);
                var value = Assert.IsType<string[]>(valueResult["key1"]);
                Assert.Equal("Error", value[0]);
            }
        }

        [Fact]
        public async Task PostPais_Created()
        {
            var options = Options;

            var pais = new Pais()
            {
                PaisId = Guid.NewGuid(),
                Nome = "Brasil",
                Sigla = "BR",
            };

            using (TrucoDbContext context = new TrucoDbContext(options))
            {
                PaisesController controller = new PaisesController(context);

                var result = await controller.PostPais(pais);

                var createdResult = Assert.IsType<CreatedAtActionResult>(result);
                var paisResult = Assert.IsType<Pais>(createdResult.Value);
                Assert.Equal(pais, paisResult);
                Assert.Equal("GetPais", createdResult.ActionName);
                Assert.Equal(pais.PaisId, createdResult.RouteValues["id"]);
            }

            using (TrucoDbContext context = new TrucoDbContext(options))
            {
                var paisResult = await context.Paises.FirstOrDefaultAsync();
                var jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(paisResult);
                var paisJson = Newtonsoft.Json.JsonConvert.SerializeObject(pais);

                Assert.Equal(paisJson, jsonResult);

            }
        }

        [Fact]
        public async Task DeletePais_BadRequest_ModelState_Invalid()
        {
            var options = Options;

            using (TrucoDbContext context = new TrucoDbContext(options))
            {
                PaisesController controller = new PaisesController(context);

                controller.ModelState.AddModelError("key1", "Error");

                var result = await controller.DeletePais(Guid.NewGuid());

                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var valueResult = Assert.IsType<SerializableError>(badRequestResult.Value);
                var value = Assert.IsType<string[]>(valueResult["key1"]);
                Assert.Equal("Error", value[0]);
            }
        }

        [Fact]
        public async Task DeletePais_NotFound()
        {
            var options = Options;

            using (TrucoDbContext context = new TrucoDbContext(options))
            {

                PaisesController controller = new PaisesController(context);

                var result = await controller.DeletePais(Guid.NewGuid());

                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task DeletePais_Ok()
        {
            var options = Options;

            var pais = await AdicionarPais(options);

            using (TrucoDbContext context = new TrucoDbContext(options))
            {
                PaisesController controller = new PaisesController(context);

                var result = await controller.DeletePais(pais.PaisId);

                var okResult = Assert.IsType<OkObjectResult>(result);
                var paisResult = Assert.IsType<Pais>(okResult.Value);
            }

            using (TrucoDbContext context = new TrucoDbContext(options))
            {
                Assert.True(!(await context.Paises.AnyAsync(a => a.PaisId == pais.PaisId)));
            }
        }


        private static async Task<Pais> AdicionarPais(DbContextOptions<TrucoDbContext> options)
        {
            var pais = new Pais()
            {
                PaisId = Guid.NewGuid(),
                Nome = "Brasil",
                Sigla = "BR",
            };

            using (TrucoDbContext context = new TrucoDbContext(options))
            {
                await context.Paises.AddAsync(pais);
                await context.SaveChangesAsync();
            }

            return pais;
        }

        private static DbContextOptions<TrucoDbContext> Options => new DbContextOptionsBuilder<TrucoDbContext>()
                        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                        .Options;
    }
}
