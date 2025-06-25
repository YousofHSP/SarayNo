using System.ComponentModel.DataAnnotations;
using Domain;

namespace Presentation.DTO;

public class ProjectDetailDto : BaseDto<ProjectDetailDto, ProjectDetail>
{
    [Display(Name = "عنوان")]
    public string Title { get; set; }
    
    public ProjectDetailType Type { get; set; }
    
    [Display(Name = "فی")]
    public float UnitPrice { get; set; }
    
    [Display(Name = "مقدار")]
    public float Area { get; set; }
    
    [Display(Name = "قیمت کل")]
    public float TotalPrice { get; set; }
    
    public int ProjectId { get; set; }
    
}