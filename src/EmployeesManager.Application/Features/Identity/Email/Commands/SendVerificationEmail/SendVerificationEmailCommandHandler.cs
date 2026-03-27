using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Common.Interfaces.Identity;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Identity.Email.Commands.SendVerificationEmail;

public sealed class SendVerificationEmailCommandHandler(
    IEmailService emailService,
    ICurrentUser currentUser
) : IRequestHandler<SendVerificationEmailCommand, Result<Updated>>
{
    private readonly IEmailService _emailService = emailService;
    private readonly ICurrentUser _currentUser = currentUser;

    public Task<Result<Updated>> Handle(
        SendVerificationEmailCommand request,
        CancellationToken cancellationToken
    )
    {
        if (!_currentUser.IsAuthenticated)
            return Task.FromResult<Result<Updated>>(
                Error.Unauthorized("Identity.Unauthorized", "Unauthorized.")
            );

        return _emailService.SendVerificationEmailAsync(
            _currentUser.Id,
            request.ConfirmEmailBaseUrl,
            request.ReturnUrl
        );
    }
}
