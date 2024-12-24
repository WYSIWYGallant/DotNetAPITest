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
    public class PersonController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PersonController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
            return await _context.Person.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> Get(int id)
        {
            var person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return person;
        }

        [HttpPost]
        public async Task<ActionResult<Person>> CreatePerson([FromBody] Person person)
        {
            _context.Person.Add(person);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = person.PersonID }, person);
        }
        //update person in database. pass id in url and person object in body
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, [FromBody] Person person)
        {
            if (id != person.PersonID)
            {
                return BadRequest();
            }

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Person.Any(e => e.PersonID == id))
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
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.Person.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("maxPersonID")]
        public async Task<ActionResult<int>> GetMaxPersonID()
        {
            var maxPersonID = await _context.Person.MaxAsync(p => p.PersonID);
            return maxPersonID+1;
        }
    }
}