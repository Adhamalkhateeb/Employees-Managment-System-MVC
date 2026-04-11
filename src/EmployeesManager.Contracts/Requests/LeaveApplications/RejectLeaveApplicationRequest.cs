using System.ComponentModel.DataAnnotations;

namespace EmployeesManager.Contracts.Requests.LeaveApplications;

public sealed class RejectLeaveApplicationRequest
{
    [Required(ErrorMessage = "Rejection reason is required")]
    [StringLength(500)]
    public string RejectionReason { get; set; } = default!;
}
