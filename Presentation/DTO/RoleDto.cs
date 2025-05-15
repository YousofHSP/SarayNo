using System.ComponentModel.DataAnnotations;
using Domain;

namespace Presentation.DTO
{
    public class RoleDto: BaseDto<RoleDto, Role>
    {
        [Required(ErrorMessage = "نام اجباری است")]
        [Display(Name = "نام")]
        public string Name { get; set; } = null!;
        [Display(Name = "عنوان")]
        public string? Title { get; set; }
        [Display(Name = "دسترسی ها")] public List<string> Permissions { get; set; }
    }

    public class RoleResDto : BaseDto<RoleResDto, Role>
    {
        [Display(Name = "نام")]
        public string Name { get; set; } = null!;
        [Display(Name = "عنوان")]
        public string? Title { get; set; }
    }
}
