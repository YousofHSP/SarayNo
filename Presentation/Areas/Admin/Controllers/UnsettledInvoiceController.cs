using AutoMapper;
using Common.Utilities;
using Data.Contracts;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;
using Presentation.Helpers;

namespace Presentation.Areas.Admin.Controllers;



[Area("Admin")]
public class UnsettledInvoiceController(
    IRepository<Project> projectRepository,
    IMapper mapper,
    IRepository<Payoff> payoffRepository,
    IRepository<Invoice> invoiceRepository,
    IRepository<InvoiceDetail> invoiceDetailRepository,
    IRepository<InvoiceLog> invoiceLogRepository)
    : Controller
{
    private readonly IMapper _mapper = mapper;

    public async Task<IActionResult> Index(int? projectId, CancellationToken ct)
    {
        if (projectId is null or 0)
        {
            var query = projectRepository.TableNoTracking
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
        var list = await invoiceRepository.TableNoTracking
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

    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await payoffRepository.TableNoTracking.Where(i => i.InvoiceId == id).ExecuteDeleteAsync(ct);
        await invoiceLogRepository.TableNoTracking.Where(i => i.InvoiceId == id).ExecuteDeleteAsync(ct);
        await invoiceDetailRepository.TableNoTracking.Where(i => i.InvoiceId == id).ExecuteDeleteAsync(ct);
        await invoiceRepository.TableNoTracking.Where(i => i.Id == id).ExecuteDeleteAsync(ct);

        TempData["SuccessMessage"] = " فاکتور با موفقیت حذف شد";
        return RedirectToAction("Index");
        
    }


}