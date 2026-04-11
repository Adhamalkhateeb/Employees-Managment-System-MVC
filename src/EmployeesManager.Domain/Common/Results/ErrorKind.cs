namespace EmployeesManager.Domain.Common.Results;

public enum ErrorKind
{
    Validation,
    Conflict,
    NotFound,
    Unauthorized,
    Forbidden,
    Failure,
    Unexpected,
}
