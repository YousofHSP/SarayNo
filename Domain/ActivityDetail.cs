using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Identity.Client;

namespace Domain;


public class ActivityDetail :BaseEntity
{

    public int ActivityId { get; set; }
    public ActivityDetailType Type { get; set; }
    public string Number { get; set; }
    public DateTimeOffset Date { get; set; }
    public float Price { get; set; }
    public ActivityDetailStatus Status { get; set; }

    public Activity Activity { get; set; }
}

public class ActivityDetailConfiguration : IEntityTypeConfiguration<ActivityDetail>
{
    public void Configure(EntityTypeBuilder<ActivityDetail> builder)
    {
        builder.HasOne(i => i.Activity)
            .WithMany(i => i.Details)
            .HasForeignKey(i => i.ActivityId);
    }
}

public enum ActivityDetailStatus
{
    [Display(Name = "در انتظار تایید")]
    Pending,
    [Display(Name = "تایید شده")]
    Approved,
}
public enum ActivityDetailType
{
    [Display(Name = "چک")]
    Check,
    
    [Display(Name = "پرداختی کارفرما")]
    EmployerPayment,
    
    [Display(Name = "وجه امانی")]
    TrustFund
}