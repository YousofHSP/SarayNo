using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Common.Utilities;
using Domain;
using Presentation.Attributes;
using Presentation.Models;

namespace Presentation.DTO;

public class ActivityDto : BaseDto<ActivityDto, Activity>
{

    [Display(Name = "پروژه")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [Field(FieldType.Select)]
    public int ProjectId { get; set; }
    [Display(Name = "گروه هزینه")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [Field(FieldType.Select)]
    public int CostGroupId { get; set; }
    
    [Display(Name = "بستانکار")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [Field(FieldType.Select)]
    public int CreditorId { get; set; }
    
    [Display(Name = "توضیحات")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [Field(FieldType.Text)]
    public string Description { get; set; }
    
    public string Date { get; set; }
    protected override void CustomMappings(IMappingExpression<ActivityDto, Activity> mapping)
    {
        mapping.ForMember(
            m => m.Date,
            s => s.MapFrom(d => d.Date.ToGregorian()));
    }
}

public class ActivityResDto : BaseDto<ActivityResDto, Activity>
{

    [Display(Name = "پروژه")]
    public string ProjectTitle { get; set; }
    
    [Display(Name = "گروه هزینه")]
    public string CostGroupTitle { get; set; }
    
    [Display(Name = "بستانکار")]
    public string CreditorFullName { get; set; }
    
    [Display(Name = "توضیحات")]
    public string Description { get; set; }
    
    [Display(Name = "علی الحساب")]
    public string OnAccount { get; set; }
    public string Date { get; set; }

    protected override void CustomMappings(IMappingExpression<Activity, ActivityResDto> mapping)
    {
        mapping.ForMember(
            d => d.CreditorFullName,
            s => s.MapFrom(m => $"{m.Creditor.FirstName} {m.Creditor.LastName}"));
        mapping.ForMember(
            d => d.OnAccount,
            s => s.MapFrom(m => m.Details.Sum(i => i.Price).ToNumeric()));
        mapping.ForMember(
            d => d.Date,
            s => s.MapFrom(m => m.Date.ToShamsi()));
    }
}

public class AddImageDto
{
    public int ModelId { get; set; }
    public int? AlbumId { get; set; }
    public IFormFile File { get; set; }
    public string Description { get; set; }
    public string? ReturnUrl{ get; set; }
}