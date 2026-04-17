using EmployeesManager.Application.Features.SystemCodes.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.SystemCodes.Queries.GetAllSystemCodes;

public sealed record GetAllSystemCodesQuery() : IRequest<Result<List<SystemCodeDto>>>;
