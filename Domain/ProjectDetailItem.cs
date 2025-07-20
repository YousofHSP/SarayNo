using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class ProjectDetailItem : BaseEntity
{

    public string Title { get; set; }
    public float? UnitPrice { get; set; }
    public float? Area { get; set; }
    public string? Description { get; set; }
    public float TotalPrice { get; set; }
    public ProjectDetailItemType Type { get; set; }
    public int ProjectDetailId { get; set; }
    [JsonIgnore]
    public ProjectDetail ProjectDetail { get; set; }
}
public class ProjectDetailItemConfiguration : IEntityTypeConfiguration<ProjectDetailItem>
{
    public void Configure(EntityTypeBuilder<ProjectDetailItem> builder)
    {
        builder.HasOne(i => i.ProjectDetail)
            .WithMany(i => i.ProjectDetailItems)
            .HasForeignKey(i => i.ProjectDetailId);

    }
}

public enum ProjectDetailItemType
{
    [Display(Name = "تخمین")]
    Estimate,
    [Display(Name = "نهایی")]
    Final
}
