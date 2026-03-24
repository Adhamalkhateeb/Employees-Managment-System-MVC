using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Identity.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Identity.Commands.Register;

public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AppUserDto>>
{
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IIdentityService identityService) =>
        _identityService = identityService;

    public Task<Result<AppUserDto>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken
    ) =>
        _identityService.RegisterAsync(
            command.UserName,
            command.Email,
            command.PhoneNumber,
            command.Password,
            cancellationToken
        );
}
