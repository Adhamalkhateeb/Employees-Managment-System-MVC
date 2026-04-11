using EmployeesManager.Application.Common.Interfaces.Identity;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Identity.Email.Commands.ConfirmEmailChange;

public sealed class ConfirmEmailChangeCommandHandler(IEmailService identityService)
    : IRequestHandler<ConfirmEmailChangeCommand, Result<Updated>>
{
    private readonly IEmailService _identityService = identityService;

    public Task<Result<Updated>> Handle(
        ConfirmEmailChangeCommand request,
        CancellationToken cancellationToken
    ) => _identityService.ConfirmEmailChangeAsync(request.UserId, request.Email, request.Code);
}
