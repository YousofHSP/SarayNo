using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Common.Utilities;
using Domain;
using Presentation.Attributes;
using Presentation.Models;

namespace Presentation.DTO;

public class ProjectDto : BaseDto<ProjectDto, Project>
{
    [Display(Name = "عنوان")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [MaxLength(100)]
    [Field(FieldType.Text)]
    public string Title { get; set; }

    [Display(Name = "درصد")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [Field(FieldType.Number)]
    public float Percent { get; set; }

    [Display(Name = "تاریخ")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [Field(FieldType.DateTime)]
    public string Date { get; set; }

    protected override void CustomMappings(IMappingExpression<ProjectDto, Project> mapping)
    {
        mapping.ForMember(
            m => m.Date,
            s => s.MapFrom(d => d.Date.ToGregorian()));
    }
}

public class ProjectResDto : BaseDto<ProjectResDto, Project>
{
    [Display(Name = "عنوان")] public string Title { get; set; }

    [Display(Name = "درصد")] public float Percent { get; set; }

    [Display(Name = "تاریخ")] public string Date { get; set; }

    [Display(Name = "قیمت تخمینی")] public float EstimatePrice { get; set; }

    [Display(Name = "قیمت نهایی")] public float FinalPrice { get; set; }

    [Display(Name = "جزئیات")] public List<ProjectDetail> Details { get; set; }

    protected override void CustomMappings(IMappingExpression<Project, ProjectResDto> mapping)
    {
        mapping.ForMember(
            d => d.Date,
            s => s.MapFrom(m => m.Date.ToShamsi()));
    }
}

public class ProjectCostDto
{
    public int ProjectId { get; set; }
    public string CostGroupTitle { get; set; }
    public string CreditorFullName { get; set; }
    public string ProjectTitle { get; set; }
    public string Date { get; set; }
    public string AmountNumeric { get; set; }
    public float Amount { get; set; }
    public string Number { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public string PayType { get; set; }
}