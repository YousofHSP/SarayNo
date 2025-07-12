using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class EmployerPayment : BaseEntity
{

    public DateTimeOffset Date { get; set; }
    public float Amount { get; set; }
    public string? Description { get; set; }
    public int ProjectId { get; set; }

    public Project Project { get; set; }
}

public class EmployerPaymentConfiguration : IEntityTypeConfiguration<EmployerPayment>
{
    public void Configure(EntityTypeBuilder<EmployerPayment> builder)
    {
        builder.HasOne(i => i.Project)
            .WithMany(i => i.EmployerPayments)
            .HasForeignKey(i => i.ProjectId);
    }
}