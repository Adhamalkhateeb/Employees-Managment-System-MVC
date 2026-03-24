using EmployeesManager.Application.Features.Cities.Common;
using EmployeesManager.Application.Features.Cities.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Cities.Commands.CreateCity;

public sealed record CreateCityCommand(string Code, string Name, Guid CountryId)
    : IRequest<Result<CityDto>>,
        ICityCommand;
