using Microsoft.EntityFrameworkCore;
using Oranges.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Users> Users { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Meals> Meals { get; set; }
    public DbSet<Orders> TodayOrders { get; set; }
    public DbSet<VotingSession> VotingSessions { get; set; }
    public DbSet<Vote> Votes { get; set; }
}
