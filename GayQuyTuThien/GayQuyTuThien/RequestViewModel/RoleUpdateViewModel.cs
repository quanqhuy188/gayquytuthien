using System.ComponentModel.DataAnnotations;

namespace GayQuyTuThien.RequestViewModel
{
    public class RoleUpdateViewModel
    {
        [Required(ErrorMessage = "RoleName is required.")]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
