using FluentValidation;

namespace EmployeesManager.Application.Features.SystemCodes.Queries.GetSystemCodeById;

public sealed class GetSystemCodeByIdQueryValidator : AbstractValidator<GetSystemCodeByIdQuery>
{
    public GetSystemCodeByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}
