using System.ComponentModel.DataAnnotations;

namespace EmployeesManager.Contracts.Requests.LeaveApplications;

public sealed class CreateLeaveApplicationRequest
{
    [Required(ErrorMessage = "Employee is required")]
    public Guid EmployeeId { get; set; }

    [Required(ErrorMessage = "Leave type is required")]
    public Guid LeaveTypeId { get; set; }

    [Required(ErrorMessage = "Duration is required")]
    public string Duration { get; set; } = default!;

    [Required(ErrorMessage = "Start date is required")]
    public DateTimeOffset StartDate { get; set; }

    [Required(ErrorMessage = "End date is required")]
    public DateTimeOffset EndDate { get; set; }

    [Required(ErrorMessage = "Description is required")]
    [StringLength(1000)]
    public string Description { get; set; } = default!;

    [StringLength(500)]
    public string? Attachment { get; set; }
}
