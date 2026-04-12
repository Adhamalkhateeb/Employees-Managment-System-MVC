using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Domain.Entities.Branches;

public static class BranchErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("Branch.NotFound", $"Branch '{id}' was not found.", nameof(Branch.Id));

    public static Error AlreadyExists(string name) =>
        Error.Conflict(
            "Branch.AlreadyExists",
            $"A Branch named '{name}' already exists.",
            nameof(Branch.Name)
        );

    public static readonly Error NameRequired = Error.Validation(
        "Branch.NameRequired",
        "Name is required.",
        nameof(Branch.Name)
    );

    public static readonly Error NameTooLong = Error.Validation(
        "Branch.NameTooLong",
        $"Name must not exceed {BranchConstants.NameMaxLength} characters.",
        nameof(Branch.Name)
    );

    public static readonly Error AddressRequired = Error.Validation(
        "Branch.AddressRequired",
        "Address is required.",
        nameof(Branch.Address)
    );

    public static readonly Error AddressTooLong = Error.Validation(
        "Branch.AddressTooLong",
        $"Address must not exceed {BranchConstants.AddressMaxLength} characters.",
        nameof(Branch.Address)
    );
    public static readonly Error DuplicatePhone = Error.Conflict(
        "Branch.DuplicatePhone",
        "Phone is already in use.",
        nameof(Branch.Phone)
    );

    public static readonly Error DuplicateEmail = Error.Conflict(
        "Branch.DuplicateEmail",
        "Email is already in use.",
        nameof(Branch.Email)
    );

    public static readonly Error ManagerNotFound = Error.NotFound(
        "Branch.ManagerNotFound",
        "Manager was not found.",
        nameof(Branch.ManagerId)
    );
}
