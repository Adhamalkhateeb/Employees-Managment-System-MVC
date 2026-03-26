using EmployeesManager.Domain.Common;

namespace EmployeesManager.Domain.Entities.LeaveApplications
{
    public class DecisionActivity : AuditableEntity
    {
        public Guid? DecisionById { get; protected set; }
        public DateTimeOffset? DecisionAtUtc { get; protected set; }

        protected DecisionActivity()
        {
            // Required by EF Core
        }

        protected DecisionActivity(Guid id)
            : base(id) { }

        protected void SetDecision(Guid user)
        {
            DecisionById = user;
            DecisionAtUtc = DateTimeOffset.UtcNow;
        }
    }
}
