using EmployeesManager.Application.Common.Interfaces.Identity;
using EmployeesManager.Application.Features.Identity.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Identity.Authentication.Commands.LoginWithRecoveryCode;

public sealed class LoginWithRecoveryCodeCommandHandler(IAuthService authService)
    : IRequestHandler<LoginWithRecoveryCodeCommand, Result<AppUserDto>>
{
    private readonly IAuthService _authService = authService;

    public Task<Result<AppUserDto>> Handle(
        LoginWithRecoveryCodeCommand request,
        CancellationToken cancellationToken
    ) => _authService.LoginWithRecoveryCodeAsync(request.RecoveryCode);
}
