using AutoMapper;
using Common.Utilities;
using Domain;

namespace Presentation.DTO;

public class PayoffDto : BaseDto<PayoffDto, Payoff>
{
    public int UnsettledInvoiceId { get; set; }
    public int ProjectId { get; set; }
    public float Price { get; set; }
    public PayoffType Type { get; set; }
    public string Number { get; set; }
    public string Date { get; set; }

    protected override void CustomMappings(IMappingExpression<PayoffDto, Payoff> mapping)
    {
        mapping.ForMember(
            m => m.Date,
            s => s.MapFrom(d => d.Date.ToGregorian()));
    }
}

public class PayoffResDto : BaseDto<PayoffResDto, Payoff>
{
    public int UnsettledInvoiceId { get; set; }
    public string Number { get; set; }
    public string Price { get; set; }
    public string TypeDisplay { get; set; }
    public string Date { get; set; }
    protected override void CustomMappings(IMappingExpression<Payoff, PayoffResDto> mapping)
    {
        mapping.ForMember(
            d => d.Price,
            s => s.MapFrom(m => m.Price.ToNumeric()));
        mapping.ForMember(
            d => d.TypeDisplay,
            s => s.MapFrom(m => m.Type.ToDisplay(EnumExtensions.DisplayProperty.Name)));
        mapping.ForMember(
            d => d.Date,
            s => s.MapFrom(m => m.Date.ToShamsi()));
    }
}