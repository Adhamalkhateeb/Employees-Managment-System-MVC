namespace EmployeesManager.Domain.Identity.Events;

public sealed record ApplicationUserRegisteredDomainEvent(
    Guid UserId,
    string Email,
    DateTime OccurredOnUtc
);
