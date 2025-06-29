using SWBackend.Models.SignUp.Identity;

namespace SWBackend.Models.Workout;

public class WorkoutM : IObject
{
    public int Id { get; set; }

    public AppUser? PDUser { get; set; }
    public int UserId { get; set; }

    public string WorkoutName { get; set; } = string.Empty;
    public bool IsCustom { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DeleteDate { get; set; }

    public ICollection<WorkoutPositionM> Positions { get; set; } = new List<WorkoutPositionM>();
}