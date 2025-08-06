using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class Payoff : BaseEntity
{
    public int? ProjectId{ get; set; }
    public int? InvoiceId { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public PayoffType Type { get; set; }
    public DepositType DepositType{ get; set; }
    public PayoffStatus Status{ get; set; }
    public DateTimeOffset Date { get; set; }
    public DateTimeOffset? DueDate { get; set; }
    
    public Invoice? Invoice{ get; set; }
    public Project? Project{ get; set; }
    
}
public class PayoffConfiguration : IEntityTypeConfiguration<Payoff>
{
    public void Configure(EntityTypeBuilder<Payoff> builder)
    {
        builder.HasOne(i => i.Project)
            .WithMany(i => i.Payoffs)
            .HasForeignKey(i => i.ProjectId);
        builder.HasOne(i => i.Invoice)
            .WithMany(i => i.Payoffs)
            .HasForeignKey(i => i.InvoiceId);

    }
}

public enum PayoffStatus
{
    [Display(Name = "در انتظار تایید")]
    Pending,
    [Display(Name = "تایید شده")]
    Approved
}

public enum DepositType
{
    [Display(Name = "برداشت")]
    Decrease,
    [Display(Name = "افزایش")]
    Increalse
}
public enum PayoffType
{
    [Display(Name = "کارت نقدی")]
    Cash,
    [Display(Name = "چک")]
    Check,
    [Display(Name = "پرداختی کارفرما")]
    EmployerPayment
}