using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Countries.Commands.DeleteCountry;

public sealed record DeleteCountryCommand(Guid Id) : IRequest<Result<Deleted>>;
