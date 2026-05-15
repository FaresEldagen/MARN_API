using Microsoft.AspNetCore.Identity;

namespace MARN_API.Models
{
    public class Admin : ApplicationUser
    {
        public virtual ICollection<Report> ReportsReviewed { get; set; } = new HashSet<Report>();
        public virtual ICollection<AdminActionLog> ActionLogs { get; set; } = new HashSet<AdminActionLog>();
    }
}
