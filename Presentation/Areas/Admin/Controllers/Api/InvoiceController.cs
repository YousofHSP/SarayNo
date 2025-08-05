using AutoMapper;
using Common.Utilities;
using Data.Contracts;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;
using Service.Model.Contracts;

namespace Presentation.Areas.Admin.Controllers.Api;

[ApiController]
[Area("Admin")]
public class InvoiceController : Controller
{
    private readonly IMapper _mapper;
    private readonly IRepository<InvoiceDetail> _invoiceDetailRepository;
    private readonly IRepository<InvoiceLog> _invoiceLogRepository;
    private readonly IRepository<Payoff> _payoffRepository;
    private readonly IRepository<Invoice> _invoiceRepository;
    private readonly IUploadedFileService _uploadedFileService;

    public InvoiceController(IMapper mapper, IRepository<InvoiceDetail> invoiceDetailRepository, IRepository<Invoice> invoiceRepository, IUploadedFileService uploadedFileService, IRepository<Payoff> payoffRepository, IRepository<InvoiceLog> invoiceLogRepository)
    {
        _mapper = mapper;
        _invoiceDetailRepository = invoiceDetailRepository;
        _invoiceRepository = invoiceRepository;
        _uploadedFileService = uploadedFileService;
        _payoffRepository = payoffRepository;
        _invoiceLogRepository = invoiceLogRepository;
    }

    [HttpPost("[area]/api/[controller]/[action]")]
    public async Task<IActionResult> Add([FromBody]InvoiceDto dto, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(dto.Amount))
            dto.Amount = "0";
        var model = dto.ToEntity(_mapper);
        model.Status = InvoiceStatus.Pending;
        await _invoiceRepository.AddAsync(model, ct);
        return Ok();
    }
    [HttpGet("[area]/api/[controller]/{id:int}/[action]")]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken ct)
    {
        var model = await _invoiceRepository.TableNoTracking
            .FirstOrDefaultAsync(i => i.Id == id, ct);
        if (model is null)
            return NotFound();
        return Ok(model);
    }
    [HttpPost("[area]/api/[controller]/[action]")]
    public async Task<IActionResult> Update([FromBody]InvoiceUpdateDto dto, CancellationToken ct)
    {

        var model = await _invoiceRepository.GetByIdAsync(ct, dto.Id);
        if(model is null)
            return NotFound();
        if(string.IsNullOrEmpty(dto.Discount))
            dto.Discount = "0";
        model.Discount = decimal.Parse(dto.Discount);
        if(dto.IsDone)
            model.Type = InvoiceType.Settled;
        await _invoiceRepository.UpdateAsync(model, ct);
        return Ok();
    }
    [HttpPost("[area]/api/[controller]/[action]")]
    public async Task<IActionResult> AddDetail([FromBody]InvoiceDetailDto dto, CancellationToken ct)
    {
        var model = dto.ToEntity(_mapper);
        model.Status = InvoiceDetailStatus.Pending;
        await _invoiceDetailRepository.AddAsync(model, ct);
        return Ok();
    }
    [HttpPost("[area]/api/[controller]/[action]")]
    public async Task<IActionResult> ChangeStatus(InvoiceChangeStatusDto dto, CancellationToken ct)
    {
        
        var invoice = await _invoiceRepository.GetByIdAsync(ct, dto.Id);
        if(invoice is null)
            return NotFound();

        invoice.Status = dto.Status;
        await _invoiceRepository.UpdateAsync(invoice, ct);
        var userId = User.Identity!.GetUserId<int>();
        var log = new InvoiceLog
        {
            Status = dto.Status,
            InvoiceId = dto.Id,
            CreatedAt = DateTimeOffset.Now,
            CreatorUserId = userId,
            Description = dto.Description ?? ""
        };
        await _invoiceLogRepository.AddAsync(log, ct);
        
        return Ok();
    }
    [HttpGet("[area]/api/[controller]/{id:int}/[action]")]
    public async Task<IActionResult> ChangeType([FromRoute] int id, [FromQuery]InvoiceType type, CancellationToken ct)
    {
        
        var invoice = await _invoiceRepository.GetByIdAsync(ct, id);
        if(invoice is null)
            return NotFound();
        
        invoice.Type = type;
        invoice.Status = InvoiceStatus.Pending;
        await _invoiceRepository.UpdateAsync(invoice, ct);
        
        return Ok();
    }
    [HttpPost("[area]/api/[controller]/{id:int}/[action]")]
    public async Task<IActionResult> ApprovedDetail([FromRoute] int id, CancellationToken ct)
    {
        var model = await _invoiceDetailRepository.Table
            .FirstOrDefaultAsync(i => i.Id == id, ct);
        if (model is null)
            return NotFound();
        if (model.Status == InvoiceDetailStatus.Pending)
        {
            model.Status = InvoiceDetailStatus.Approved;
            await _invoiceDetailRepository.UpdateAsync(model, ct);
        }

        return Ok();
    }
    
    [HttpPost("[area]/api/[controller]/[action]")]
    public async Task<IActionResult> AddPayoff(PayoffDto dto, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(dto.Price))
            dto.Price = "0";
        var invoice = await _invoiceRepository.TableNoTracking
            .FirstOrDefaultAsync(i => i.Id == dto.InvoiceId, ct);
        if (invoice is null)
            return NotFound();
        var model = dto.ToEntity(_mapper);
        model.Status = PayoffStatus.Pending;
        await _payoffRepository.AddAsync(model, ct);
        var res = PayoffResDto.FromEntity(model, _mapper);
        return Ok(res);
    }
}