using AutoMapper;
using Data.Contracts;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;

namespace Presentation.Areas.Admin.Controllers;

[Area("Admin")]
public class CostGroupController(IRepository<CostGroup> repository, IMapper mapper) : BaseController<CostGroupDto, CostGroupResDto, CostGroup>(repository, mapper)
{
    
}