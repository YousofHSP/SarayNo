using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class Activity : BaseEntity
{
    public int ProjectId { get; set; }
    public int CostGroupId { get; set; }
    public int CreditorId { get; set; }
    public string Description { get; set; }

    public CostGroup CostGroup { get; set; }
    public Creditor Creditor{ get; set; }
    public List<ActivityDetail> Details { get; set; }
    public Project Project { get; set; }
}

public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.HasOne(i => i.CostGroup)
            .WithMany(i => i.Activities)
            .HasForeignKey(i => i.CostGroupId);
        builder.HasOne(i => i.Creditor)
            .WithMany(i => i.Activities)
            .HasForeignKey(i => i.CreditorId);
        builder.HasMany(i => i.Details)
            .WithOne(i => i.Activity)
            .HasForeignKey(i => i.ActivityId);
        builder.HasOne(i => i.Project)
            .WithMany(i => i.Activities)
            .HasForeignKey(i => i.ProjectId);
    }
}