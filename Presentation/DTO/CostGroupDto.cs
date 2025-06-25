using System.ComponentModel.DataAnnotations;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain;
using Presentation.Attributes;
using Presentation.Models;

namespace Presentation.DTO;

public class CostGroupDto : BaseDto<CostGroupDto, CostGroup>
{
    [Display(Name = "نام")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [MaxLength(100)]
    [Field(FieldType.Text )]
    public string Title { get; set; }
    
}

public class CostGroupResDto : BaseDto<CostGroupResDto, CostGroup>
{
    [Display(Name = "عنوان")]
    public string Title { get; set; }
}