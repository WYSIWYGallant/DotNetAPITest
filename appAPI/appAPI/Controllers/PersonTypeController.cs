using appAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace appAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonTypeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PersonTypeController(AppDbContext context)
        {
            _context = context;
        }
        //Get all PersonTypes for use in UI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonType>>> GetPersonTypes()
        {
            return await _context.PersonType.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonType>> GetPersonType(int id)
        {
            var personType = await _context.PersonType.FindAsync(id);
            if (personType == null)
            {
                return NotFound();
            }
            return personType;
        }

        [HttpPost]
        public async Task<ActionResult<PersonType>> PostPersonType([FromBody] PersonType personType)
        {
            _context.PersonType.Add(personType);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPersonType), new { id = personType.PersonTypeID }, personType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonType(int id, [FromBody] PersonType personType)
        {
            if (id != personType.PersonTypeID)
            {
                return BadRequest();
            }

            _context.Entry(personType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.PersonType.Any(e => e.PersonTypeID == id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonType(int id)
        {
            var personType = await _context.PersonType.FindAsync(id);
            if (personType == null)
            {
                return NotFound();
            }

            _context.PersonType.Remove(personType);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}