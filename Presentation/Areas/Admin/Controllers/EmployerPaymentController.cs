using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Utilities;
using Data.Contracts;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;
using Service.Model.Contracts;

namespace Presentation.Areas.Admin.Controllers;

[Authorize(Roles = "Admin")]
[Area("Admin")]
public class EmployerPaymentController : Controller
{
    private readonly IRepository<EmployerPayment> _repository;
    private readonly IRepository<Project> _projectRepository;
    private readonly IRepository<Activity> _activityRepository;
    private readonly IRepository<UnverifiedInvoice> _unverifiedInvoiceRepository;
    private readonly IUploadedFileService _uploadedFileService;
    private readonly IMapper _mapper;

    public EmployerPaymentController(IRepository<EmployerPayment> repository, IRepository<Project> projectRepository, IMapper mapper, IUploadedFileService uploadedFileService, IRepository<Activity> activityRepository, IRepository<UnverifiedInvoice> unverifiedInvoiceRepository)
    {
        _repository = repository;
        _projectRepository = projectRepository;
        _mapper = mapper;
        _uploadedFileService = uploadedFileService;
        _activityRepository = activityRepository;
        _unverifiedInvoiceRepository = unverifiedInvoiceRepository;
    }

    public async Task<IActionResult> Index([FromQuery] int? projectId, CancellationToken ct)
    {
        if (projectId is null or 0)
        {
            var projects = await _projectRepository.TableNoTracking.ToListAsync(ct);
            ViewBag.Projects = projects;
            return View();
        }

        var project = await _projectRepository.TableNoTracking
            .Include(i => i.Details)
            .Include(i => i.Activities)
            .ThenInclude(i => i.Details)
            .Include(i => i.UnverifiedInvoices)
            .FirstOrDefaultAsync(i => i.Id == projectId, ct);
        if (project is null)
            return NotFound();
        ViewBag.Project = project;
        
        var list = await _repository.TableNoTracking
            .Where(i => i.ProjectId == projectId)
            .Include(i => i.Project)
            .ProjectTo<EmployerPaymentResDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return View(list);

    }

    public async Task<IActionResult> ProjectCosts([FromQuery] int? projectId, CancellationToken ct)
    {
        if (projectId is null or 0)
        {
            var projects = await _projectRepository.TableNoTracking.ToListAsync(ct);
            ViewBag.Projects = projects;
            return View();
        }

        var project = await _projectRepository.TableNoTracking
            .Include(i => i.Details)
            .FirstOrDefaultAsync(i => i.Id == projectId, ct);
        if (project is null)
            return NotFound();
        ViewBag.Project = project;
        var activityList = await _activityRepository.TableNoTracking
            .Where(i => i.ProjectId == projectId)
            .Include(i => i.Project)
            .Include(i => i.Creditor)
            .Include(i => i.CostGroup)
            .Include(i => i.Details)
            .ToListAsync(ct);
        var unverifiedInvoices = await _unverifiedInvoiceRepository.TableNoTracking
            .Where(i => i.ProjectId == projectId)
            .Include(i => i.Project)
            .Include(i => i.CostGroup)
            .Include(i => i.Creditor)
            .ToListAsync(ct);
        var list = new List<ProjectCostDto>();
        foreach (var item in unverifiedInvoices)
        {
            list.Add(new ()
            {
                ProjectId = item.ProjectId,
                Description = item.Description ?? "",
                Number = "",
                Date = item.Date.ToShamsi(),
                ProjectTitle = item.Project.Title,
                AmountNumeric = item.Amount.ToNumeric(),
                CostGroupTitle = item.CostGroup.Title,
                CreditorFullName = item.Creditor.FirstName + " " + item.Creditor.LastName
            });
        }

        foreach (var activity in activityList)
        {
            foreach (var item in activity.Details)
            {
                list.Add(new()
                {
                    CostGroupTitle = activity.CostGroup.Title,
                    CreditorFullName = activity.Creditor.FirstName + " " + activity.Creditor.LastName,
                    Description = activity.Description ?? "",
                    Number = item.Number,
                    ProjectId = activity.ProjectId,
                    ProjectTitle = activity.Project.Title,
                    AmountNumeric = item.Price.ToNumeric(),
                    Date = item.Date.ToShamsi()
                });
            }
        }
        return View(list);

    }

    [HttpPost]
    public async Task<IActionResult> Create(EmployerPaymentDto dto, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View("Index");
        var model = dto.ToEntity(_mapper);
        await _repository.AddAsync(model, ct);
        return RedirectToAction("Index", new { projectId = model.ProjectId });
    }

    [HttpPost]
    public async Task<IActionResult> AddImage(AddImageDto dto, CancellationToken ct)
    {
        var userId = User.Identity!.GetUserId<int>();
        await _uploadedFileService.UploadFileAsync(dto.File, dto.Type, nameof(EmployerPayment),
            dto.ModelId, userId, ct, dto.Description);
        return RedirectToAction("Index");
        
    }
}