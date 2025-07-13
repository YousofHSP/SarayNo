using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Contracts;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;
using Presentation.Models;

namespace Presentation.Areas.Admin.Controllers;

[Area("Admin")]
public class ProjectController(
    IRepository<Project> repository,
    IRepository<ProjectDetail> projectDetailRepository,
    IMapper mapper)
    : BaseController<ProjectDto, ProjectResDto, Project>(repository, mapper)
{
    private readonly IRepository<Project> _repository = repository;
    private readonly IRepository<ProjectDetail> _projectDetailRepository = projectDetailRepository;
    private readonly IMapper _mapper = mapper;

    public override async Task<ViewResult> Index(IndexDto model, CancellationToken ct)
    {
        var list = await _repository.TableNoTracking
            .ProjectTo<ProjectResDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);
        return View(list);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> EstimateDetails([FromQuery] int? projectId, CancellationToken ct)
    {
        if (projectId is null)
        {
            var projects = await _repository.TableNoTracking.ToListAsync(ct);

            ViewBag.Projects = projects;
            return View();
        }

        var model = await _repository.TableNoTracking
            .Include(i => i.Details)
            .FirstOrDefaultAsync(i => i.Id == projectId, ct);
        if (model is null)
            return NotFound();

        return View(model);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> FinalDetails([FromQuery] int? projectId, CancellationToken ct)
    {
        if (projectId is null)
        {
            var projects = await _repository.TableNoTracking.ToListAsync(ct);

            ViewBag.Projects = projects;
            return View();
        }

        var model = await _repository.TableNoTracking
            .Include(i => i.Details)
            .FirstOrDefaultAsync(i => i.Id == projectId, ct);
        if (model is null)
            return NotFound();

        return View(model);
    }


    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> AddEstimateDetail(int id, CancellationToken ct)
    {
        var dto = new ProjectDetailDto()
        {
            ProjectId = id,
            Type = ProjectDetailType.Estimate
        };

        return View("AddProjectDetail", dto);
    }

    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> AddFinalDetail(int id, CancellationToken ct)
    {
        var dto = new ProjectDetailDto()
        {
            ProjectId = id,
            Type = ProjectDetailType.Final
        };

        return View("AddProjectDetail", dto);
    }

    public async Task<IActionResult> CreateProjectDetail(ProjectDetailDto dto, CancellationToken ct)
    {
        var model = dto.ToEntity(_mapper);
        await _projectDetailRepository.AddAsync(model, ct);
        if (model.Type == ProjectDetailType.Estimate)
            return RedirectToAction(nameof(EstimateDetails),
                new { projectId = model.ProjectId });
        return RedirectToAction(nameof(FinalDetails), new { projectId = model.ProjectId });
    }
}