using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWBackend.Models.Workout;

public class PositionM : IObject
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Position_X { get; set; }
    public double Position_Y { get; set; }

    public ICollection<WorkoutPositionM> UsedInWorkouts { get; set; } = new List<WorkoutPositionM>();
}