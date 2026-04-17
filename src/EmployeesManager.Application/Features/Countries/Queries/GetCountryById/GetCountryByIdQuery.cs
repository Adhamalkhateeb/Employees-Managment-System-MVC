using EmployeesManager.Application.Features.Countries.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Countries.Queries.GetCountryById;

public sealed record GetCountryByIdQuery(Guid Id) : IRequest<Result<CountryDto>>;
