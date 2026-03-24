using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.SystemCodeDetails.Commands.DeleteSystemCodeDetail;

public sealed record DeleteSystemCodeDetailCommand(Guid Id) : IRequest<Result<Deleted>>;
