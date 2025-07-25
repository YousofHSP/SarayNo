using AutoMapper;
using Common.Utilities;
using Domain;

namespace Presentation.DTO;

public class InvoiceDetailDto : BaseDto<InvoiceDetailDto, InvoiceDetail>
{
    public int InvoiceId { get; set; }
    public string Price { get; set; }
    public string? Description { get; set; }
    public string Date { get; set; }
    public string Type { get; set; }

    protected override void CustomMappings(IMappingExpression<InvoiceDetailDto, InvoiceDetail> mapping)
    {
        mapping.ForMember(
            m => m.Date,
            s => s.MapFrom(d => d.Date.ToGregorian()));
    }
}