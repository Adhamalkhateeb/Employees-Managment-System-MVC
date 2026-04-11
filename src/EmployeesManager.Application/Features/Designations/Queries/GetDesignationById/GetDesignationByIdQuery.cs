using EmployeesManager.Application.Features.Designations.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Designations.Queries.GetDesignationById;

public sealed record GetDesignationByIdQuery(Guid Id) : IRequest<Result<DesignationDto>>;
