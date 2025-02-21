using System.ComponentModel.DataAnnotations;

namespace GayQuyTuThien.RequestViewModel
{
    public class UserAddViewModel
    {
        [Required(ErrorMessage = "UserName is required.")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "FullName is required.")]
        public string? FullName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Phone is required.")]
        public string? PhoneNumber { get; set; }
        public string ApplicationRoleId { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "ConfirmPassword is required.")]
        [Compare("Password", ErrorMessage = "ConfirmPassword is wrong")]
        public string? ConfirmPassword { get; set; }
    }
}
