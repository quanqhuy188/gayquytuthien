using GayQuyTuThien.DataContext.Entity;

namespace GayQuyTuThien.DTOs
{
    public class PermissionDto
    {
        public string Id { get; set; }
        public string FunctionId { get; set; }
        public string ApplicationRoleId { get; set; }
        public bool CanCreate { get; set; }
        public bool CanRead { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public ApplicationRole ApplicationRole { get; set; }
        public Function Function { get; set; }
    }
}
