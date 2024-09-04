using Microsoft.AspNetCore.Mvc;
using Oranges.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class VotingSessionController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public VotingSessionController(ApplicationDbContext context)
    {
        _context = context;
    }
    [HttpGet("AllSession/{id:int}")]
    public IActionResult GetSession(int id)
    {
        var data = _context.VotingSessions.Find(id);
        return Ok(data);
    }

    // Create a new voting session with selected restaurants
    [HttpPost("CreateVotingSession")]
    public async Task<IActionResult> CreateVotingSession([FromBody] VotingSessionDto votingSessionDto)
    {
        // Validate the DTO
        if (votingSessionDto == null || votingSessionDto.StartTime >= votingSessionDto.EndTime || votingSessionDto.RestaurantIds == null || !votingSessionDto.RestaurantIds.Any())
        {
            return BadRequest("Invalid voting session details.");
        }

        // Verify if the restaurants exist
        var restaurants = await _context.Restaurants
            .Where(r => votingSessionDto.RestaurantIds.Contains(r.Id))
            .ToListAsync();

        if (restaurants.Count != votingSessionDto.RestaurantIds.Count)
        {
            return BadRequest("Some of the provided restaurant IDs are invalid.");
        }

        // Create the new VotingSession object
        var votingSession = new VotingSession
        {
            StartTime = votingSessionDto.StartTime,
            EndTime = votingSessionDto.EndTime,
            Votes = new List<Vote>(),
            RestaurantIds = votingSessionDto.RestaurantIds // Save restaurant IDs
        };

        // Save the session to the database
        _context.VotingSessions.Add(votingSession);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Voting session created successfully.", votingSessionId = votingSession.Id });
    }


    // Close the voting session
    [HttpPost("CloseVotingSession")]
    public async Task<IActionResult> CloseVotingSession(int votingSessionId)
    {
        var session = await _context.VotingSessions
            .Include(v => v.Votes)
            .FirstOrDefaultAsync(v => v.Id == votingSessionId);

        if (session == null)
        {
            return NotFound("Voting session not found.");
        }

        // Check if the session has ended
        if (session.EndTime > DateTime.UtcNow)
        {
            return BadRequest("The voting session has not ended yet.");
        }

        // Count votes for each restaurant
        var voteCounts = session.Votes
            .GroupBy(v => v.RestaurantId)
            .Select(group => new
            {
                RestaurantId = group.Key,
                VoteCount = group.Count()
            })
            .ToList();

        // Filter out restaurants with less than 5 votes
        var validRestaurants = voteCounts.Where(v => v.VoteCount >= 5).ToList();

        if (!validRestaurants.Any())
        {
            return Ok(new { message = "No restaurants received sufficient votes." });
        }

        var topRestaurant = validRestaurants.OrderByDescending(v => v.VoteCount).First();

        return Ok(new
        {
            message = "Voting session closed successfully.",
            topRestaurantId = topRestaurant.RestaurantId,
            voteCount = topRestaurant.VoteCount
        });
    }

    [HttpPost("SubmitVote")]
    public async Task<IActionResult> SubmitVote([FromBody] Vote vote)
    {
        var session = await _context.VotingSessions.FindAsync(vote.VotingSessionId);
        if (session == null || session.StartTime > DateTime.UtcNow || session.EndTime < DateTime.UtcNow)
        {
            return BadRequest("Invalid or closed voting session.");
        }

        var user = await _context.Users.FindAsync(vote.UserId);
        if (user == null)
        {
            return BadRequest("User not found.");
        }

        var restaurant = await _context.Restaurants.FindAsync(vote.RestaurantId);
        if (restaurant == null)
        {
            return BadRequest("Restaurant not found.");
        }

        // Check if the user has already voted in this session
        var existingVote = await _context.Votes
            .FirstOrDefaultAsync(v => v.UserId == vote.UserId && v.VotingSessionId == vote.VotingSessionId);

        if (existingVote != null)
        {
            return BadRequest("User has already voted in this session.");
        }

        _context.Votes.Add(vote);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Vote submitted successfully." });
    }

    // Get voting session results
    [HttpGet("GetResults/{votingSessionId}")]
    public async Task<IActionResult> GetResults(int votingSessionId)
    {
        var session = await _context.VotingSessions
            .Include(v => v.Votes)
            .ThenInclude(v => v.Restaurant)
            .FirstOrDefaultAsync(v => v.Id == votingSessionId);

        if (session == null)
        {
            return NotFound("Voting session not found.");
        }

        var results = session.Votes
            .GroupBy(v => v.RestaurantId)
            .Select(group => new
            {
                RestaurantId = group.Key,
                RestaurantName = group.First().Restaurant.Name,
                VoteCount = group.Count()
            })
            .OrderByDescending(r => r.VoteCount)
            .ToList();

        if (!results.Any())
        {
            return Ok(new { message = "No votes have been cast yet." });
        }

        return Ok(results);
    }
}
