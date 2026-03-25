using FluentValidation;

namespace EmployeesManager.Application.Features.SystemCodeDetails.Queries.GetSystemCodeDetailById;

public sealed class GetSystemCodeDetailByIdQueryValidator
    : AbstractValidator<GetSystemCodeDetailByIdQuery>
{
    public GetSystemCodeDetailByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}
