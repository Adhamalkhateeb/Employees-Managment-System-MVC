using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Identity.Email.Commands.SendVerificationEmail;

public sealed record SendVerificationEmailCommand(string ConfirmEmailBaseUrl, string? ReturnUrl)
    : IRequest<Result<Updated>>;
