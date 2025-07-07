using Data.Contracts;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Areas.Admin.Controllers;



[Authorize(Roles = "Admin")]
[Area("Admin")]
public class UnsettledInvoiceController : Controller
{
    private readonly IRepository<UnsettledInvoice> _repository;

    public UnsettledInvoiceController(IRepository<UnsettledInvoice> repository)
    {
        _repository = repository;
    }

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var list = await _repository.TableNoTracking
            .Include(i => i.CostGroup)
            .Include(i => i.Creditor)
            .Include(i => i.Activity)
            .ThenInclude(i => i.Project)
            .Include(i => i.UnverifiedInvoice)
            .ThenInclude(i => i.Project)
            .ToListAsync(ct);

        return View(list);
    }
    
}