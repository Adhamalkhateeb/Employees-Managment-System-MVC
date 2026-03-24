using EmployeesManager.Application.Features.Designations.Common;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Designations.Commands.UpdateDesignation;

public sealed record UpdateDesignationCommand(Guid Id, string Name)
    : IRequest<Result<Updated>>,
        IDesignationCommand;
