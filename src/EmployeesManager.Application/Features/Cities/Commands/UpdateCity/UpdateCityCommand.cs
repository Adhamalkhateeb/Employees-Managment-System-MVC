using EmployeesManager.Application.Features.Cities.Common;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Cities.Commands.UpdateCity;

public sealed record UpdateCityCommand(Guid Id, string Code, string Name, Guid CountryId)
    : IRequest<Result<Updated>>,
        ICityCommand;
