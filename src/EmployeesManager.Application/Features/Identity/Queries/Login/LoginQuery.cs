using EmployeesManager.Application.Features.Identity.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Identity.Queries.Login;

public sealed record LoginQuery(string Email, string Password, bool RememberMe)
    : IRequest<Result<AppUserDto>>;
