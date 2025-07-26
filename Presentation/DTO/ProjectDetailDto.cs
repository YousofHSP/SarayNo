using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Common.Utilities;
using Domain;

namespace Presentation.DTO;

public class ProjectDetailDto : BaseDto<ProjectDetailDto, ProjectDetail>
{
    [Display(Name = "عنوان")]
    public string Title { get; set; }

    public string Date { get; set; }
    public int Percent { get; set; }
    public string? Description { get; set; }
    public int ProjectId { get; set; }

    protected override void CustomMappings(IMappingExpression<ProjectDetailDto, ProjectDetail> mapping)
    {
        mapping.ForMember(
            m => m.Date,
            s => s.MapFrom(d => d.Date.ToGregorian()));
    }
}

public class ProjectDetailItemDto : BaseDto<ProjectDetailItemDto, ProjectDetailItem>
{
    public int ProjectId { get; set; }
    public int ProjectDetailId { get; set; }
    public string Title { get; set; }
    public float? UnitPrice { get; set; }
    public float? Area { get; set; }
    public float TotalPrice{ get; set; }
    public string? Description{ get; set; }
    public ProjectDetailItemType Type { get; set; }
    
}