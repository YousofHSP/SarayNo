using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

[Display(Name = "گروه هزینه")]
public class CostGroup : BaseEntity
{
    public string Title { get; set; }

    public List<Activity> Activities { get; set; }
    public List<UnverifiedInvoice> UnverifiedInvoices { get; set; }
    public List<UnsettledInvoice> UnsettledInvoices{ get; set; }
}

public class CostGroupConfiguration : IEntityTypeConfiguration<CostGroup>
{
    public void Configure(EntityTypeBuilder<CostGroup> builder)
    {
        builder.HasMany(i => i.Activities)
            .WithOne(i => i.CostGroup)
            .HasForeignKey(i => i.CostGroupId);
        builder.HasMany(i => i.UnverifiedInvoices)
            .WithOne(i => i.CostGroup)
            .HasForeignKey(i => i.CostGroupId);
        builder.HasMany(i => i.UnsettledInvoices)
            .WithOne(i => i.CostGroup)
            .HasForeignKey(i => i.CostGroupId);
    }
}