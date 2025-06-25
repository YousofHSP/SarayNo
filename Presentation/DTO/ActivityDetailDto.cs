using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Common.Utilities;
using Domain;
using Presentation.Attributes;
using Presentation.Models;

namespace Presentation.DTO;

public class ActivityDetailDto : BaseDto<ActivityDetailDto, ActivityDetail>
{

    [Field(FieldType.Hidden)]
    public int ActivityId { get; set; }
    
    [Display(Name = "نوع")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [Field(FieldType.Select)]
    public ActivityDetailType Type { get; set; }
    
    [Display(Name = "شماره")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [Field(FieldType.Text)]
    public string Number { get; set; }
    
    [Display(Name = "تاریخ")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [Field(FieldType.DateTime)]
    public string Date { get; set; }
    
    [Display(Name = "مبلغ")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [Field(FieldType.Number)]
    public float Price { get; set; }

    protected override void CustomMappings(IMappingExpression<ActivityDetailDto, ActivityDetail> mapping)
    {
        mapping.ForMember(
            m => m.Date,
            s => s.MapFrom(d => d.Date.ToGregorian()));
    }
}

public class ActivityDetailResDto : BaseDto<ActivityDetailResDto, ActivityDetail>
{
    
    public string ActivityTitle { get; set; }
    public ActivityDetailType Type { get; set; }
    public string Number { get; set; }
    public string Date { get; set; }
    public float Price { get; set; }
}