using Microsoft.AspNetCore.Mvc;
using Oranges.Models;
using Microsoft.EntityFrameworkCore;


[ApiController]
[Route("api/[controller]")]
public class VotingSessionController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public VotingSessionController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("Create")]
    public async Task<ActionResult> CreateVotingSession([FromForm] VotingSessionDto dto)
    {
        if (dto.VotingSession == null || dto.RestaurantIds == null || !dto.RestaurantIds.Any())
        {
            return BadRequest("Invalid session data or no restaurants selected.");
        }

        _context.VotingSessions.Add(dto.VotingSession);
        await _context.SaveChangesAsync();

        foreach (var restaurantId in dto.RestaurantIds)
        {
            _context.Votes.Add(new Vote
            {
                RestaurantId = restaurantId,
                VotingSessionId = dto.VotingSession.Id,
            });
        }

        await _context.SaveChangesAsync();

        return Ok("Voting session created successfully.");
    }


    // GET: api/VotingSession/Current
    [HttpGet("Current")]
    public async Task<ActionResult<VotingSession>> GetCurrentVotingSession()
    {
        var currentSession = await _context.VotingSessions
            .Include(s => s.Votes)
            .ThenInclude(v => v.Restaurant)
            .FirstOrDefaultAsync(s => s.StartTime <= DateTime.Now && s.EndTime >= DateTime.Now);

        if (currentSession == null)
        {
            return NotFound("No active voting session found.");
        }

        return Ok(currentSession);
    }
}
