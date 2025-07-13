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
    
    public List<Activity> Activities { get; set; }
    public List<UnverifiedInvoice> UnverifiedInvoices { get; set; }
    public List<UnsettledInvoice> UnsettledInvoices { get; set; }
}

public class CreditorConfiguration : IEntityTypeConfiguration<Creditor>
{
    public void Configure(EntityTypeBuilder<Creditor> builder)
    {
        builder.HasMany(i => i.Activities)
            .WithOne(i => i.Creditor)
            .HasForeignKey(i => i.CreditorId);
        builder.HasMany(i => i.UnverifiedInvoices)
            .WithOne(i => i.Creditor)
            .HasForeignKey(i => i.CreditorId);
        builder.HasMany(i => i.UnsettledInvoices)
            .WithOne(i => i.Creditor)
            .HasForeignKey(i => i.CreditorId);
    }
}