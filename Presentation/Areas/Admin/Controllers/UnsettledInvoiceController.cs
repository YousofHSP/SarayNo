using AutoMapper;
using Data.Contracts;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;

namespace Presentation.Areas.Admin.Controllers;



[Authorize(Roles = "Admin")]
[Area("Admin")]
public class UnsettledInvoiceController : Controller
{
    private readonly IRepository<UnsettledInvoice> _repository;
    private readonly IRepository<Project> _projectRepository;
    private readonly IMapper _mapper;
    private readonly IRepository<Payoff> _payoffRepository; 

    public UnsettledInvoiceController(IRepository<UnsettledInvoice> repository, IRepository<Project> projectRepository, IMapper mapper, IRepository<Payoff> payoffRepository)
    {
        _repository = repository;
        _projectRepository = projectRepository;
        _mapper = mapper;
        _payoffRepository = payoffRepository;
    }

    public async Task<IActionResult> Index(int? projectId, CancellationToken ct)
    {
        if (projectId is null or 0)
        {
            var projects = await _projectRepository.TableNoTracking.ToListAsync(ct);
            ViewBag.Projects = projects;
            return View();
        }
        ViewBag.ProjectId = projectId;
        var list = await _repository.TableNoTracking
            .Where(i => i.Activity.ProjectId == projectId || i.UnverifiedInvoice.ProjectId == projectId)
            .Include(i => i.CostGroup)
            .Include(i => i.Creditor)
            .Include(i => i.Activity)
            .ThenInclude(i => i.Project)
            .Include(i => i.UnverifiedInvoice)
            .ThenInclude(i => i.Project)
            .ToListAsync(ct);

        return View(list);
    }


    [HttpPost]
    public async Task<IActionResult> AddPayoff(PayoffDto dto, CancellationToken ct)
    {
        var model = dto.ToEntity(_mapper);
        
        await _payoffRepository.AddAsync(model, ct);
        return RedirectToAction("Index", new {projectId = dto.ProjectId});
    }

    [HttpPost]
    public async Task<IActionResult> Update(UnsettledInvoiceDto dto, CancellationToken ct)
    {
        var model = await _repository
            .Table
            .Include(i => i.Activity)
            .Include(i => i.UnverifiedInvoice)
            .FirstOrDefaultAsync(i => i.Id == dto.Id, ct);
        if (model is null)
            return NotFound();
        if (dto.IsDone)
            model.Status = UnsettledInvoiceStatus.Done;
        model.Discount = dto.Discount;
        await _repository.UpdateAsync(model, ct);
        var projectId = 0;
        if (model.Activity is not null)
            projectId = model.Activity.ProjectId;
        else if (model.UnverifiedInvoice is not null)
            projectId = model.UnverifiedInvoice.ProjectId;
        return RedirectToAction("Index", new {projectId});

    }
}