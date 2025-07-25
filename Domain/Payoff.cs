using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class Payoff : BaseEntity
{
    public int InvoiceId { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public PayoffType Type { get; set; }
    public DateTimeOffset Date { get; set; }
    
    public Invoice Invoice{ get; set; }
    
}
public class PayoffConfiguration : IEntityTypeConfiguration<Payoff>
{
    public void Configure(EntityTypeBuilder<Payoff> builder)
    {
        builder.HasOne(i => i.Invoice)
            .WithMany(i => i.Payoffs)
            .HasForeignKey(i => i.InvoiceId);

    }
}

public enum PayoffType
{
    [Display(Name = "نقدی")]
    Cash,
    [Display(Name = "چک")]
    Check,
    [Display(Name = "پرداختی کارفرما")]
    EmployerPayment
}