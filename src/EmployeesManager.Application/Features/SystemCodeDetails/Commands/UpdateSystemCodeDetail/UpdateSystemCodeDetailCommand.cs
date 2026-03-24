using EmployeesManager.Application.Features.SystemCodeDetails.Common;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.SystemCodeDetails.Commands.UpdateSystemCodeDetail;

public sealed record UpdateSystemCodeDetailCommand(
    Guid Id,
    Guid SystemCodeId,
    string Code,
    string Description,
    int? OrderNo
) : IRequest<Result<Updated>>, ISystemCodeDetailCommand;
