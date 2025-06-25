using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

[Display(Name = "کاربران")]
public class User : IdentityUser<int>, IEntity<int>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string NationalCode { get; set; }
    public GenderType Gender { get; set; }
    public UserStatus Status { get; set; }
    public DateTimeOffset LastLoginDate { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

    public List<Role> Roles { get; set; }
    public List<UploadedFile> UploadedFiles { get; set; }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(user => user.UserName).IsRequired().HasMaxLength(100);
        builder.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(u => u.NationalCode).IsRequired().HasMaxLength(10);
        builder.HasIndex(u => u.NationalCode).IsUnique();
        builder.HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<IdentityUserRole<int>>();
        builder.HasMany(i => i.UploadedFiles)
            .WithOne(i => i.User)
            .HasForeignKey(i => i.UserId);
    }
}

public enum GenderType
{
    [Display(Name = "مرد")] Male = 1,
    [Display(Name = "زن")] Female = 2,
}

public enum UserStatus
{
    [Display(Name = "فعال")] Active = 1,
    [Display(Name = "غیرفعال")] Disable = 0,
}