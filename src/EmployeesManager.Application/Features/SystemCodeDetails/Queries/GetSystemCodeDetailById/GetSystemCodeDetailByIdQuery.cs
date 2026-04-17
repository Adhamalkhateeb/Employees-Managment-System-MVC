using EmployeesManager.Application.Features.SystemCodeDetails.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.SystemCodeDetails.Queries.GetSystemCodeDetailById;

public sealed record GetSystemCodeDetailByIdQuery(Guid Id) : IRequest<Result<SystemCodeDetailDto>>;
