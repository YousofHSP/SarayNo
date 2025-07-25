using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Utilities;
using Data.Contracts;
using DocumentFormat.OpenXml.Bibliography;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;
using Presentation.Models;
using Service.Model.Contracts;

namespace Presentation.Areas.Admin.Controllers;

[Area("Admin")]
public class ActivityController(
    IRepository<Invoice> invoiceRepository,
    IRepository<Project> projectRepository) : Controller 
{
    [HttpGet("[area]/[controller]")]
    public async Task<IActionResult> Index([FromQuery] int? projectId, CancellationToken ct)
    {
        if (projectId is null or 0)
        {
            var projects = await projectRepository.TableNoTracking
                .Include(i => i.User)
                .ToListAsync(ct);
            ViewBag.Projects = projects;
            return View();
        }

        var project = await projectRepository.TableNoTracking.FirstOrDefaultAsync(i => i.Id == projectId, ct);
        ViewBag.Project = project;
        var list = await invoiceRepository.TableNoTracking
            .Where(i => i.ProjectId == projectId)
            .Where(i => i.Type == InvoiceType.Activity)
            .Include(i => i.InvoiceDetails)
            .Include(i => i.CostGroup)
            .Include(i => i.Creditor)
            .Include(i => i.Project)
            .ToListAsync(ct);
        return View(list);
    }

    [HttpGet("[area]/[controller]/[action]/{id}")]
    public async Task<IActionResult> Details([FromRoute] int id, CancellationToken ct)
    {
        var model = await invoiceRepository.TableNoTracking
            .Include(i => i.InvoiceDetails)
            .FirstOrDefaultAsync(i => i.Id == id, ct);
        if (model is null)
            return NotFound();

        return View(model);
    }


    [HttpPost("[area]/[controller]/[action]/{id:int}")]
    public async Task<IActionResult> SetTotalAmount([FromRoute] int id, decimal totalAmount, CancellationToken ct)
    {
        var model = await invoiceRepository.GetByIdAsync(ct, id);
        if (model is null)
            return NotFound();
        model.Amount = totalAmount;
        await invoiceRepository.UpdateAsync(model, ct);
        return RedirectToAction("Details", new { id, ct });
    }
}