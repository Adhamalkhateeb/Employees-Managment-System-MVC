using EmployeesManager.Domain.Common.Results;
using Microsoft.AspNetCore.Identity;

namespace EmployeesManager.Application.Common.Interfaces.Identity;

public static class IdentityMapHelperExtensions
{
    public static List<Error> MapIdentityErrors(this IEnumerable<IdentityError> errors) =>
        [
            .. errors.Select(e =>
                e.Code is "DuplicateEmail" or "DuplicateUserName"
                    ? Error.Conflict(e.Code, e.Description, MapPropertyName(e.Code))
                    : Error.Validation(e.Code, e.Description, MapPropertyName(e.Code))
            ),
        ];

    private static string? MapPropertyName(string? identityCode) =>
        identityCode switch
        {
            "DuplicateEmail" or "InvalidEmail" => "Email",
            "DuplicateUserName" or "InvalidUserName" => "UserName",
            "InvalidPhoneNumber" => "PhoneNumber",
            "PasswordMismatch"
            or "PasswordTooShort"
            or "PasswordRequiresNonAlphanumeric"
            or "PasswordRequiresDigit"
            or "PasswordRequiresLower"
            or "PasswordRequiresUpper"
            or "PasswordRequiresUniqueChars" => "Password",
            _ => null,
        };
}
