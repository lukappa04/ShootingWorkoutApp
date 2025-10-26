using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWBackend.Models.Workout;

public class ShotM : IObject
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set ; }
    
    public WorkoutM? WSession { get; set; }
    public int SessionId { get; set; }

    public PositionM? WPosition { get; set; }
    public int PositionId { get; set; }

    public int Made { get; set; }
   
}