using AutoMapper;
using Common.Utilities;
using Data.Contracts;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;
using Presentation.Helpers;

namespace Presentation.Areas.Admin.Controllers;

[Area("Admin")]
public class ProjectCostController : Controller
{
    private readonly IRepository<Payoff> _payoffRepository;
    private readonly IRepository<Project> _projectRepository;
    private readonly IMapper _mapper;

    public ProjectCostController(IRepository<Payoff> payoffRepository, IRepository<Project> projectRepository,
        IMapper mapper)
    {
        _payoffRepository = payoffRepository;
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    [HttpGet("[area]/[controller]/[action]")]
    public async Task<IActionResult> CashCart([FromQuery] int? projectId, CancellationToken ct)
    {
        if (projectId is null)
        {
            var query = _projectRepository.TableNoTracking
                .Include(i => i.User)
                .AsQueryable();
            if (!CheckPermission.Check(User, "Project.All"))
            {
                var userId = User.Identity!.GetUserId<int>();
                query = query.Where(i => i.UserId == userId);
            }
            var projects = await query
                .ToListAsync(ct);
            ViewBag.Projects = projects;
            return View();
        }

        var project = await _projectRepository.TableNoTracking
            .FirstOrDefaultAsync(i => i.Id == projectId, ct);
        if (project is null)
            return NotFound();
        ViewBag.Project = project;
        var list = await _payoffRepository.TableNoTracking
            .Where(i => i.ProjectId == projectId)
            .Where(i => i.Type == PayoffType.Cash)
            .Where(i => i.Status == PayoffStatus.Approved)
            .Include(i => i.Invoice)
            .Include(i => i.Project)
            .ToListAsync(ct);
        return View(list);
    }

    [HttpPost("[area]/[controller]/[action]")]
    public async Task<IActionResult> CreateCashCart(PayoffDto dto, CancellationToken ct)
    {
        dto.DepositType = DepositType.Increalse;
        var model = dto.ToEntity(_mapper);
        model.Status = PayoffStatus.Approved;
        await _payoffRepository.AddAsync(model, ct);
        return RedirectToAction("CashCart", new { projectId = dto.ProjectId });
    }

    [HttpGet("[area]/[controller]/[action]/{id:int}")]
    public async Task<IActionResult> DeleteCashCart([FromRoute] int id, CancellationToken ct)
    {
        var model = await _payoffRepository.GetByIdAsync(ct, id);
        if (model is null)
        {
            TempData["ErrorMessage"] = "پیدا نشد";
            return RedirectToAction("CashCart");
        }

        if (model.DepositType != DepositType.Increalse)
        {
            TempData["ErrorMessage"] = "امکان حذف این پرداخت در این بخش نیست";
            return RedirectToAction("CashCart", new { projectId = model.ProjectId });
            
        }

        await _payoffRepository.DeleteAsync(model, ct);
        TempData["SuccessMessage"] = "پرداخت با موفقیت حذف شد";
        return RedirectToAction("CashCart", new { projectId = model.ProjectId });
    }

    [HttpGet("[area]/[controller]/[action]")]
    public async Task<IActionResult> Check([FromQuery] int? projectId, CancellationToken ct)
    {
        if (projectId is null)
        {
            var query = _projectRepository.TableNoTracking
                .Include(i => i.User)
                .AsQueryable();
            if (!CheckPermission.Check(User, "Project.All"))
            {
                var userId = User.Identity!.GetUserId<int>();
                query = query.Where(i => i.UserId == userId);
            }
            var projects = await query
                .ToListAsync(ct);
            ViewBag.Projects = projects;
            return View();
        }

        var list = await _payoffRepository.TableNoTracking
            .Where(i => i.ProjectId == projectId)
            .Where(i => i.Type == PayoffType.Check)
            .Where(i => i.Status == PayoffStatus.Approved)
            .Include(i => i.Invoice)
            .Include(i => i.Project)
            .ToListAsync(ct);
        return View(list);
    }

    [HttpGet("[area]/[controller]/[action]")]
    public async Task<IActionResult> DirectPayment([FromQuery] int? projectId, CancellationToken ct)
    {
        if (projectId is null)
        {
            var query = _projectRepository.TableNoTracking
                .Include(i => i.User)
                .AsQueryable();
            if (!CheckPermission.Check(User, "Project.All"))
            {
                var userId = User.Identity!.GetUserId<int>();
                query = query.Where(i => i.UserId == userId);
            }
            var projects = await query
                .ToListAsync(ct);
            ViewBag.Projects = projects;
            return View();
        }

        var list = await _payoffRepository.TableNoTracking
            .Where(i => i.ProjectId == projectId)
            .Where(i => i.Type == PayoffType.EmployerPayment)
            .Where(i => i.Status == PayoffStatus.Approved)
            .Include(i => i.Invoice)
            .Include(i => i.Project)
            .ToListAsync(ct);
        return View(list);
    }
}