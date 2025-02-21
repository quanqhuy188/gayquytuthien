using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GayQuyTuThien.DataContext.Entity
{
    public class ApplicationRole : IdentityRole
    {
        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
