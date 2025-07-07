using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class Payoff : BaseEntity
{
    public int UnsettledInvoiceId { get; set; }
    public float Price { get; set; }
    public PayoffType Type { get; set; }
    public string Number { get; set; }
    public DateTimeOffset Date { get; set; }
    
    public UnsettledInvoice UnsettledInvoice { get; set; }
    
}
public class PayoffConfiguration : IEntityTypeConfiguration<Payoff>
{
    public void Configure(EntityTypeBuilder<Payoff> builder)
    {
        builder.Property(i => i.Number).HasDefaultValue("");
        builder.HasOne(i => i.UnsettledInvoice)
            .WithMany(i => i.Payoffs)
            .HasForeignKey(i => i.UnsettledInvoiceId);

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