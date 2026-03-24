using EmployeesManager.Application.Features.SystemCodeDetails.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.SystemCodeDetails.Queries.GetAllSystemCodeDetails;

public sealed record GetAllSystemCodeDetailsQuery() : IRequest<Result<List<SystemCodeDetailDto>>>;
