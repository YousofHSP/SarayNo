using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain;

public class InvoiceDetail : BaseEntity
{

    public int InvoiceId { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset Date { get; set; }
    public InvoiceDetailStatus Status { get; set; }
    public InvoiceDetailType Type { get; set; }

    public Invoice Invoice { get; set; }
}
public enum InvoiceDetailType 
{
    [Display(Name = "چک")]
    Check,
    
    [Display(Name = "پرداختی کارفرما")]
    EmployerPayment,
    
    [Display(Name = "وجه امانی")]
    TrustFund
}
public enum InvoiceDetailStatus
{
    [Display(Name = "در انتظار تایید")]
    Pending,
    [Display(Name = "تایید شده")]
    Approved
}
