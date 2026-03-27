using EmployeesManager.Application.Common.Interfaces.Identity;
using EmployeesManager.Application.Features.Identity.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Identity.Authentication.Commands.LoginWith2fa;

public sealed class LoginWith2faCommandHandler(IAuthService authService)
    : IRequestHandler<LoginWith2faCommand, Result<AppUserDto>>
{
    private readonly IAuthService _authService = authService;

    public async Task<Result<AppUserDto>> Handle(
        LoginWith2faCommand request,
        CancellationToken cancellationToken
    ) =>
        await _authService.LoginWithAuthenticatorCodeAsync(
            request.TwoFactorCode,
            request.RememberMe,
            request.RememberMachine
        );
}
