using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Truco.Backend.Data;
using Truco.Backend.Models;

namespace Truco.Backend.Controllers
{
    [Produces("application/json")]
    [Route("api/Paises")]
    public class PaisesController : Controller
    {
        private readonly TrucoDbContext _context;

        public PaisesController(TrucoDbContext context)
        {
            _context = context;
        }

        // GET: api/Paises
        [HttpGet]
        public IEnumerable<Pais> GetPaises()
        {
            return _context.Paises;
        }

        // GET: api/Paises/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPais([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pais = await _context.Paises.SingleOrDefaultAsync(m => m.PaisId == id);

            if (pais == null)
            {
                return NotFound();
            }

            return Ok(pais);
        }

        // PUT: api/Paises/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPais([FromRoute] Guid id, [FromBody] Pais pais)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pais.PaisId)
            {
                return BadRequest();
            }

            _context.Entry(pais).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaisExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Paises
        [HttpPost]
        public async Task<IActionResult> PostPais([FromBody] Pais pais)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Paises.Add(pais);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPais", new { id = pais.PaisId }, pais);
        }

        // DELETE: api/Paises/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePais([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pais = await _context.Paises.SingleOrDefaultAsync(m => m.PaisId == id);
            if (pais == null)
            {
                return NotFound();
            }

            _context.Paises.Remove(pais);
            await _context.SaveChangesAsync();

            return Ok(pais);
        }

        private bool PaisExists(Guid id)
        {
            return _context.Paises.Any(e => e.PaisId == id);
        }
    }
}