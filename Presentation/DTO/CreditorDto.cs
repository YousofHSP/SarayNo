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
}

public class CreditorResDto : BaseDto<CreditorResDto, Creditor>
{
    [Display(Name = "نام")]
    public string FirstName { get; set; }
    [Display(Name = "نام خانوادگی")]
    public string LastName { get; set; }
}