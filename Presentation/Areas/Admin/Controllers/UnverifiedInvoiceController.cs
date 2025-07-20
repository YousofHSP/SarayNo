using AutoMapper;
using Common.Utilities;
using Data.Contracts;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;
using Service.Model.Contracts;

namespace Presentation.Areas.Admin.Controllers;

[Area("Admin")]
public class UnverifiedInvoiceController : Controller
{
    private readonly IRepository<UnverifiedInvoice> _repository;
    private readonly IRepository<UnsettledInvoice> _unsettledInvoiceRepository;
    private readonly IRepository<Project> _projectRepository;
    private readonly IUploadedFileService _uploadedFileService;
    private readonly IMapper _mapper;

    public UnverifiedInvoiceController(IRepository<UnverifiedInvoice> repository, IMapper mapper,
        IUploadedFileService uploadedFileService, IRepository<UnsettledInvoice> unsettledInvoiceRepository,
        IRepository<Project> projectRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _uploadedFileService = uploadedFileService;
        _unsettledInvoiceRepository = unsettledInvoiceRepository;
        _projectRepository = projectRepository;
    }

    public async Task<IActionResult> Index([FromQuery] int? projectId, CancellationToken ct)
    {
        if (projectId is null or 0)
        {
            var projects = await _projectRepository.TableNoTracking.ToListAsync(ct);
            ViewBag.Projects = projects;
            return View();
        }

        var query = _repository.TableNoTracking
            .Where(i => i.ProjectId == projectId)
            .AsQueryable();
        var list = await query
            .Include(i => i.UnsettledInvoices)
            .Include(i => i.Project)
            .Include(i => i.CostGroup)
            .Include(i => i.Creditor)
            .ToListAsync(ct);
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(UnverifiedInvoiceDto dto, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Index", "UnverifiedInvoice", new { ct });
        var model = dto.ToEntity(_mapper);
        await _repository.AddAsync(model, ct);
        return RedirectToAction("Index", "UnverifiedInvoice", new { ct });
    }

    [HttpPost]
    public async Task<IActionResult> Update(UnverifiedInvoiceDto dto, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Index", "UnverifiedInvoice", new { ct });
        var model = await _repository.GetByIdAsync(ct, dto.Id);
        if (model is null)
            return NotFound();
        model = dto.ToEntity(model, _mapper);
        await _repository.UpdateAsync(model, ct);
        return RedirectToAction("Index", "UnverifiedInvoice", new { projectId = model.ProjectId });
    }

    [HttpPost]
    public async Task<IActionResult> AddImage(AddImageDto dto, CancellationToken ct)
    {
        var userId = User.Identity!.GetUserId<int>();
        await _uploadedFileService.UploadFileAsync(dto.File, dto.Type, nameof(UnverifiedInvoice),
            dto.ModelId, userId, ct, dto.Description);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> ChangeStatus(int id, UnverifiedInvoiceStatus status, CancellationToken ct)
    {
        var model = await _repository.GetByIdAsync(ct, id);
        if (model is null)
            return NotFound();
        model.Status = status;
        await _repository.UpdateAsync(model, ct);
        return RedirectToAction("Index", new {projectId = model.ProjectId});
    }

    [HttpGet]
    public async Task<IActionResult> AddToUnsettledInvoice(int id, CancellationToken ct)
    {
        var model = await _repository.TableNoTracking.FirstOrDefaultAsync(i => i.Id == id, ct);
        if (model is null)
            return NotFound();
        if (model.Status == UnverifiedInvoiceStatus.Approved)
        {
            var files = await _uploadedFileService.GetFiles(nameof(UnverifiedInvoice), [model.Id], null, ct);
            var unsettledInvoice = new UnsettledInvoice
            {
                CostGroupId = model.CostGroupId,
                CreditorId = model.CreditorId,
                Date = model.Date,
                Description = model.Description,
                Status = UnsettledInvoiceStatus.Pending,
                UnverifiedInvoiceId = model.Id,
                Discount = 0,
                RemainAmount = model.Amount
            };
            var newFiles = files.Select(i => new UploadedFile()
            {
                ModelId = model.Id,
                ModelType = nameof(unsettledInvoice),
                Description = i.Description,
                Status = i.Status,
                CreatedAt = DateTimeOffset.Now,
                Type = i.Type,
                MimeType = i.MimeType,
                OriginalName = i.OriginalName,
                SavedName = i.SavedName,
                UserId = i.UserId
            }).ToList();

            await _uploadedFileService.AddRangeAsync(newFiles, ct);
            await _unsettledInvoiceRepository.AddAsync(unsettledInvoice, ct);
        }

        return RedirectToAction("Index", new {projectId = model.ProjectId});
    }
}