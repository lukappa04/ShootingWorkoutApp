using Microsoft.EntityFrameworkCore;
using SWBackend.Models.SignUp.Credentials;
using SWBackend.Models.SignUp.PersonalData;
using SWBackend.Models.Workout;

namespace SWBackend.DataBase;

public class SWDbContext : DbContext
{
    public SWDbContext(DbContextOptions<SWDbContext> options) : base(options){}

    //DbSet per ogni mia classe
    public DbSet<CredentialM> Credentials {get; set;}
    public DbSet<PersonalDataM> PersonalDatas { get; set; }
    public DbSet<PositionM> Positions { get; set; }
    public DbSet<ShotM> Shots { get; set; }
    public DbSet<WorkoutM> Workouts { get; set; }
    public DbSet<WorkoutPositionM> WorkoutPositions { get; set; }
    public DbSet<WorkoutSessionM> WorkoutSessions { get; set; }

    //configuro i miei campi del db
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region Signup
        //Credentials
        modelBuilder.Entity<CredentialM>()
        .HasIndex(c => c.Email)
        .IsUnique();

        modelBuilder.Entity<CredentialM>()
        .HasIndex(c => c.Username)
        .IsUnique();

        modelBuilder.Entity<CredentialM>()
        .Property(c => c.CreationDate)
        .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<CredentialM>()
        .HasOne(c => c.User)
        .WithOne() // .WithOne() se la relazione è 1:1
        .HasForeignKey<CredentialM>(c => c.UserId)
        .OnDelete(DeleteBehavior.Cascade); 
        #endregion

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