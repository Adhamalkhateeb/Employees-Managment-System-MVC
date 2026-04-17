using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Cities.Commands.DeleteCity;

public sealed record DeleteCityCommand(Guid Id) : IRequest<Result<Deleted>>;
