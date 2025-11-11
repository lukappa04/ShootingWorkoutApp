using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SWBackend.DataBase;
using SWBackend.Models.SignUp.Identity;

namespace SWBackend.Models.Workout;

public class WorkoutM : IEntityTypeConfiguration<WorkoutM>
{
    public Guid Id { get; set; }
    
    public AppUser? AppUser { get; set; }
    public Guid? UserId { get; set; }

    [MaxLength(256)] public string WorkoutName { get; set; } = string.Empty;
    public bool IsCustom { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeleteDate { get; set; }

    public ICollection<WorkoutPositionM> Positions { get; set; } = new List<WorkoutPositionM>();
    public ICollection<WorkoutSessionM> Sessions { get; set; } = new List<WorkoutSessionM>();
    public ICollection<ShotM> Shots { get; set; } = new List<ShotM>();
    public void Configure(EntityTypeBuilder<WorkoutM> builder)
    {
        builder.ToTable("workout_m", Schemas.Workouts);
        
        builder.HasKey(x => x.Id);
        
        builder
            .HasOne(x => x.AppUser)
            .WithMany(x => x.Workouts)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasAlternateKey(w => new { w.UserId, w.WorkoutName });
    }
}