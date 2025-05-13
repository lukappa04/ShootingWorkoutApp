using SWBackend.Models.SignUp.PersonalData;

namespace SWBackend.Models.Workout;

public class WorkoutM : IObject
{
    public int Id { get; set; }

    public PersonalDataM? PDUser { get; set; }
    public int UserId { get; set; }

    public string WorkoutName { get; set; } = string.Empty;
    public bool IsCustom { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<WorkoutPositionM> Positions { get; set; } = new List<WorkoutPositionM>();
}