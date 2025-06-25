using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Contracts;
using DocumentFormat.OpenXml.Bibliography;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;
using Presentation.Models;

namespace Presentation.Areas.Admin.Controllers;

[Area("Admin"), Authorize(Roles = "Admin")]
public class ActivityController(
    IRepository<Activity> repository,
    IRepository<Project> projectRepository,
    IRepository<ActivityDetail> activityDetailRepository,
    IRepository<CostGroup> costGroupRepository,
    IRepository<Creditor> creditorRepository,
    IMapper mapper) : BaseController<ActivityDto, ActivityResDto, Activity>(repository, mapper)
{
    public override async Task Configure(string method, CancellationToken ct)
    {
        await base.Configure(method, ct);
        SetIncludes(nameof(Activity.Creditor), nameof(Activity.CostGroup), nameof(Activity.Details), nameof(Activity.Project));
        var costGroups = await costGroupRepository.GetSelectListItems(ct: ct);
        var creditors = await creditorRepository.GetSelectListItems(nameof(Creditor.LastName), ct: ct);
        AddOptions(nameof(ActivityDto.CreditorId), creditors);
        AddOptions(nameof(ActivityDto.CostGroupId), costGroups);
        AddListAction("جزئیات", "fa fa-list", nameof(Details), "Activity");
        var projects = await projectRepository.GetSelectListItems(ct: ct, hasDefault: false);
        AddOptions(nameof(ActivityDto.ProjectId), projects);
        AddFilter("ProjectId", "پروژه", FieldType.Select, "", projects);
    }


    [HttpGet("[area]/[controller]/[action]/{id}")]
    public async Task<IActionResult> Details([FromRoute] int id, CancellationToken ct)
    {
        var model = await repository.TableNoTracking
            .Include(i => i.Details)
            .FirstOrDefaultAsync(i => i.Id == id, ct);
        if (model is null)
            return NotFound();

        return View(model);
    }

    [HttpGet("[area]/[controller]/[action]/{id}")]
    public IActionResult AddDetail([FromRoute] int id)
    {
        var dto = new ActivityDetailDto()
        {
            ActivityId = id
        };
        return View(dto);
    }

    [HttpPost("[area]/[controller]/[action]")]
    public async Task<IActionResult> CreateDetail(ActivityDetailDto dto, CancellationToken ct)
    {
        var model = dto.ToEntity(mapper);
        if (!ModelState.IsValid)
        {
            return View("AddDetail", dto);
        }
        await activityDetailRepository.AddAsync(model, ct);

        return RedirectToAction(nameof(Details), new { id = dto.ActivityId, ct = new CancellationToken() });

    }

    public async Task<IActionResult> ApprovedDetail([FromRoute] int id, CancellationToken ct)
    {
        var model = await activityDetailRepository.Table
            .FirstOrDefaultAsync(i => i.Id == id, ct);
        if (model is null)
            return NotFound();
        if (model.Status == ActivityDetailStatus.Pending)
        {
            model.Status = ActivityDetailStatus.Approved;
            await activityDetailRepository.UpdateAsync(model, ct);
        }
        return RedirectToAction(nameof(Details), "Activity", new {id = model.ActivityId});
    }
}