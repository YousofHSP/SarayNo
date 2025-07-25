using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class Invoice : BaseEntity
{
    public int ProjectId { get; set; }
    public int CostGroupId { get; set; }
    public int CreditorId { get; set; }
    public decimal Amount { get; set; }
    public decimal Discount { get; set; }
    public InvoiceType Type { get; set; }
    public InvoiceStatus Status { get; set; }
    public DateTimeOffset Date { get; set; }
    public string? Description { get; set; }

    public CostGroup CostGroup { get; set; }
    public Creditor Creditor{ get; set; }
    public Project Project { get; set; }
    public List<InvoiceDetail> InvoiceDetails { get; set; } = new();
    public List<Payoff>? Payoffs{ get; set; }
    public List<InvoiceLog>? InvoiceLogs { get; set; }
}

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasOne(i => i.CostGroup)
            .WithMany(i => i.Invoices)
            .HasForeignKey(i => i.CostGroupId);
        builder.HasOne(i => i.Creditor)
            .WithMany(i => i.Invoices)
            .HasForeignKey(i => i.CreditorId);
        builder.HasOne(i => i.Project)
            .WithMany(i => i.Invoices)
            .HasForeignKey(i => i.ProjectId);
        builder.HasMany(i => i.InvoiceDetails)
            .WithOne(i => i.Invoice)
            .HasForeignKey(i => i.InvoiceId);
        builder.HasMany(i => i.Payoffs)
            .WithOne(i => i.Invoice)
            .HasForeignKey(i => i.InvoiceId);
        builder.HasMany(i => i.InvoiceLogs)
            .WithOne(i => i.Invoice)
            .HasForeignKey(i => i.InvoiceId);

    }
}
public enum InvoiceType
{
    [Display(Name = "فعالیت های در حال انجام")]
    Activity,
    [Display(Name = "فاکتور های تایید نشده")]
    Unverified,
    [Display(Name = "فاکتور های تسویه نشده")]
    Unsettled,
    [Display(Name = "فاکتور های تسویه شده")]
    Settled
}

public enum InvoiceStatus
{
    [Display(Name = "در انتظار بررسی")]
    Pending,
    [Display(Name = "تایید شده")]
    Approved,
    [Display(Name = "رد شده")]
    Rejected
}