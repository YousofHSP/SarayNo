using AutoMapper;
using Data.Contracts;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;

namespace Presentation.Areas.Admin.Controllers;



[Area("Admin")]
public class UnsettledInvoiceController : Controller
{
    private readonly IRepository<Invoice> _invoiceRepository;
    private readonly IRepository<Project> _projectRepository;
    private readonly IMapper _mapper;
    private readonly IRepository<Payoff> _payoffRepository; 

    public UnsettledInvoiceController(IRepository<Project> projectRepository, IMapper mapper, IRepository<Payoff> payoffRepository, IRepository<Invoice> invoiceRepository)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
        _payoffRepository = payoffRepository;
        _invoiceRepository = invoiceRepository;
    }

    public async Task<IActionResult> Index(int? projectId, CancellationToken ct)
    {
        if (projectId is null or 0)
        {
            var projects = await _projectRepository.TableNoTracking
                .Include(i => i.User)
                .ToListAsync(ct);
            ViewBag.Projects = projects;
            return View();
        }
        ViewBag.ProjectId = projectId;
        var list = await _invoiceRepository.TableNoTracking
            .Where(i => i.ProjectId == projectId)
            .Where(i => i.Type == InvoiceType.Unsettled)
            .Include(i => i.Payoffs)
            .Include(i => i.CostGroup)
            .Include(i => i.Creditor)
            .Include(i => i.Project)
            .Include(i => i.InvoiceDetails)
            .ToListAsync(ct);

        return View(list);
    }


}