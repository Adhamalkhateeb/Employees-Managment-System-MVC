using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Common.Interfaces.Identity;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Identity.Email.Commands.SendEmailConfirmation;

public sealed class SendEmailConfirmationCommandHandler
    : IRequestHandler<SendEmailConfirmationCommand, Result<Updated>>
{
    private readonly IEmailService _emailService;

    public SendEmailConfirmationCommandHandler(IEmailService emailService) =>
        _emailService = emailService;

    public Task<Result<Updated>> Handle(
        SendEmailConfirmationCommand request,
        CancellationToken cancellationToken
    ) =>
        _emailService.SendEmailConfirmationAsync(
            request.Email,
            request.ConfirmationBaseUrl,
            request.ReturnUrl
        );
}
