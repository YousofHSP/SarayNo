using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class UnverifiedInvoice : BaseEntity
{
    public int ProjectId { get; set; }
    public int CostGroupId { get; set; }
    public int CreditorId { get; set; }
    public float Amount { get; set; }
    public UnverifiedInvoiceStatus Status { get; set; }
    public DateTimeOffset Date { get; set; }
    public string? Description { get; set; }
    
    public Creditor Creditor { get; set; }
    public CostGroup CostGroup{ get; set; }
    public Project Project { get; set; }

    public List<UnsettledInvoice> UnsettledInvoices { get; set; }
    
}
public class UnverifiedInvoiceConfiguration : IEntityTypeConfiguration<UnverifiedInvoice>
{
    public void Configure(EntityTypeBuilder<UnverifiedInvoice> builder)
    {
    }
}

public enum UnverifiedInvoiceStatus
{
    [Display(Name = "رد شده")]
    Rejected,
    [Display(Name = "تایید شده")]
    Approved,
    [Display(Name = "در انتظار تایید")]
    Pending
}