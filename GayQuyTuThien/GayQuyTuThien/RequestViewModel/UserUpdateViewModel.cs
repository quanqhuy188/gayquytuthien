using System.ComponentModel.DataAnnotations;

namespace GayQuyTuThien.RequestViewModel
{
    public class UserUpdateViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "FullName is required.")]
        public string? FullName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Code is required.")]
        public string? PhoneNumber { get; set; }
        public string ApplicationRoleId { get; set; }
    }
}
