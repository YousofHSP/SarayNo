using AutoMapper;
using Common.Utilities;
using Domain;

namespace Presentation.DTO;

public class EmployerPaymentDto : BaseDto<EmployerPaymentDto, EmployerPayment>
{
    public int ProjectId { get; set; }
    public string? Description { get; set; }
    public string Date { get; set; }
    public float Amount { get; set; }

    protected override void CustomMappings(IMappingExpression<EmployerPaymentDto, EmployerPayment> mapping)
    {
        mapping.ForMember( 
            m => m.Date,
            s => s.MapFrom(d => d.Date.ToGregorian()));
    }
}

public class EmployerPaymentResDto : BaseDto<EmployerPaymentResDto, EmployerPayment>
{

    public string ProjectTitle { get; set; }
    public string? Description{ get; set; }
    public string Date { get; set; }
    public string AmountNumeric { get; set; }
    public decimal Amount { get; set; }

    protected override void CustomMappings(IMappingExpression<EmployerPayment, EmployerPaymentResDto> mapping)
    {
        mapping.ForMember(
            d => d.Date,
            s => s.MapFrom(m => m.Date.ToShamsi()));
        mapping.ForMember(
            d => d.AmountNumeric,
            s => s.MapFrom(m => m.Amount.ToNumeric()));
    }
}