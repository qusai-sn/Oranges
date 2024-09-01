using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oranges.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class RestaurantController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RestaurantController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Restaurant
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Restaurant>>> GetAllRestaurants()
    {
        return await _context.Restaurants.ToListAsync();
    }

    // GET: api/Restaurant/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Restaurant>> GetRestaurantById(int id)
    {
        var restaurant = await _context.Restaurants.FindAsync(id);

        if (restaurant == null)
        {
            return NotFound("Restaurant not found.");
        }

        return Ok(restaurant);
    }

    // POST: api/Restaurant
    [HttpPost]
    public async Task<ActionResult<Restaurant>> CreateRestaurant([FromForm] Restaurant restaurant)
    {
        if (restaurant == null)
        {
            return BadRequest("Invalid restaurant data.");
        }

        _context.Restaurants.Add(restaurant);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRestaurantById), new { id = restaurant.Id }, restaurant);
    }

    // PUT: api/Restaurant/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRestaurant(int id, [FromForm] Restaurant restaurant)
    {
        if (id != restaurant.Id)
        {
            return BadRequest("Restaurant ID mismatch.");
        }

        var existingRestaurant = await _context.Restaurants.FindAsync(id);
        if (existingRestaurant == null)
        {
            return NotFound("Restaurant not found.");
        }

        existingRestaurant.Name = restaurant.Name;
        existingRestaurant.Description = restaurant.Description;
        existingRestaurant.Image = restaurant.Image;

        _context.Entry(existingRestaurant).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Restaurant/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRestaurant(int id)
    {
        var restaurant = await _context.Restaurants.FindAsync(id);
        if (restaurant == null)
        {
            return NotFound("Restaurant not found.");
        }

        _context.Restaurants.Remove(restaurant);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
