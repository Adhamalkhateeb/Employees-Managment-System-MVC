using EmployeesManager.Application.Features.SystemCodes.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.SystemCodes.Queries.GetSystemCodeById;

public sealed record GetSystemCodeByIdQuery(Guid Id) : IRequest<Result<SystemCodeDto>>;
