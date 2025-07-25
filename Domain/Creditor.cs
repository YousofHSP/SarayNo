using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;


[Display(Name = "بستانگار")]
public class Creditor : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? CardNumber { get; set; }

    public List<Invoice>? Invoices{ get; set; }

    public string GetFullName()
    {
        return FirstName + " " + LastName;
    }
}

public class CreditorConfiguration : IEntityTypeConfiguration<Creditor>
{
    public void Configure(EntityTypeBuilder<Creditor> builder)
    {
        builder.HasMany(i => i.Invoices)
            .WithOne(i => i.Creditor)
            .HasForeignKey(i => i.CreditorId);
    }
}