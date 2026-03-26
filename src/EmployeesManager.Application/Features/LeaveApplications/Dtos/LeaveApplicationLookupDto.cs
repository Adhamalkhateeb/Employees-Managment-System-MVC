namespace EmployeesManager.Application.Features.LeaveApplications.Dtos;

public sealed record LeaveApplicationLookupDto(
    IEnumerable<EmployeeLookupDto> Employees,
    IEnumerable<LeaveTypeLookupDto> LeaveTypes,
    IEnumerable<string> Durations
);

public sealed record EmployeeLookupDto(Guid Id, string FullName);

public sealed record LeaveTypeLookupDto(Guid Id, string Code);
