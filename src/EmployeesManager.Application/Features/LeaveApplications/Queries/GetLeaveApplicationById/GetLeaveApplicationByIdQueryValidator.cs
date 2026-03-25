using FluentValidation;

namespace EmployeesManager.Application.Features.LeaveApplications.Queries.GetLeaveApplicationById;

public sealed class GetLeaveApplicationByIdQueryValidator : AbstractValidator<GetLeaveApplicationByIdQuery>
{
    public GetLeaveApplicationByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}
