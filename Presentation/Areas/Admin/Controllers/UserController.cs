using AutoMapper;
using Common.Exceptions;
using Data.Contracts;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers;
using Presentation.DTO;

namespace Presentation.Areas.Admin.Controllers;

[Area("Admin")]
public class UserController(
    IMapper mapper,
    IUserRepository repository,
    IRepository<Role> roleRepository,
    UserManager<User> userManager
    )
    : BaseController<UserDto, UserResDto, User>(repository, mapper)
{
    private readonly IMapper _mapper = mapper;

    public override async Task Configure(string method, CancellationToken ct)
    {
        await base.Configure(method, ct);
        SetIncludes(nameof(Domain.User.Roles));

        var selectedRole = "";
        if (method is "edit" or "update" && Model is not null)
            selectedRole = (await userManager.GetRolesAsync(Model)).FirstOrDefault() ?? "";
        
        var roles = await roleRepository.GetSelectListItems(
            nameof(Role.Name),
            nameof(Role.Title),
            null,
            [selectedRole],
            hasDefault:false,
            ct:ct
        );
        AddOptions(nameof(UserDto.RoleName), roles);
        AddCondition(m => !m.Id.Equals(1));
    }

    public override async Task<IActionResult> Create(UserDto dto, CancellationToken ct)
    {
        await Configure("create", ct);
        CreateViewModel.Properties = typeof(UserDto).GetProperties();
        CreateViewModel.Options = Options;
        if (string.IsNullOrEmpty(dto.Password))
        {
            CreateViewModel.Error = true;
            ModelState.AddModelError(nameof(UserDto.Password), "رمز کاربر را وارد کنید");
            return View("~/Areas/Admin/Views/Base/Create.cshtml", CreateViewModel);
        }

        if (!ModelState.IsValid)
        {
            CreateViewModel.Error = true;
            return View("~/Areas/Admin/Views/Base/Create.cshtml", CreateViewModel);
        }

        var isExists = await repository.CheckUserName(dto.PhoneNumber, ct);
        if (isExists)
        {
            ModelState.AddModelError(nameof(UserDto.PhoneNumber), "این موبایل قبلا انتخاب شده");
            CreateViewModel.Error = true;
            return View("~/Areas/Admin/Views/Base/Create.cshtml", CreateViewModel);
        }
        isExists = await repository.CheckNationalCode(dto.NationalCode, ct);
        if (isExists)
        {
            ModelState.AddModelError(nameof(UserDto.NationalCode), "این کدملی قبلا انتخاب شده");
            CreateViewModel.Error = true;
            return View("~/Areas/Admin/Views/Base/Create.cshtml", CreateViewModel);
        }

        var model = dto.ToEntity(mapper);
        model.UserName = model.PhoneNumber;
        var result = await userManager.CreateAsync(model);
        if (!result.Succeeded)
        {
            CreateViewModel.Error = true;
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
            return View("~/Areas/Admin/Views/Base/Create.cshtml", CreateViewModel);
            
        }
        await userManager.AddToRoleAsync(model, dto.RoleName);
        await userManager.AddPasswordAsync(model, dto.Password);
        return RedirectToAction(nameof(Index));
    }

    public override async Task<IActionResult> Edit(UserDto dto, CancellationToken ct)
    {
        await Configure("update", ct);
        var model = await repository.GetByIdAsync(ct, dto.Id);
        if (model is null)
            return NotFound();
        model = dto.ToEntity(model, mapper);
        EditViewModel.Fields = CreateViewModel.Fields;
        EditViewModel.Options = Options;
        EditViewModel.Error = false;
        EditViewModel.Properties = typeof(UserDto).GetProperties();
        ViewBag.Model = EditViewModel;
        if (!ModelState.IsValid)
        {
            EditViewModel.Error = true;
            return View("~/Areas/Admin/Views/Base/Edit.cshtml", dto);
        }
        var isExists = await repository.CheckUserName(dto.PhoneNumber, ct, model.Id);
        if (isExists)
        {
            ModelState.AddModelError(nameof(UserDto.PhoneNumber), "این موبایل قبلا انتخاب شده");
            EditViewModel.Error = true;
            return View("~/Areas/Admin/Views/Base/Edit.cshtml", dto);
        }
            
        isExists = await repository.CheckNationalCode(dto.NationalCode, ct, model.Id);
        if (isExists)
        {
            ModelState.AddModelError(nameof(UserDto.NationalCode), "این کدملی قبلا انتخاب شده");
            EditViewModel.Error = true;
            return View("~/Areas/Admin/Views/Base/Edit.cshtml", dto);
        }

        await userManager.UpdateAsync(model);
        var roles = await userManager.GetRolesAsync(model);
        await userManager.RemoveFromRolesAsync(model, roles);
        if (!string.IsNullOrEmpty(dto.RoleName))
            await userManager.AddToRoleAsync(model, dto.RoleName);
        if (!string.IsNullOrEmpty(dto.Password))
        {
            await userManager.RemovePasswordAsync(model);
            await userManager.AddPasswordAsync(model, dto.Password);
        }

        return RedirectToAction(nameof(Index));
    }

    public override async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var model = await userManager.FindByIdAsync(id.ToString());
        if (model is null)
            throw new NotFoundException();
        var roles = await userManager.GetRolesAsync(model);
        await userManager.RemoveFromRolesAsync(model, roles);

        await userManager.DeleteAsync(model);
        return RedirectToAction(nameof(Index));
    }
}