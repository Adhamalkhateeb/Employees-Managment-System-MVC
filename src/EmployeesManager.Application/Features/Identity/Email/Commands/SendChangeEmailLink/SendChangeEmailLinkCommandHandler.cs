using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Common.Interfaces.Identity;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Identity.Email.Commands.SendChangeEmailLink;

public sealed class SendChangeEmailLinkCommandHandler(
    IEmailService identityService,
    ICurrentUser currentUser
) : IRequestHandler<SendChangeEmailLinkCommand, Result<Updated>>
{
    private readonly IEmailService _identityService = identityService;
    private readonly ICurrentUser _currentUser = currentUser;

    public Task<Result<Updated>> Handle(
        SendChangeEmailLinkCommand request,
        CancellationToken cancellationToken
    )
    {
        if (!_currentUser.IsAuthenticated)
            return Task.FromResult<Result<Updated>>(
                Error.Unauthorized("Identity.Unauthorized", "Unauthorized.")
            );

        return _identityService.SendChangeEmailLinkAsync(
            _currentUser.Id,
            request.NewEmail,
            request.ConfirmEmailChangeBaseUrl,
            request.ReturnUrl
        );
    }
}
