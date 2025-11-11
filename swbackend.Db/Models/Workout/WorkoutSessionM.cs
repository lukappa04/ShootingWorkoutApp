using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SWBackend.DataBase;

namespace SWBackend.Models.Workout;

public class WorkoutSessionM : IEntityTypeConfiguration<WorkoutSessionM>
{
    public Guid Id { get; set; }
    public Guid WorkoutId { get; set; }

    public WorkoutM Workouts { get; set; } = null!;

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime CreatedAt { get; set; }


    public void Configure(EntityTypeBuilder<WorkoutSessionM> builder)
    {
        builder.ToTable("workout_session", Schemas.Workouts);
        
        builder.HasKey(x => x.Id);
        
        //FK
        builder
            .HasOne(w => w.Workouts)
            .WithMany(w => w.Sessions)
            .HasForeignKey(w => w.WorkoutId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}