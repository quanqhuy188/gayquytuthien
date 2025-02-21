using System.ComponentModel.DataAnnotations;

namespace GayQuyTuThien.RequestViewModel
{
	public class RoleViewModel
	{
        public string? Id { get; set; }
        [Required(ErrorMessage = "RoleName is required.")]
        public string Name { get; set; }
        public int? UserCount { get; set; }
        public string? Description { get; set; }
    }
}
