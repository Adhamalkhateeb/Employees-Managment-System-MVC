using FluentValidation;

namespace EmployeesManager.Application.Features.Designations.Commands.DeleteDesignation;

public sealed class DeleteDesignationCommandValidator : AbstractValidator<DeleteDesignationCommand>
{
    public DeleteDesignationCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}
