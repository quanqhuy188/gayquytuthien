using System.ComponentModel.DataAnnotations;
namespace GayQuyTuThien.RequestViewModel
{
	public class SubmitFormUpdateRequest
	{
		public string Id { get; set; }
		
		public string Username { get; set; }
		public string? Gender { get; set; }
	}
}
