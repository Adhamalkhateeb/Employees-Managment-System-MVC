using EmployeesManager.Application.Features.Countries.Common;
using EmployeesManager.Application.Features.Countries.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Countries.Commands.CreateCountry;

public sealed record CreateCountryCommand(string Code, string Name)
    : IRequest<Result<CountryDto>>,
        ICountryCommand;
