using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain;
using Presentation.Attributes;
using Presentation.Models;

namespace Presentation.DTO;

public class UserDto : BaseDto<UserDto, User>
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

    [Display(Name = "آدرس")]
    [Field(FieldType.Text )]
    public string? Address{ get; set; }

    [Display(Name = "کدملی")]
    [Field(FieldType.Text)]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [StringLength(10, ErrorMessage = "{0} معتبر نیست")]
    public string NationalCode { get; set; }
    
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [StringLength(11, ErrorMessage = "شماره موبایل معتبر نیست")]
    [Display(Name = "شماره موبایل")]
    [Field(FieldType.Text)]
    public string PhoneNumber { get; set; }

    [Display(Name = "وضعیت")]
    public UserStatus Status { get; set; }
    
    [DataType(DataType.Password)]
    [Display(Name = "رمز عبور")]
    [Field(FieldType.Text)]
    public string? Password { get; set; } = string.Empty;

    [Display(Name = "نقش")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [Field(FieldType.Select)]
    public string RoleName { get; set; } = string.Empty;
}

public class UserResDto : BaseDto<UserResDto, User>
{
    [Display(Name = "نام")] 
    public string FirstName { get; set; } = null!;
    
    [Display(Name = "نام خانوادگی")] 
    public string LastName { get; set; } = null!;

    [Display(Name = "شماره موبایل")]
    public string PhoneNumber { get; set; } = null!;
    
    [Display(Name = "کدملی")]
    public string NationalCode { get; set; } = null!;
    
    [Display(Name = "نقش")]
    public string RoleName { get; set; } = null!;

    protected override void CustomMappings(IMappingExpression<User, UserResDto> mapping)
    {
        mapping.ForMember(
            d => d.RoleName,
            s => s.MapFrom(m => m.Roles.FirstOrDefault()!.Name ?? "")
            );
    }
}