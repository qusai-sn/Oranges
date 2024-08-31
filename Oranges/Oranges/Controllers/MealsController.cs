using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oranges.Models;

namespace Oranges.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MealsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Meals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Meals>>> GetMeals()
        {
            return await _context.Meals.ToListAsync();
        }

        // GET: api/Meals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Meals>> GetMeals(int id)
        {
            var meals = await _context.Meals.FindAsync(id);

            if (meals == null)
            {
                return NotFound();
            }

            return meals;
        }

        // PUT: api/Meals/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeals(int id, Meals meals)
        {
            if (id != meals.Id)
            {
                return BadRequest();
            }

            _context.Entry(meals).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MealsExists(id))
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

        // POST: api/Meals
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Meals>> PostMeals(Meals meals)
        {
            _context.Meals.Add(meals);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMeals", new { id = meals.Id }, meals);
        }

        // DELETE: api/Meals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeals(int id)
        {
            var meals = await _context.Meals.FindAsync(id);
            if (meals == null)
            {
                return NotFound();
            }

            _context.Meals.Remove(meals);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MealsExists(int id)
        {
            return _context.Meals.Any(e => e.Id == id);
        }
    }
}
