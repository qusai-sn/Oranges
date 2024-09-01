using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oranges.Models;

namespace Oranges.Controllers
{
    public class VotingResultController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VotingResultController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/VotingResult/SessionWinners/{sessionId}
        [HttpGet("SessionWinners/{sessionId}")]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetWinningRestaurants(int sessionId)
        {
            var session = await _context.VotingSessions
                .Include(s => s.Votes)
                .ThenInclude(v => v.Restaurant)
                .FirstOrDefaultAsync(s => s.Id == sessionId);

            if (session == null)
            {
                return NotFound("Voting session not found.");
            }

            var winningRestaurants = session.Votes
                .GroupBy(v => v.RestaurantId)
                .Select(group => new
                {
                    RestaurantId = group.Key,
                    Votes = group.Count()
                })
                .Where(g => g.Votes > 5)  // Any restaurant with more than 5 votes
                .Select(g => g.RestaurantId)
                .ToList();

            var restaurants = await _context.Restaurants
                .Where(r => winningRestaurants.Contains(r.Id))
                .ToListAsync();

            return Ok(restaurants);
        }
    }

}
