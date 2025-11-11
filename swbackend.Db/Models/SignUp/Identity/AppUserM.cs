using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using swbackend.Db.DataBase;
using swbackend.Db.Enum;
using swbackend.Db.Models.Token;
using swbackend.Db.Models.Workout;

namespace swbackend.Db.Models.SignUp.Identity;

public class AppUser : IdentityUser<Guid>, IEntityTypeConfiguration<AppUser>
{
    //Dati Anagrafici
    [MaxLength(256)] public string Name { get; set; } = string.Empty;
    [MaxLength(256)] public string Surname { get; set; } = string.Empty;
    public Guid Age { get; set; }
    public DateOnly BirthDay { get; set; }

    //Ruolo Custom
    public Role RoleCode { get; set; }

    //metadata utente
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdateAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeleteDate { get; set; }

    public ICollection<WorkoutM> Workouts { get; set; } = [];
    public ICollection<RefreshToken>? RefreshTokens { get; set; } = [];


    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable("appuser", Schemas.Users);
        
        builder.HasKey(u => u.Id);
        
        builder.HasQueryFilter(u => u.DeleteDate == null);
        builder.HasIndex(u => u.NormalizedUserName).IsUnique();
        builder.HasIndex(u => u.NormalizedEmail).IsUnique(); 
        builder.Property(o => o.RoleCode).HasConversion<string>();
    }
}