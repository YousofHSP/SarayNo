using Common.Utilities;
using Data.Contracts;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;
using Service.Model.Contracts;

namespace Presentation.Areas.Admin.Controllers;

[Area("Admin")]
public class InvoiceController : Controller
{
    private readonly IRepository<Invoice> _invoiceRepository;
    private readonly IUploadedFileService _uploadedFileService;

    public InvoiceController(IUploadedFileService uploadedFileService, IRepository<Invoice> invoiceRepository)
    {
        _uploadedFileService = uploadedFileService;
        _invoiceRepository = invoiceRepository;
    }

    [HttpPost("[area]/[controller]/[action]")]
    public async Task<IActionResult> AddImage(AddImageDto dto, CancellationToken ct)
    {
        var userId = User.Identity!.GetUserId<int>();
        var invoice = await _invoiceRepository.TableNoTracking
            .FirstOrDefaultAsync(i => i.Id == dto.ModelId, ct);
        if (invoice is null)
        {
            TempData["ErrorMessage"] = " پیدا نشد";
        }
        else
        {
            await _uploadedFileService.UploadFileAsync(dto.File, dto.AlbumId, nameof(Invoice),
                dto.ModelId, userId, ct, dto.Description);
        }

        if (!string.IsNullOrEmpty(dto.ReturnUrl))
        {
            return Redirect(dto.ReturnUrl);
        }

        return RedirectToAction("Images", "Project", new { projectId = invoice?.ProjectId ?? 0 });
    }

    [HttpGet("[area]/[controller]/[action]/{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
    {
        var model = await _invoiceRepository.Table
            .FirstOrDefaultAsync(i => i.Id == id, ct);
        if (model is null)
        {
            TempData["ErrorMessage"] = "فاکتور پیدا نشد";
            return Redirect(Request.Headers.Referer.ToString());
        }

        await _invoiceRepository.DeleteAsync(model, ct);
        TempData["SuccessMessage"] = "پروژه با موفقیت حذف شد";
        return Redirect(Request.Headers.Referer.ToString());
    }
}