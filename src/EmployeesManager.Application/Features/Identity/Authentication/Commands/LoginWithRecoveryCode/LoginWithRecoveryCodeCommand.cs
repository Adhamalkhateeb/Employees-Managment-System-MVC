using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Identity.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Identity.Authentication.Commands.LoginWithRecoveryCode;

public sealed record LoginWithRecoveryCodeCommand(string RecoveryCode)
    : IRequest<Result<AppUserDto>>;
