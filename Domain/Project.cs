using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class Project : BaseEntity
{

    public string Title { get; set; }
    public decimal Percent { get; set; }
    public int UserId { get; set; }
    public DateTimeOffset Date { get; set; }
    public List<ProjectDetail> Details { get; set; } = new();
    public List<EmployerPayment> EmployerPayments{ get; set; }
    public List<Invoice> Invoices { get; set; } = new();
    public List<Payoff> Payoffs{ get; set; } = new();
    public User User { get; set; }

}
public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasMany(i => i.Details)
            .WithOne(i => i.Project)
            .HasForeignKey(i => i.ProjectId);
        builder.HasMany(i => i.Invoices)
            .WithOne(i => i.Project)
            .HasForeignKey(i => i.ProjectId);
        builder.HasMany(i => i.Payoffs)
            .WithOne(i => i.Project)
            .HasForeignKey(i => i.ProjectId);
        builder.HasMany(i => i.EmployerPayments)
            .WithOne(i => i.Project)
            .HasForeignKey(i => i.ProjectId);
        builder.HasOne(i => i.User)
            .WithMany(i => i.Projects)
            .HasForeignKey(i => i.UserId);
    }
}