using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Common.Interfaces.Identity;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Identity.Authentication.Commands.Logout;

public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result<Success>>
{
    private readonly IAuthService _identityService;

    public LogoutCommandHandler(IAuthService authService) => _identityService = authService;

    public async Task<Result<Success>> Handle(
        LogoutCommand request,
        CancellationToken cancellationToken
    )
    {
        await _identityService.LogoutAsync();
        return Result.Success;
    }
}
