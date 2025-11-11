using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SWBackend.Models.Log;
using SWBackend.Models.SignUp.Identity;
using SWBackend.Models.Token;
using SWBackend.Models.Workout;

namespace SWBackend.DataBase;

public class SwDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
{
    public SwDbContext(DbContextOptions<SwDbContext> options) : base(options){}

    //DbSet per ogni mia classe
    public DbSet<PositionM> Positions { get; set; }
    public DbSet<ShotM> Shots { get; set; }
    public DbSet<WorkoutM> Workouts { get; set; }
    public DbSet<WorkoutPositionM> WorkoutPositions { get; set; }
    public DbSet<WorkoutSessionM> WorkoutSessions { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<UserActionLog> UserActionLogs { get; set; }

    //configuro i miei campi del db
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SwDbContext).Assembly);
    }
}