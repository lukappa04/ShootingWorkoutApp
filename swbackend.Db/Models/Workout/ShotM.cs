using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using swbackend.Db.DataBase;

namespace swbackend.Db.Models.Workout;

public class ShotM : IEntityTypeConfiguration<ShotM>
{
    public Guid Id { get; set ; }
    public Guid SessionId { get; set; }
    public Guid PositionId { get; set; }
    public int Made { get; set; }
    
    public WorkoutM? WSession { get; set; }
    public PositionM? WPosition { get; set; }


    public void Configure(EntityTypeBuilder<ShotM> builder)
    {
        builder.ToTable("shot", Schemas.Workouts);
        
        builder.HasKey(x => x.Id);
        
        builder
            .HasOne(s => s.WPosition)
            .WithMany(s => s.PositionShots)
            .HasForeignKey(s => s.PositionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(s => s.WSession)
            .WithMany(s => s.Shots)
            .HasForeignKey(s => s.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}