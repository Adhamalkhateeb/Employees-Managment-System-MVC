using EmployeesManager.Application.Common.Interfaces.Identity;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Identity.Email.Commands.ConfirmEmail;

public sealed class ConfirmEmailCommandHandler
    : IRequestHandler<ConfirmEmailCommand, Result<Updated>>
{
    private readonly IEmailService _emailService;

    public ConfirmEmailCommandHandler(IEmailService emailService) => _emailService = emailService;

    public Task<Result<Updated>> Handle(
        ConfirmEmailCommand request,
        CancellationToken cancellationToken
    ) => _emailService.ConfirmEmailAsync(request.UserId, request.Code);
}
