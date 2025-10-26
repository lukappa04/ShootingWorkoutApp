using System.ComponentModel.DataAnnotations.Schema;
using SWBackend.Models.SignUp.Identity;

namespace SWBackend.Models.Workout;

public class WorkoutM : IObject
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public AppUser AppUser { get; set; }
    public int? UserId { get; set; }

    public string WorkoutName { get; set; } = string.Empty;
    public bool IsCustom { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeleteDate { get; set; }

    public ICollection<WorkoutPositionM> Positions { get; set; } = new List<WorkoutPositionM>();
    public ICollection<WorkoutSessionM> Sessions { get; set; } = new List<WorkoutSessionM>();
    public ICollection<ShotM> SessionShots { get; set; } = new List<ShotM>();
}