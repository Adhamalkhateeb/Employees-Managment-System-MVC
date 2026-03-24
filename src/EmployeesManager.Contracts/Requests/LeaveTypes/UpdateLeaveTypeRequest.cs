using System.ComponentModel.DataAnnotations;

namespace EmployeesManager.Contracts.Requests.LeaveTypes;

public sealed class UpdateLeaveTypeRequest
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
}
