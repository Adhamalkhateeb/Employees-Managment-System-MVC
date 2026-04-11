using FluentValidation;

namespace EmployeesManager.Application.Features.LeaveTypes.Queries.GetLeaveTypeById;

public sealed class GetLeaveTypeByIdQueryValidator : AbstractValidator<GetLeaveTypeByIdQuery>
{
    public GetLeaveTypeByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}
