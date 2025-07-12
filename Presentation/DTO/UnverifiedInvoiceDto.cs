using AutoMapper;
using Common.Utilities;
using Domain;

namespace Presentation.DTO;

public class UnverifiedInvoiceDto : BaseDto<UnverifiedInvoiceDto, UnverifiedInvoice>
{
    
    public int ProjectId { get; set; }
    public int CostGroupId { get; set; }
    public int CreditorId { get; set; }
    public float Amount { get; set; }
    public string Date { get; set; }
    public string? Description { get; set; } = "";

    protected override void CustomMappings(IMappingExpression<UnverifiedInvoiceDto, UnverifiedInvoice> mapping)
    {
        mapping.ForMember(
            m => m.Date,
            s => s.MapFrom(d => d.Date.ToGregorian()));
    }
}