using EmployeesManager.Application.Features.SystemCodeDetails.Common;
using EmployeesManager.Application.Features.SystemCodeDetails.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.SystemCodeDetails.Commands.CreateSystemCodeDetail;

public sealed record CreateSystemCodeDetailCommand(
    Guid SystemCodeId,
    string Code,
    string Description,
    int? OrderNo
) : IRequest<Result<SystemCodeDetailDto>>, ISystemCodeDetailCommand;
