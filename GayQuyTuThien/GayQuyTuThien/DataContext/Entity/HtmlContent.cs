using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GayQuyTuThien.DataContext.Entity
{
    [Table("HtmlContent")]
    public class HtmlContent
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }
        
        public string Content { get; set; }
    }
}
