using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWBackend.Models.Workout;

public class WorkoutPositionM : IObject
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public WorkoutM? WorkId { get; set; }
    public int WorkoutId { get; set; }

    public PositionM? WPosition { get; set; }
    public int PositionId { get; set; }

    public int MaxShot { get; set; }
}