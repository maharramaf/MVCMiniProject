namespace MVCMiniProject.Helpers
{
    public static class AppRoles
    {
        public const string User = "user";
        public const string Admin = "admin";
        public const string SuperAdmin = "superadmin";
        public const string Staff = "admin,superadmin";

        public static bool IsStaff(string? role)
        {
            return string.Equals(role, Admin, StringComparison.OrdinalIgnoreCase)
                || string.Equals(role, SuperAdmin, StringComparison.OrdinalIgnoreCase);
        }
    }
}
