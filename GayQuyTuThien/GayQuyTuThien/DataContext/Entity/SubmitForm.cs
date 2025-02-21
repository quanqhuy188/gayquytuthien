using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GayQuyTuThien.DataContext.Entity
{
	[Table("SubmitForm")]
	public class SubmitForm
	{
		[Key]
		public string Id { get; set; }
		public string Username { get; set; }
		public string? Gender { get; set; }

		public DateTime CreatedOn { get; set; }
	}
}
