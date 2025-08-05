using System.ComponentModel.DataAnnotations;
using Domain;
using Presentation.Attributes;
using Presentation.Models;

namespace Presentation.DTO;

public class CreditorDto : BaseDto<CreditorDto, Creditor>
{

    [Display(Name = "نام")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [MaxLength(100)]
    [Field(FieldType.Text )]
    public string FirstName { get; set; }
    
    [Display(Name = "نام خانوادگی")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [MaxLength(100)]
    [Field(FieldType.Text )]
    public string LastName { get; set; }
    
    [Display(Name = "شماره تماس")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [MaxLength(100)]
    [Field(FieldType.Text )]
    public string PhoneNumber { get; set; }
    
    [Display(Name = "شماره کارت")]
    [MaxLength(100)]
    [Field(FieldType.Text )]
    public string? CardNumber { get; set; }
}

public class CreditorResDto : BaseDto<CreditorResDto, Creditor>
{
    [Display(Name = "نام")]
    public string FirstName { get; set; }
    [Display(Name = "نام خانوادگی")]
    public string LastName { get; set; }
    [Display(Name = "شماره تماس")]
    public string PhoneNumber { get; set; }
    [Display(Name = "شماره کارت")]
    public string CardNumber { get; set; }
}

public class CreditorDebtDto
{
    public string CreditorFullName { get; set; }
    public string Title { get; set; }
    public decimal Amount { get; set; }
    public string AmountNumeric { get; set; }
    public string Description{ get; set; }
    public string Date { get; set; }
    public DateTimeOffset DateTime { get; set; }
}