using EmployeesManager.Application.Features.Cities.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Cities.Queries.GetAllCities;

public sealed record GetAllCitiesQuery() : IRequest<Result<List<CityDto>>>;
