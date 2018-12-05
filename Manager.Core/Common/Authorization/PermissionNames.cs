namespace Manager.Core.Common.Authorization
{
    public static class PermissionNames
    {
        public const string Pages = "Pages";

        public const string Pages_Tenants = "Pages.Tenants";

        #region Pages Configuration

        public const string Pages_Configuration = "Pages.Configuration";

        // Users
        public const string Pages_Configuration_Users = "Pages.Configuration.Users";
        public const string Pages_Configuration_Users_Create = "Pages.Configuration.Users.Create";
        public const string Pages_Configuration_Users_Edit = "Pages.Configuration.Users.Edit";
        public const string Pages_Configuration_Users_Delete = "Pages.Configuration.Users.Delete";
        // Roles
        public const string Pages_Configuration_Roles = "Pages.Configuration.Roles";
        public const string Pages_Configuration_Roles_Create = "Pages.Configuration.Roles.Create";
        public const string Pages_Configuration_Roles_Edit = "Pages.Configuration.Roles.Edit";
        public const string Pages_Configuration_Roles_Delete = "Pages.Configuration.Roles.Delete";

        #endregion
    }
}