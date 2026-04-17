using FluentValidation;

namespace EmployeesManager.Application.Features.SystemCodes.Commands.DeleteSystemCode;

public sealed class DeleteSystemCodeCommandValidator : AbstractValidator<DeleteSystemCodeCommand>
{
    public DeleteSystemCodeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}
