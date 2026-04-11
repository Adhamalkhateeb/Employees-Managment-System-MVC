using EmployeesManager.Application.Features.Countries.Common;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Countries.Commands.UpdateCountry;

public sealed record UpdateCountryCommand(Guid Id, string Code, string Name)
    : IRequest<Result<Updated>>,
        ICountryCommand;
