using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class Activity : BaseEntity
{
    public int ProjectId { get; set; }
    public int CostGroupId { get; set; }
    public int CreditorId { get; set; }
    public float TotalAmount { get; set; }
    public DateTimeOffset Date{ get; set; }
    public string? Description { get; set; }
    public bool IsDone { get; set; }

    public CostGroup CostGroup { get; set; }
    public Creditor Creditor{ get; set; }
    public List<ActivityDetail> Details { get; set; } = new();
    public Project Project { get; set; }
    public List<UnsettledInvoice> UnsettledInvoices{ get; set; }
}

public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {

    }
}