using EmployeesManager.Application.Features.SystemCodeDetails.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.SystemCodeDetails.Queries.GetSystemCodeDetailsBySystemCode;

public sealed record GetSystemCodeDetailsBySystemCodeQuery(string SystemCode)
    : IRequest<Result<List<SystemCodeDetailDto>>>;
