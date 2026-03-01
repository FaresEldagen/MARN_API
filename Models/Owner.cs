namespace MARN_API.Models
{
    public class Owner : ApplicationUser
    {
        public decimal WithdrawableEarnings { get; set; } = 0m;
        public virtual ICollection<Property> Properties { get; set; } = new HashSet<Property>();
        public virtual ICollection<BookingRequest> BookingRequestsAsOwner { get; set; } = new HashSet<BookingRequest>();
        public virtual ICollection<Contract> ContractsAsOwner { get; set; } = new HashSet<Contract>();
    }
}
