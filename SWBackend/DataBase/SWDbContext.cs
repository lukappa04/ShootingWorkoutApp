using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SWBackend.Models.Log;
using SWBackend.Models.SignUp.Identity;
using SWBackend.Models.Token;
using SWBackend.Models.Workout;

namespace SWBackend.DataBase;

public class SWDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
{
    public SWDbContext(DbContextOptions<SWDbContext> options) : base(options){}

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
        //TODO: Cosa fare con questo comando???

        #region User
        modelBuilder.Entity<AppUser>().HasQueryFilter(u => u.DeleteDate == null);
        modelBuilder.Entity<AppUser>().HasIndex(u => u.NormalizedUserName).IsUnique();
        modelBuilder.Entity<AppUser>().HasIndex(u => u.NormalizedEmail).IsUnique(); 
        modelBuilder.Entity<AppUser>().Property(o => o.RoleCode).HasConversion<string>();
        #endregion
 
        #region Workout
        modelBuilder.Entity<WorkoutM>()
        .HasIndex(w => w.WorkoutName)
        .IsUnique();
        #endregion

        #region FK
        //ShotM
        modelBuilder.Entity<ShotM>()
        .HasOne(s => s.WPosition)
        .WithMany(s => s.PositionShots)
        .HasForeignKey(s => s.PositionId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ShotM>()
        .HasOne(s => s.WSession)
        .WithMany(s => s.SessionShots)
        .HasForeignKey(s => s.SessionId)
        .OnDelete(DeleteBehavior.Cascade);

        //WorkoutPositionM
        modelBuilder.Entity<WorkoutPositionM>()
            .HasOne(wp => wp.Workout)
            .WithMany(w => w.Positions)
            .HasForeignKey(wp => wp.WorkoutId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WorkoutPositionM>()
            .HasOne(wp => wp.Position)
            .WithMany(wp => wp.UsedInWorkouts)
            .HasForeignKey(wp => wp.PositionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WorkoutPositionM>()
        .HasIndex(wp => new { wp.WorkoutId, wp.PositionId })
        .IsUnique(); // VINCOLO: una posizione può comparire una volta sola per workout

        //WorkoutM
        modelBuilder.Entity<WorkoutM>()
        .HasOne(x => x.AppUser)
        .WithMany(x => x.Workouts)
        .HasForeignKey(w => w.UserId)
        .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<WorkoutM>()
            .Property(w => w.Id)
            .ValueGeneratedOnAdd();
        
        //Workout Session
        modelBuilder.Entity<WorkoutSessionM>()
        .HasOne(w => w.Workouts)
        .WithMany(w => w.Sessions)
        .HasForeignKey(w => w.WorkoutId)
        .OnDelete(DeleteBehavior.Cascade);
        #endregion
        
        
        #region RefreshToken
        modelBuilder.Entity<RefreshToken>()
            .HasOne(r => r.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RefreshToken>()
            .Property(r => r.TokenHash)
            .HasMaxLength(128);

        #endregion
    }
}