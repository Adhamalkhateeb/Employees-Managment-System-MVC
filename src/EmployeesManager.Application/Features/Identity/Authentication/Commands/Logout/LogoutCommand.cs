using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Identity.Authentication.Commands.Logout;

public sealed record LogoutCommand : IRequest<Result<Success>>;
