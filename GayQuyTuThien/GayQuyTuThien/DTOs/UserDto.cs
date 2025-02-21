namespace GayQuyTuThien.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Code { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? Avatar { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ApplicationRoleId { get; set; }
        public string ApplicationRoleName { get; set; }
        public DateTimeOffset? LockedEnd { get; set; }
        public bool LockedEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string? SecretKey { get; set; }
        public string? Scopes { get; set; }
    }
}
