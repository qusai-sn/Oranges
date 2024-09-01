using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oranges.Models;

[ApiController]
[Route("api/[controller]")]
public class MealsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MealsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("GetAllMeals")]
    public async Task<ActionResult<IEnumerable<Meals>>> GetMeals()
    {
        var meals = await _context.Meals.ToListAsync();
        return Ok(meals);
    }
}
