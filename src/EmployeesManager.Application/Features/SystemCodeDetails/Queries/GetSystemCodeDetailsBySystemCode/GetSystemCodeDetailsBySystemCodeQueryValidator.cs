using FluentValidation;

namespace EmployeesManager.Application.Features.SystemCodeDetails.Queries.GetSystemCodeDetailsBySystemCode;

public sealed class GetSystemCodeDetailsBySystemCodeQueryValidator
    : AbstractValidator<GetSystemCodeDetailsBySystemCodeQuery>
{
    public GetSystemCodeDetailsBySystemCodeQueryValidator()
    {
        RuleFor(x => x.SystemCode).NotEmpty().WithMessage("System code cannot be empty");
    }
}
