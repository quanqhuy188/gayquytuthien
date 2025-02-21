namespace GayQuyTuThien.DTOs
{
    public class FunctionDto
    {
        public string Id { get; set; }
        public string Name { set; get; }
        public string? Slug { set; get; }
        public string? ParentId { set; get; }
        public string? IconCss { get; set; }
        public int SortOrder { set; get; }
        public int Status { set; get; }
        public int Level { get; set; }
    }
}
