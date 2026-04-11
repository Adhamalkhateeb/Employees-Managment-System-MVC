using FluentValidation;

namespace EmployeesManager.Application.Features.SystemCodeDetails.Commands.DeleteSystemCodeDetail;

public sealed class DeleteSystemCodeDetailCommandValidator : AbstractValidator<DeleteSystemCodeDetailCommand>
{
    public DeleteSystemCodeDetailCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}
