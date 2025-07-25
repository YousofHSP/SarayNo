using AutoMapper;
using Common.Utilities;
using Data.Contracts;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;

namespace Presentation.Areas.Admin.Controllers;

[Area("Admin")]
public class CreditorController(
    IRepository<Creditor> repository,
    IRepository<Invoice> invoiceRepository,
    IRepository<Project> projectRepository,
    IMapper mapper
) : BaseController<CreditorDto, CreditorResDto, Creditor>(repository, mapper)
{
    public override async Task Configure(string method, CancellationToken ct)
    {
        await base.Configure(method, ct);
        AddListAction("بدهی ها", "fa fa-dollar", "ProjectDebts", "Creditor");
    }

    [HttpGet("[action]/{id:int}")]
    public async Task<IActionResult> ProjectDebts([FromQuery] int? projectId, int id,
        CancellationToken ct)
    {
        ViewBag.CreditorId = id;
        if (projectId is null)
        {
            var projects = await projectRepository.TableNoTracking
                .Include(i => i.User)
                .ToListAsync(ct);
            ViewBag.Projects = projects;
            return View();
        }

        var project = await projectRepository.TableNoTracking.FirstOrDefaultAsync(i => i.Id == projectId, ct);
        if (project is null)
            return NotFound();
        ViewBag.Project = project;

        var creditor = await repository.TableNoTracking
            .FirstOrDefaultAsync(i => i.Id == id, ct);
        if (creditor is null)
            return NotFound();
        var invoices = await invoiceRepository.TableNoTracking
            .Where(i => i.CreditorId == creditor.Id)
            .Include(i => i.CostGroup)
            .Include(i => i.Creditor)
            .Include(i => i.Project)
            .Include(i => i.InvoiceDetails)
            .ToListAsync(ct);
        
        var list = new List<CreditorDebtDto>();
        foreach (var item in invoices)
        {
            var amount = item.InvoiceDetails.Sum(i => i.Price);
            list.Add(new()
            {
                CreditorFullName= creditor.FirstName + " " + creditor.LastName,
                Title = "فعالیت - " + item.CostGroup.Title,
                Amount = amount,
                AmountNumeric = amount.ToNumeric(),
                Description = item.Description ?? "",
                Date = item.Date.ToShamsi(),
                DateTime = item.Date
            });
        }

        list = list.OrderBy(i => i.DateTime).ToList();
        
        return View(list);
    }
}