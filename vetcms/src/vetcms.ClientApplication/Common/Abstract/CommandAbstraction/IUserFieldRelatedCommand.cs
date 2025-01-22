namespace vetcms.ClientApplication.Common.CommandAbstraction
{
    public interface IUserFieldRelatedCommand
    {
        public int TargetUserId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string VisibleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
