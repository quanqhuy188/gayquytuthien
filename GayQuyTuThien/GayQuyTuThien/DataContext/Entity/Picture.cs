using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GayQuyTuThien.DataContext.Entity
{
	[Table("Picture")]
	public class Picture
	{
		[Key]
		public string Id { get; set; }

		public DateTime CreatedOn { get; set; }

		public string? Guid { get; set; }
	}
}
