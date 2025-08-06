using AutoMapper;
using Common.Utilities;
using Domain;

namespace Presentation.DTO;

public class InvoiceDto : BaseDto<InvoiceDto, Invoice>
{
    
    public int ProjectId { get; set; }
    public int CostGroupId { get; set; }
    public int CreditorId { get; set; }
    public string Amount { get; set; }
    public string Type { get; set; }
    public string Date { get; set; }
    public string? Description { get; set; }
    
    protected override void CustomMappings(IMappingExpression<InvoiceDto, Invoice> mapping)
    {
        mapping.ForMember(
            m => m.Date,
            s => s.MapFrom(d => d.Date.ToGregorian()));
    }
}

public class InvoiceResDto : BaseDto<InvoiceResDto, Invoice>
{
    
    public int ProjectId { get; set; }
    public int CostGroupId { get; set; }
    public int CreditorId { get; set; }
    public decimal Amount { get; set; }
    public decimal Discount { get; set; }
    public InvoiceType Type { get; set; }
    public InvoiceStatus Status { get; set; }
    public string Date { get; set; }
    public string? Description { get; set; }

    protected override void CustomMappings(IMappingExpression<Invoice, InvoiceResDto> mapping)
    {
        mapping.ForMember(d => d.Date,
            s => s.MapFrom(m => m.Date.ToShamsi()));
    }
}
public class InvoiceUpdateDto
{
    public int Id{ get; set; }
    public string Discount { get; set; }
    public bool IsDone { get; set; }
}

public class InvoiceChangeStatusDto
{
    public int Id{ get; set; }
    public InvoiceStatus Status { get; set; }
    public string? Description { get; set; }
}
