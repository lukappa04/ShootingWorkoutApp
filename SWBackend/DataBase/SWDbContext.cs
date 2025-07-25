using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SWBackend.Models.SignUp.Identity;
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

    //configuro i miei campi del db
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        base.OnModelCreating(modelBuilder);
        //TODO: Cosa fare con questo comando???

        modelBuilder.Entity<AppUser>().HasQueryFilter(u => u.DeleteDate == null);
        modelBuilder.Entity<AppUser>().HasIndex(u => u.NormalizedUserName).IsUnique();
        modelBuilder.Entity<AppUser>().HasIndex(u => u.NormalizedEmail).IsUnique(); 
        modelBuilder.Entity<AppUser>().Property(o => o.RoleCode).HasConversion<string>();
 
        #region Workout
        modelBuilder.Entity<WorkoutM>()
        .HasIndex(w => w.WorkoutName)
        .IsUnique();

        #region FK
        //ShotM
        modelBuilder.Entity<ShotM>()
        .HasOne(s => s.WPosition)
        .WithMany()
        .HasForeignKey(s => s.PositionId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ShotM>()
        .HasOne(s => s.WSession)
        .WithMany()
        .HasForeignKey(s => s.SessionId)
        .OnDelete(DeleteBehavior.Cascade);

        //WorkoutPositionM
        modelBuilder.Entity<WorkoutPositionM>()
        .HasOne(s => s.WPosition)
        .WithMany()
        .HasForeignKey(s => s.PositionId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WorkoutPositionM>()
        .HasOne(w => w.WorkId)
        .WithMany()
        .HasForeignKey(w => w.WorkoutId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WorkoutPositionM>()
        .HasIndex(wp => new { wp.WorkoutId, wp.PositionId })
        .IsUnique(); // VINCOLO: una posizione può comparire una volta sola per workout

        //WorkoutM
        modelBuilder.Entity<WorkoutM>()
        .HasOne(w => w.PDUser)
        .WithMany()
        .HasForeignKey(w => w.UserId)
        .OnDelete(DeleteBehavior.Cascade);

        //Workout Session
        modelBuilder.Entity<WorkoutSessionM>()
        .HasOne(w => w.WorkId)
        .WithOne()
        .HasForeignKey<WorkoutSessionM>(w => w.WorkoutId)
        .OnDelete(DeleteBehavior.Cascade);

        #endregion
        #endregion
    }
}