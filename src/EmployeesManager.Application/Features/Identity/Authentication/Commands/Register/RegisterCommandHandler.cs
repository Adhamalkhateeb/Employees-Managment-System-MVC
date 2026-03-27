using EmployeesManager.Application.Common.Interfaces.Identity;
using EmployeesManager.Application.Features.Identity.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Identity.Authentication.Commands.Register;

public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AppUserDto>>
{
    private readonly IAuthService _authService;

    public RegisterCommandHandler(IAuthService authService) => _authService = authService;

    public async Task<Result<AppUserDto>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken
    )
    {
        var registeredResult = await _authService.RegisterAsync(
            command.UserName,
            command.Email,
            command.PhoneNumber,
            command.Password,
            command.ConfirmationBaseUrl,
            command.ReturnUrl
        );

        if (registeredResult.IsError)
            return registeredResult.Errors;

        return registeredResult.Value;
    }
}
