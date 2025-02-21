namespace GayQuyTuThien.RequestViewModel
{
    public class PermissionUpdateViewModel
    {
        public string Id { get; set; }
        public string RoleId { get; set; }
        public bool CanCreate { get; set; }
        public bool CanRead { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
    }
}
