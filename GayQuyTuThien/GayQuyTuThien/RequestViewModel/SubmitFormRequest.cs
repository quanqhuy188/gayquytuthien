namespace GayQuyTuThien.RequestViewModel
{
	public class SubmitFormRequest
	{
		public string Id { get; set; }
		public string Username { get; set; }
		public string? Gender { get; set; }

		public DateTime fromDate { get; set; }
		public DateTime toDate { get; set; }
	}
}
