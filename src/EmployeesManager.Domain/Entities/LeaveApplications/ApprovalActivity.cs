using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeesManager.Domain.Common;

namespace EmployeesManager.Domain.Entities.LeaveApplications
{
    public class ApprovalActivity : AuditableEntity
    {
        public string? ApprovedBy { get; set; }
        public DateTimeOffset? ApprovedAtUtc { get; set; }

        protected ApprovalActivity() { }

        protected ApprovalActivity(Guid id)
            : base(id) { }
    }
}
