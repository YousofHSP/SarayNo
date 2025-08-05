using Common.Utilities;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;
using Service.Model.Contracts;

namespace Presentation.Areas.Admin.Controllers;

[Area("Admin")]
public class InvoiceController : Controller
{
    private readonly IUploadedFileService _uploadedFileService;

    public InvoiceController(IUploadedFileService uploadedFileService)
    {
        _uploadedFileService = uploadedFileService;
    }

    [HttpPost("[area]/api/[controller]/[action]")]
    public async Task<IActionResult> AddImage(AddImageDto dto, CancellationToken ct)
    {
        
        var userId = User.Identity!.GetUserId<int>();
        await _uploadedFileService.UploadFileAsync(dto.File, dto.AlbumId, nameof(Invoice),
            dto.ModelId, userId, ct, dto.Description);
        if (!string.IsNullOrEmpty(dto.ReturnUrl))
            return Redirect(dto.ReturnUrl);
        return RedirectToAction("Images", "Project");
    }
    
}