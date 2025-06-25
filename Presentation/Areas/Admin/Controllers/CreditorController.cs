using AutoMapper;
using Data.Contracts;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;

namespace Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CreditorController(IRepository<Creditor> repository, IMapper mapper) : BaseController<CreditorDto, CreditorResDto, Creditor>(repository, mapper)
{
    
}