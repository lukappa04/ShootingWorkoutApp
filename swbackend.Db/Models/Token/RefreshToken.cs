using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using swbackend.Db.DataBase;
using swbackend.Db.Models.SignUp.Identity;

namespace swbackend.Db.Models.Token;

public class RefreshToken : IEntityTypeConfiguration<RefreshToken>
{
    public Guid Id { get; set; }

    public AppUser? User { get; set; }
    public Guid UserId { get; set; }  // FK -> AspNetUsers.Id

    public string TokenHash { get; set; } = null!;
    
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
    [MaxLength(10000000)] public string? CreatedByIp { get; set; }
    
    // prop per la revocazione di un token durante una rotation
    public bool Revoked { get; set; }
    public DateTime? RevokedAt { get; set; }
    public string? RevokedByIp { get; set; }
    public string? ReplacedByTokenHash { get; set; }
    
    //Memorizza il browser o il dispositivo da cui il token è stato generato. Utile per sicurezza e log
    public string? UserAgent { get; set; }

    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsActive => !Revoked && !IsExpired;

    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_token", Schemas.RefreshTokens);
        builder.HasKey(x => x.Id);
        
        //FK
        builder
            .HasOne(r => r.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        //Hash prop
        builder
            .Property(r => r.TokenHash)
            .HasMaxLength(128);
    }
}