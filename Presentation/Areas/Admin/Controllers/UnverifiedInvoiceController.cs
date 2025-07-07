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

[Authorize(Roles = "Admin")]
[Area("Admin")]
public class UnverifiedInvoiceController : Controller
{
    private readonly IRepository<UnverifiedInvoice> _repository;
    private readonly IUploadedFileService _uploadedFileService;
    private readonly IMapper _mapper;

    public UnverifiedInvoiceController(IRepository<UnverifiedInvoice> repository, IMapper mapper,
        IUploadedFileService uploadedFileService)
    {
        _repository = repository;
        _mapper = mapper;
        _uploadedFileService = uploadedFileService;
    }

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var query = _repository.TableNoTracking.AsQueryable();
        var projectId = (int?)ViewData["ProjectId"];
        if (projectId is not null && projectId != 0)
            query = query.Where(i => i.ProjectId == projectId);
        var list = await query
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
    public async Task<IActionResult> AddImage(AddImageDto dto, CancellationToken ct)
    {
        var userId = User.Identity!.GetUserId<int>();
        await _uploadedFileService.UploadFileAsync(dto.File, dto.Type, nameof(UnverifiedInvoice),
            dto.ModelId, userId, ct, dto.Description);
        return RedirectToAction("Index");
    }
}