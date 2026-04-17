using EmployeesManager.Application.Features.Countries.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Countries.Queries.GetAllCountries;

public sealed record GetAllCountriesQuery() : IRequest<Result<List<CountryDto>>>;
