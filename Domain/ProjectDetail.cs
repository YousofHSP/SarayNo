using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ProjectDetail : BaseEntity
{
    public int ProjectId { get; set; }
    public string Title { get; set; }
    public float? UnitPrice { get; set; }
    public float? Area { get; set; }
    public float TotalPrice { get; set; }
    public ProjectDetailType Type { get; set; }
    public DateTimeOffset Date { get; set; }

    public Project Project { get; set; }
}

public enum ProjectDetailType
{
    [Display(Name = "تخمین")]
    Estimate,
    [Display(Name = "نهایی")]
    Final
}

public class ProjectDetailConfiguration : IEntityTypeConfiguration<ProjectDetail>
{
    public void Configure(EntityTypeBuilder<ProjectDetail> builder)
    {
        builder.HasOne(i => i.Project)
            .WithMany(i => i.Details)
            .HasForeignKey(i => i.ProjectId);
    }
}