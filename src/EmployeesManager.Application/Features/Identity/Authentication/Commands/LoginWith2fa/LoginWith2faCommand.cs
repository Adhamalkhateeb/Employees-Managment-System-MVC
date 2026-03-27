using EmployeesManager.Application.Features.Identity.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Identity.Authentication.Commands.LoginWith2fa;

public sealed record LoginWith2faCommand(
    string TwoFactorCode,
    bool RememberMe,
    bool RememberMachine
) : IRequest<Result<AppUserDto>>;
