using System.Globalization;
using EmployeesManager.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.SystemCodeDetails.Common;

public static class SystemCodeLookUpConstants
{
    public const string LeaveTypeSystemCode = "LEAVE_TYPE";
    public const string LeaveApplicationStatusSystemCode = "LEAVE_APPLICATION_STATUS";
    public const string LeaveDurationSystemCode = "LEAVE_DURATION";
    public const string Gender = "GENDER";
}
