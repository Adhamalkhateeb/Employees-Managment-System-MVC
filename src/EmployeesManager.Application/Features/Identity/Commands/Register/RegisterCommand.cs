using EmployeesManager.Application.Features.Identity.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Identity.Commands.Register;

public sealed record RegisterCommand(
    string UserName,
    string Email,
    string PhoneNumber,
    string Password
) : IRequest<Result<AppUserDto>>;
