using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GayQuyTuThien.DataContext.Entity
{
    [Table("Functions")]
    public class Function
    {
        [Key]
        [StringLength(450)]
        public string Id { get; set; }
        [StringLength(128)]
        public string Name { set; get; }
        [StringLength(250)]
        public string? Slug { set; get; }
        [StringLength(128)]
        public string? ParentId { set; get; }
        [StringLength(250)]
        public string? IconCss { get; set; }
        public int SortOrder { set; get; }
        public int Status { set; get; }
        public int Level { get; set; }
    }
}
