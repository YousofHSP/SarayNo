using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Exceptions;
using Common.Utilities;
using Data.Contracts;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;
using Presentation.Models;
using Service.Model.Contracts;

namespace Presentation.Areas.Admin.Controllers;

[Area("Admin")]
public class ProjectController(
    IRepository<Project> repository,
    IRepository<Album> albumRepository,
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
        var users = await userRepository.TableNoTracking.Select(i => new SelectListItem(i.FirstName + " " + i.LastName, i.Id.ToString())).ToListAsync(ct);
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
            var projects = await _repository.TableNoTracking
                .Include(i => i.User)
                .ToListAsync(ct);
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
    [HttpPost]
    public async Task<IActionResult> EditProjectDetail(ProjectDetailDto dto, CancellationToken ct)
    {
        var model = await _projectDetailRepository.GetByIdAsync(ct, dto.Id);
        if(model is null)
            return NotFound();
        model = dto.ToEntity(_mapper);
        await _projectDetailRepository.UpdateAsync(model, ct);
        return RedirectToAction(nameof(ProjectDetails), new { projectId = model.ProjectId });
    }

    public async Task<IActionResult> CreateProjectDetailItem(ProjectDetailItemDto dto, CancellationToken ct)
    {
        var model = dto.ToEntity(_mapper);
        await projectDetailItemRepository.AddAsync(model, ct);
        return RedirectToAction("ProjectDetails", new { projectId = dto.ProjectId });
    }

    public async Task<IActionResult> Images([FromQuery] int? albumId, CancellationToken ct)
    {

        if (albumId is null)
        {
            var albums = await albumRepository.TableNoTracking.ToListAsync(ct);
            ViewBag.Albums = albums;
            ViewBag.AlbumId = 0;
            var files2 = await uploadedFileService.GetFiles(null, ct);
            return View(files2);
        }

        var album = await albumRepository.TableNoTracking
            .FirstOrDefaultAsync(i => i.Id == albumId, ct);
        if (album is null)
            return NotFound();
        ViewBag.AlbumId = album.Id;
        var files = await uploadedFileService.GetFiles(album.Id, ct);
        return View(files);
    }
    

    [HttpPost]
    public async Task<IActionResult> AddImage(AddImageDto dto, CancellationToken ct)
    {
        var userId = User.Identity!.GetUserId<int>();
        await uploadedFileService.UploadFileAsync(dto.File, dto.AlbumId, nameof(Album), dto.ModelId, userId, ct, dto.Description);

        return RedirectToAction("Images", new { projectId = dto.ModelId});
    }

    [HttpPost]
    public async Task<IActionResult> AddAlbum(AlbumDto dto, CancellationToken ct)
    {
        var model = dto.ToEntity(_mapper);
        await albumRepository.AddAsync(model, ct);
        return RedirectToAction("Images", new { albumId = model.Id});
    }

    public override async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var model = await _repository.Table
            .Include(i => i.EmployerPayments)
            .Include(i => i.Invoices)
            .Include(i => i.Details)
            .FirstOrDefaultAsync(i => i.Id == id, ct);
        if (model is null)
        {
            TempData["ErrorMessage"] = "پروژه پیدا نشد";
            return RedirectToAction("Index");
        }
        var hasItems = model.EmployerPayments.Count != 0 || model.Details.Count != 0 || model.Invoices.Count != 0;
        if (hasItems)
        {
            TempData["ErrorMessage"] = "برای این پروژه داده ثبت شده";
            return RedirectToAction("Index");
        }

        await _repository.DeleteAsync(model, ct);
        TempData["SuccessMessage"] = "پروژه با موفقیت حذف شد";
        return RedirectToAction("Index");

    }
}