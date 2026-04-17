using FluentValidation;

namespace EmployeesManager.Application.Features.Banks.Queries.GetBankById;

public sealed class GetBankByIdQueryValidator : AbstractValidator<GetBankByIdQuery>
{
    public GetBankByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}
