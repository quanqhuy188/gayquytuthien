using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GayQuyTuThien.DataContext.Entity
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(250)]
        public string? FullName { get; set; }
        [MaxLength(5000)]
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        [MaxLength(50)]
        public string? Code { get; set; }
        [MaxLength(500)]
        public string? Avatar { get; set; }
        [MaxLength(500)]
        public string? Address { get; set; }
        public string ApplicationRoleId { get; set; }
        [ForeignKey("ApplicationRoleId")]
        public virtual ApplicationRole ApplicationRole { get; set; }
    }
}
