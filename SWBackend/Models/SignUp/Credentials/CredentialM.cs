using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SWBackend.Models.SignUp.PersonalData;

namespace SWBackend.Models.SignUp.Credentials;

public class CredentialM : IObject
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public PersonalDataM? User { get; set; }
    public int UserId { get; set; }
    
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int RoleCode { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastUpdateAt { get; set; }
}