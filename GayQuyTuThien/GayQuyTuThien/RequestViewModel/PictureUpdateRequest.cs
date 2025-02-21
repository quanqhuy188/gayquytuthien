using System.ComponentModel.DataAnnotations;
namespace GayQuyTuThien.RequestViewModel
{
	public class PictureUpdateRequest
	{
		public string Id { get; set; }
		public string? Image { get; set; }
	}
}
