using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWBackend.Models.Workout;

public class WorkoutPositionM : IObject
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public WorkoutM Workout { get; set; } = null!;
    public int WorkoutId { get; set; }

    public PositionM Position { get; set; }
    public int PositionId { get; set; }


    public int MaxShot { get; set; }
}