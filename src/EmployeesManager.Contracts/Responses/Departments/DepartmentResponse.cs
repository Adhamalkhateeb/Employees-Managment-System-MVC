namespace EmployeesManager.Contracts.Responses.Departments;

public sealed record DepartmentResponse(
    Guid Id,
    string Name,
    int EmployeesCount,
    Guid? ManagerId,
    string? ManagerFullName
);
