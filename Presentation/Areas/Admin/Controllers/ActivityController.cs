using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Utilities;
using Data.Contracts;
using DocumentFormat.OpenXml.Bibliography;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;
using Presentation.Models;
using Service.Model.Contracts;

namespace Presentation.Areas.Admin.Controllers;

[Area("Admin")]
public class ActivityController(
    IRepository<Activity> repository,
    IRepository<Project> projectRepository,
    IRepository<ActivityDetail> activityDetailRepository,
    IRepository<CostGroup> costGroupRepository,
    IRepository<Creditor> creditorRepository,
    IRepository<UnsettledInvoice> unsettledInvoiceRepository,
    IUploadedFileService uploadedFileService,
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


    [HttpGet("[area]/[controller]")]
    public async Task<IActionResult> Index([FromQuery] int? projectId, CancellationToken ct)
    {
        if (projectId is null or 0)
        {
            var projects = await projectRepository.TableNoTracking.ToListAsync(ct);
            ViewBag.Projects = projects;
            return View();
        }

        var project = await projectRepository.TableNoTracking.FirstOrDefaultAsync(i => i.Id == projectId, ct);
        ViewBag.Project = project;
        var model = await repository.TableNoTracking
            .Include(i => i.CostGroup)
            .Include(i => i.Creditor)
            .Include(i => i.Project)
            .Where(i => i.ProjectId == projectId)
            .ProjectTo<ActivityResDto>(mapper.ConfigurationProvider)
            .ToListAsync(ct);
        return View(model);
    }

    public override async Task<IActionResult> Create(ActivityDto dto, CancellationToken ct)
    {
        var model = dto.ToEntity(mapper);
        await repository.AddAsync(model, ct);
        return RedirectToAction("Index","Activity", new { projectId = model.ProjectId });
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

    [HttpPost]
    public async Task<IActionResult> AddImage(AddImageDto dto, CancellationToken ct)
    {
        var userId = User.Identity!.GetUserId<int>();
        await uploadedFileService.UploadFileAsync(dto.File, dto.Type, nameof(Activity), dto.ModelId, userId, ct, dto.Description);

        return RedirectToAction("Details", new { id = dto.ModelId, ct });
    }

    [HttpGet]
    public async Task<IActionResult> RemoveImage(int id, CancellationToken ct)
    {

        var model = await uploadedFileService.GetFile(id, ct);
        await uploadedFileService.RemoveFile(model, ct);
        return RedirectToAction("Details", new { id = model.ModelId, ct });
    }

    [HttpPost("[area]/[controller]/[action]/{id:int}")]
    public async Task<IActionResult> SetTotalAmount([FromRoute] int id, float totalAmount, CancellationToken ct)
    {
        var model = await repository.GetByIdAsync(ct, id);
        if (model is null)
            return NotFound();
        model.TotalAmount = totalAmount;
        await repository.UpdateAsync(model, ct);
        return RedirectToAction("Details", new { id, ct });
    }

    [HttpGet]
    public async Task<IActionResult> Done(int id, CancellationToken ct)
    {
        var model = await repository.Table
            .Include(i => i.Details)
            .FirstOrDefaultAsync(i => i.Id == id, ct);
        
        if (model is null)
            return NotFound();
        if (model.IsDone)
            return RedirectToAction("Details", new { id, ct });
        model.IsDone = true;
        var invoice = new UnsettledInvoice()
        {
            ActivityId = model.Id,
            Description = model.Description,
            CreditorId = model.CreditorId,
            Date = DateTimeOffset.Now,
            RemainAmount = model.TotalAmount - model.Details.Sum(i => i.Price),
            CostGroupId = model.CostGroupId,
            CreatedAt = DateTimeOffset.Now
        };
        await unsettledInvoiceRepository.AddAsync(invoice, ct);
        await repository.UpdateAsync(model, ct);
        return RedirectToAction("Details", new { id, ct });
    }
}