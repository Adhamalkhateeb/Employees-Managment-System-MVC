using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Identity.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Identity.Queries.Login;

public sealed class LoginQueryHandler : IRequestHandler<LoginQuery, Result<AppUserDto>>
{
    private readonly IIdentityService _identityService;

    public LoginQueryHandler(IIdentityService identityService) =>
        _identityService = identityService;

    public Task<Result<AppUserDto>> Handle(LoginQuery query, CancellationToken cancellationToken) =>
        _identityService.LoginAsync(
            query.Email,
            query.Password,
            query.RememberMe,
            cancellationToken
        );
}
