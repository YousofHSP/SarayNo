using System.ComponentModel.DataAnnotations;

namespace Presentation.DTO;

public class RegisterDto
{
    [Display(Name = "نام")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(100)]
    public string FirstName { get; set; }
    
    [Display(Name = "نام خانوادگی")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(100)]
    public string LastName { get; set; }
    
    [Display(Name = "رمز")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MinLength(4, ErrorMessage = "رمز عبور نمیتواند کم تر است ۴ حرف باشد")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Display(Name = "شماره موبایل")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [StringLength(11,ErrorMessage = "شماره موبایل معتبر نیست")]
    public string PhoneNumber { get; set; }
    
    [Display(Name = "کدملی")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [StringLength(10,ErrorMessage = "{0} معتبر نیست")]
    public string NationalCode{ get; set; }
}

public class LoginDto
{
    [Display(Name = "شماره موبایل")]
    [Required(ErrorMessage = "شماره موبایل اجباری است")]
    public string PhoneNumber { get; set; }
    [Required(ErrorMessage = "رمز عبور اجباری است")]
    [Display(Name = "رمز")]
    public string Password { get; set; }
    public string ReturnUrl { get; set; }
}

