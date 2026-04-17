using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Designations.Commands.DeleteDesignation;

public sealed record DeleteDesignationCommand(Guid Id) : IRequest<Result<Deleted>>;
