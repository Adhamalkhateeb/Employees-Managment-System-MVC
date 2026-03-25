using System.ComponentModel.DataAnnotations;

namespace EmployeesManager.Contracts.Requests.LeaveApplications;

public sealed class UpdateLeaveApplicationRequest
{
    [Required(ErrorMessage = "Employee is required")]
    public Guid EmployeeId { get; set; }

    [Required(ErrorMessage = "Leave type is required")]
    public Guid LeaveTypeId { get; set; }

    [Required(ErrorMessage = "Duration is required")]
    public Guid DurationId { get; set; }

    [Required(ErrorMessage = "Status is required")]
    public Guid StatusId { get; set; }

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
