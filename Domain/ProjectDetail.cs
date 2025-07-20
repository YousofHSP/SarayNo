using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ProjectDetail : BaseEntity
{
    public int ProjectId { get; set; }
    public string Title { get; set; }
    public int Percent { get; set; }
    public DateTimeOffset Date { get; set; }
    public string? Descriptin { get; set; }

    public Project Project { get; set; }
    public List<ProjectDetailItem> ProjectDetailItems { get; set; }
}


public class ProjectDetailConfiguration : IEntityTypeConfiguration<ProjectDetail>
{
    public void Configure(EntityTypeBuilder<ProjectDetail> builder)
    {
        builder.HasOne(i => i.Project)
            .WithMany(i => i.Details)
            .HasForeignKey(i => i.ProjectId);
        builder.HasMany(i => i.ProjectDetailItems)
            .WithOne(i => i.ProjectDetail)
            .HasForeignKey(i => i.ProjectDetailId);
    }
}