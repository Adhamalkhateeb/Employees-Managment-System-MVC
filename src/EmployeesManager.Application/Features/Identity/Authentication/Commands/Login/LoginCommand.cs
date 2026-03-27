using EmployeesManager.Application.Features.Identity.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Identity.Authentication.Commands.Login;

public sealed record LoginCommand(string Email, string Password, bool RememberMe)
    : IRequest<Result<AppUserDto>>;
