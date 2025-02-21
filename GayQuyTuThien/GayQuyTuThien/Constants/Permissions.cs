using GayQuyTuThien.Enums;

namespace GayQuyTuThien.Constants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModuleAdmin(string module)
        {
            List<string> permissions = new List<string>();
            foreach (RoleAction scope in (RoleAction[])Enum.GetValues(typeof(RoleAction)))
            {
                permissions.Add($"{PermissionConstants.Prefix}.{module}.{scope}");
            }
            return permissions;
        }
        public static List<string> GeneratePermissionsForModuleManager(string module)
        {
            List<string> permissions = new List<string>();
            foreach (RoleAction scope in (RoleAction[])Enum.GetValues(typeof(RoleAction)))
            {
                if (scope == RoleAction.Read || scope == RoleAction.Create)
                {
                    permissions.Add($"{PermissionConstants.Prefix}.{module}.{scope}");
                }
            }
            return permissions;
            //return new List<string>()
            //{
            //    $"Permissions.{module}.Read",
            //    $"Permissions.{module}.Create",
            //};
        }

        public static class PermissionConstants
        {
            public const string Prefix = "Permissions";
            public const string PermissionType = "Permission";
        }
    }
}
