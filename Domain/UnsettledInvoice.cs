using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class UnsettledInvoice : BaseEntity
{
    public int? ActivityId { get; set; }
    public int? UnverifiedInvoiceId { get; set; }
    public int CostGroupId { get; set; }
    public int CreditorId { get; set; }
    public DateTimeOffset Date { get; set; }
    public float RemainAmount { get; set; }
    public string? Description { get; set; }
    public float Discount { get; set; }
    public UnsettledInvoiceStatus Status { get; set; }
    
    public Creditor Creditor { get; set; }
    public CostGroup CostGroup{ get; set; }
    public UnverifiedInvoice? UnverifiedInvoice{ get; set; }
    public Activity? Activity{ get; set; }

    public List<Payoff> Payoffs{ get; set; }
}

public enum UnsettledInvoiceStatus
{
    [Display(Name = "در انتظار تسویه")]
    Pending,
    [Display(Name = "تسویه شده")]
    Done
}
public class UnsettledInvoiceConfiguration : IEntityTypeConfiguration<UnsettledInvoice>
{
    public void Configure(EntityTypeBuilder<UnsettledInvoice> builder)
    {

        builder.Property(i => i.Discount).HasDefaultValue(0);
        builder.HasOne(i => i.CostGroup)
            .WithMany(i => i.UnsettledInvoices)
            .HasForeignKey(i => i.CostGroupId);
        builder.HasOne(i => i.Creditor)
            .WithMany(i => i.UnsettledInvoices)
            .HasForeignKey(i => i.CreditorId);
        builder.HasOne(i => i.Activity)
            .WithMany(i => i.UnsettledInvoices)
            .HasForeignKey(i => i.ActivityId);
        builder.HasOne(i => i.UnverifiedInvoice)
            .WithMany(i => i.UnsettledInvoices)
            .HasForeignKey(i => i.UnverifiedInvoiceId);
        builder.HasMany(i => i.Payoffs)
            .WithOne(i => i.UnsettledInvoice)
            .HasForeignKey(i => i.UnsettledInvoiceId);
    }
}