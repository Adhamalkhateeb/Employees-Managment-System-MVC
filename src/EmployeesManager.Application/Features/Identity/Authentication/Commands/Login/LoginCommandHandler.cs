using EmployeesManager.Application.Common.Interfaces.Identity;
using EmployeesManager.Application.Features.Identity.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Identity.Authentication.Commands.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AppUserDto>>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService) => _authService = authService;

    public async Task<Result<AppUserDto>> Handle(
        LoginCommand query,
        CancellationToken cancellationToken
    ) => await _authService.LoginAsync(query.Email, query.Password, query.RememberMe);
}
