using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Utilities;
using Data.Contracts;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;
using Presentation.Models;
using Service.Model.Contracts;

namespace Presentation.Areas.Admin.Controllers;

[Area("Admin")]
public class ProjectController(
    IRepository<Project> repository,
    IRepository<User> userRepository,
    IRepository<ProjectDetail> projectDetailRepository,
    IRepository<ProjectDetailItem> projectDetailItemRepository,
    IUploadedFileService uploadedFileService,
    IMapper mapper)
    : BaseController<ProjectDto, ProjectResDto, Project>(repository, mapper)
{
    private readonly IRepository<Project> _repository = repository;
    private readonly IRepository<ProjectDetail> _projectDetailRepository = projectDetailRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task Configure(string method, CancellationToken ct)
    {
        await base.Configure(method, ct);
        var users = await userRepository.GetSelectListItems("LastName", ct:ct);
        SetIncludes(nameof(Project.User));
        AddOptions(nameof(ProjectDto.UserId), users);
    }

    public override async Task<ViewResult> Index(IndexDto model, CancellationToken ct)
    {
        var list = await _repository.TableNoTracking
            .ProjectTo<ProjectResDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> ProjectDetails([FromQuery] int? projectId, CancellationToken ct)
    {
        if (projectId is null)
        {
            var projects = await _repository.TableNoTracking.ToListAsync(ct);
            ViewBag.Projects = projects;
            return View(new Project());
        }

        var project = await _repository.TableNoTracking
            .Include(i => i.Details)
            .ThenInclude(i => i.ProjectDetailItems) 
            .FirstOrDefaultAsync(i => i.Id == projectId, ct);
        if (project is null)
            return NotFound();
        return View(project);
    }


    public async Task<IActionResult> CreateProjectDetail(ProjectDetailDto dto, CancellationToken ct)
    {
        var model = dto.ToEntity(_mapper);
        await _projectDetailRepository.AddAsync(model, ct);
        return RedirectToAction(nameof(ProjectDetails), new { projectId = model.ProjectId });
    }

    public async Task<IActionResult> CreateProjectDetailItem(ProjectDetailItemDto dto, CancellationToken ct)
    {
        var model = dto.ToEntity(_mapper);
        await projectDetailItemRepository.AddAsync(model, ct);
        return RedirectToAction("ProjectDetails", new { projectId = dto.ProjectId });
    }

    public async Task<IActionResult> Images([FromQuery] int? projectId, CancellationToken ct)
    {

        if (projectId is null)
        {
            var projects = await _repository.TableNoTracking.ToListAsync(ct);
            ViewBag.Projects = projects;
            ViewBag.ProjectId = 0;
            return View(new List<UploadedFile>());
        }

        var project = await _repository.TableNoTracking
            .Include(i => i.Activities)
            .Include(i => i.UnverifiedInvoices)
            .FirstOrDefaultAsync(i => i.Id == projectId, ct);
        if (project is null)
            return NotFound();
        ViewBag.ProjectId = projectId;
        var activityFiles = await uploadedFileService
            .GetFiles(nameof(Activity), project.Activities.Select(i => i.Id).ToList(), null, ct);
        var invoicesFiles = await uploadedFileService
            .GetFiles(nameof(UnverifiedInvoice), project.UnverifiedInvoices.Select(i => i.Id).ToList(), null, ct);
        var projectFiles = await uploadedFileService
            .GetFiles(nameof(Project), [project.Id], null, ct);

        var files = new List<UploadedFile>();
        files.AddRange(activityFiles);
        files.AddRange(invoicesFiles);
        files.AddRange(projectFiles);
        return View(files);
    }
    

    [HttpPost]
    public async Task<IActionResult> AddImage(AddImageDto dto, CancellationToken ct)
    {
        var userId = User.Identity!.GetUserId<int>();
        await uploadedFileService.UploadFileAsync(dto.File, dto.Type, nameof(Project), dto.ModelId, userId, ct, dto.Description);

        return RedirectToAction("Images", new { projectId = dto.ModelId});
    }
}