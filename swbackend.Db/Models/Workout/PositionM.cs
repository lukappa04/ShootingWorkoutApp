using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using swbackend.Db.DataBase;

namespace swbackend.Db.Models.Workout;

public class PositionM : IEntityTypeConfiguration<PositionM>
{
    public Guid Id { get; set; }
    [MaxLength(256)] public string Name { get; set; } = string.Empty;
    public double PositionX { get; set; }
    public double PositionY { get; set; }

    public ICollection<WorkoutPositionM> UsedInWorkouts { get; set; } = new List<WorkoutPositionM>();
    public ICollection<ShotM> PositionShots { get; set; } = new List<ShotM>();
    
    public void Configure(EntityTypeBuilder<PositionM> builder)
    {
        builder.ToTable("position", Schemas.Workouts);
        
        builder.HasKey(x => x.Id);
        
    }
}