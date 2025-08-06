using Data.Contracts;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;

namespace Presentation.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private UserManager<User> _userManager { get; set; }
        private SignInManager<User> _signInManager { get; set; }
        private IUserRepository _userRepository { get; set; }

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
            IUserRepository userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
        }

        [HttpGet("[controller]/[action]")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterDto dto, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var checkPhoneNumber = await _userRepository.CheckUserName(dto.PhoneNumber, cancellationToken);
                if (checkPhoneNumber)
                {
                    ModelState.AddModelError("PhoneNumber", "شماره موبایل تکراری است");
                    return View(dto);
                }

                var checkNationalCode = await _userRepository.CheckNationalCode(dto.NationalCode, cancellationToken);
                if (checkNationalCode)
                {
                    ModelState.AddModelError(nameof(RegisterDto.NationalCode), "کدملی تکراری است");
                    return View(dto);
                }

                var user = new User
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    NationalCode = dto.NationalCode,
                    PhoneNumber = dto.PhoneNumber,
                    UserName = dto.PhoneNumber,
                    Status = UserStatus.Active
                };
                var result = await _userManager.CreateAsync(user, dto.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Guest");
                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new(ClaimTypes.GivenName, user.FirstName + " " + user.LastName)
                    };
                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };
                    await _signInManager.SignInWithClaimsAsync(user, properties, claims);
                    return RedirectToAction("Index", "Dashboard");
                }
            }

            ModelState.AddModelError("", "داده های وارد شده معتبر نیست");
            return View(dto);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            return View(new LoginDto
            {
                ReturnUrl = returnUrl,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginDto dto, CancellationToken ct)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.TableNoTracking.Where(i => i.PhoneNumber == dto.PhoneNumber)
                    .FirstOrDefaultAsync(ct);
                if (user is not null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignOutAsync();
                        var claims = new List<Claim>
                        {
                            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new(ClaimTypes.GivenName, user.FirstName + " " + user.LastName)
                        };
                        var properties = new AuthenticationProperties
                        {
                            IsPersistent = true
                        };
                        await _signInManager.SignInWithClaimsAsync(user, properties, claims);
                        var isGuest = await _userManager.IsInRoleAsync(user, "Guest");
                        if (isGuest)
                            return Redirect("/");
                        return Redirect("/Admin");
                    }
                }
            }

            ModelState.AddModelError(string.Empty, "نام کاربری یا رمز عبور اشتباه است");
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult AccessDenied(string returnUrl = "/")
        {
            return RedirectToAction("Index", "Home");
        }
    }
}