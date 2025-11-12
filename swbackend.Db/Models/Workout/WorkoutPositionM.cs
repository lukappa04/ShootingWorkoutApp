using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using swbackend.Db.DataBase;

namespace swbackend.Db.Models.Workout;

public class WorkoutPositionM : IEntityTypeConfiguration<WorkoutPositionM>
{
    public Guid Id { get; set; }
    public Guid WorkoutId { get; set; }
    public Guid PositionId { get; set; }
    public Guid MaxShot { get; set; }
    

    public WorkoutM Workout { get; set; } = null!;
    public PositionM Position { get; set; } = null!;


    public void Configure(EntityTypeBuilder<WorkoutPositionM> builder)
    {
        builder.ToTable("workout_position", Schemas.Workouts);
        
        builder.HasKey(x => x.Id);
        
        //Imposto FK
        builder
            .HasOne(wp => wp.Workout)
            .WithMany(w => w.Positions)
            .HasForeignKey(wp => wp.WorkoutId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(wp => wp.Position)
            .WithMany(wp => wp.UsedInWorkouts)
            .HasForeignKey(wp => wp.PositionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasAlternateKey(wp => new
                { wp.WorkoutId, wp.PositionId }); // VINCOLO: una posizione può comparire una volta sola per workout
    }
}