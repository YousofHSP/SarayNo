using AutoMapper;
using Common.Utilities;
using Data.Contracts;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;
using Presentation.Helpers;
using Service.Model.Contracts;

namespace Presentation.Areas.Admin.Controllers;

[Area("Admin")]
public class UnverifiedInvoiceController : Controller
{
    private readonly IRepository<Invoice> _invoiceRepository;
    private readonly IRepository<UnsettledInvoice> _unsettledInvoiceRepository;
    private readonly IRepository<Project> _projectRepository;
    private readonly IUploadedFileService _uploadedFileService;
    private readonly IMapper _mapper;

    public UnverifiedInvoiceController(IMapper mapper,
        IUploadedFileService uploadedFileService, IRepository<UnsettledInvoice> unsettledInvoiceRepository,
        IRepository<Project> projectRepository, IRepository<Invoice> invoiceRepository)
    {
        _mapper = mapper;
        _uploadedFileService = uploadedFileService;
        _unsettledInvoiceRepository = unsettledInvoiceRepository;
        _projectRepository = projectRepository;
        _invoiceRepository = invoiceRepository;
    }

    public async Task<IActionResult> Index([FromQuery] int? projectId, CancellationToken ct)
    {
        if (projectId is null or 0)
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
            return View(new List<Invoice>());
        }

        ViewBag.ProjectId = projectId;
        var list = await _invoiceRepository.TableNoTracking
            .Where(i => i.ProjectId == projectId)
            .Where(i => i.Type == InvoiceType.Unverified)
            .Include(i => i.Project)
            .Include(i => i.CostGroup)
            .Include(i => i.Creditor)
            .Include(i => i.InvoiceDetails)
            .ToListAsync(ct);
        return View(list);
    }
}