using EmployeesManager.Application.Features.Cities.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Cities.Queries.GetCityById;

public sealed record GetCityByIdQuery(Guid Id) : IRequest<Result<CityDto>>;
