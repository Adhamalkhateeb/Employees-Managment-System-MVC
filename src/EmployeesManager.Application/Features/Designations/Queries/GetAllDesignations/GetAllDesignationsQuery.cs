using EmployeesManager.Application.Features.Designations.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Designations.Queries.GetAllDesignations;

public sealed record GetAllDesignationsQuery() : IRequest<Result<List<DesignationDto>>>;
