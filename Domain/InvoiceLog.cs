using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class InvoiceLog : BaseEntity
{

    public int InvoiceId { get; set; }
    public int CreatorUserId { get; set; }
    public InvoiceStatus Status { get; set; }
    public string? Description { get; set; }
    public Invoice Invoice { get; set; }
    public User User { get; set; }
}

public class InvoiceLogConfiguration : IEntityTypeConfiguration<InvoiceLog>
{
    public void Configure(EntityTypeBuilder<InvoiceLog> builder)
    {
        builder.HasOne(i => i.Invoice)
            .WithMany(i => i.InvoiceLogs)
            .HasForeignKey(i => i.InvoiceId);
        builder.HasOne(i => i.User)
            .WithMany(i => i.InvoiceLogs)
            .HasForeignKey(i => i.CreatorUserId);
    }
}