using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class Project : BaseEntity
{

    public string Title { get; set; }
    public float Percent { get; set; }
    public DateTimeOffset Date { get; set; }
    public float EstimatePrice { get; set; }
    public float FinalPrice { get; set; }

    public List<ProjectDetail> Details { get; set; }
    public List<Activity> Activities { get; set; }
}
public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasMany(i => i.Details)
            .WithOne(i => i.Project)
            .HasForeignKey(i => i.ProjectId);
        builder.HasMany(i => i.Activities)
            .WithOne(i => i.Project)
            .HasForeignKey(i => i.ProjectId);
    }
}