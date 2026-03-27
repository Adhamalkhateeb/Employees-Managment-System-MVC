using EmployeesManager.Application.Features.Identity.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Identity.Authentication.Commands.Register;

public sealed record RegisterCommand(
    string UserName,
    string Email,
    string PhoneNumber,
    string Password,
    string ConfirmationBaseUrl,
    string? ReturnUrl = null
) : IRequest<Result<AppUserDto>>;
