using EmployeesManager.Application.Features.Designations.Common;
using EmployeesManager.Application.Features.Designations.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Designations.Commands.CreateDesignation;

public sealed record CreateDesignationCommand(string Name)
    : IRequest<Result<DesignationDto>>,
        IDesignationCommand;
