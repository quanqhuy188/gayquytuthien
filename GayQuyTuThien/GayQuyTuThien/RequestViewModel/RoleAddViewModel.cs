using System.ComponentModel.DataAnnotations;

namespace GayQuyTuThien.RequestViewModel
{
    public class RoleAddViewModel
    {
        [Required(ErrorMessage = "RoleName is required.")]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
