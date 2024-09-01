using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oranges.Models;

namespace Oranges.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VoteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Vote/Submit
        [HttpPost("Submit")]
        public async Task<ActionResult> SubmitVote(int restaurantId, int sessionId, int userId)
        {
            var session = await _context.VotingSessions
                .Include(s => s.Votes)
                .FirstOrDefaultAsync(s => s.Id == sessionId && s.StartTime <= DateTime.Now && s.EndTime >= DateTime.Now);

            if (session == null)
            {
                return BadRequest("No active voting session or invalid session ID.");
            }

            var existingVote = await _context.Votes
                .FirstOrDefaultAsync(v => v.UserId == userId && v.VotingSessionId == sessionId);

            if (existingVote != null)
            {
                return BadRequest("You have already voted in this session.");
            }

            var vote = new Vote
            {
                RestaurantId = restaurantId,
                VotingSessionId = sessionId,
                UserId = userId
            };

            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();

            return Ok("Your vote has been submitted.");
        }
    }

}
