using Abp.Application.Navigation;
using Abp.Localization;
using Manager.Core;
using Manager.Core.Common.Authorization;

namespace Manager.Web
{
    /// <summary>
    /// This class defines menus for the application.
    /// </summary>
    public class ManagerNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            #region UserMenu

            context.Manager.Menus.Add(MenuNames.User.UserMenu.ToString(),
                new MenuDefinition(MenuNames.User.UserMenu.ToString(), L(""))
                    .AddItem(
                        new MenuItemDefinition(MenuNames.User.FullScreen.ToString(),
                            L(MenuNames.User.FullScreen.ToString()),
                            url: "#",
                            icon: "fa fa-arrows-alt",
                            requiresAuthentication: true,
                            customData: new MenuCustomData
                            {
                                Class = "icon-full-screen",
                                Tooltip = "data-placement=bottom data-toggle=tooltip"
                            }
                        )
                    )
                    .AddItem(
                        new MenuItemDefinition(MenuNames.User.Configuration.ToString(),
                            L(MenuNames.User.Configuration.ToString()),
                            url: "/Configuration/_Index",
                            icon: "fa fa-cog",
                            requiresAuthentication: true,
                            customData: new MenuCustomData
                            {
                                Class = "icon-setting",
                                Tooltip = "data-placement=bottom data-toggle=tooltip"
                            }
                        )
                    )
                    .AddItem(
                        new MenuItemDefinition(MenuNames.User.Logout.ToString(),
                            L(MenuNames.User.Logout.ToString()),
                            url: "/Account/Logout",
                            icon: "fa fa-power-off",
                            customData: new MenuCustomData
                            {
                                Class = "icon-logout",
                                Tooltip = "data-placement=bottom data-toggle=tooltip"
                            }
                        )
                    )
              );

            #endregion

            #region ModulesMenu 

            context.Manager.Menus.Add(MenuNames.Modules.ModulesMenu.ToString(),
                new MenuDefinition(MenuNames.Modules.ModulesMenu.ToString(), L(""))
                    .AddItem(
                        new MenuItemDefinition(MenuNames.Modules.General.ToString(),
                            L(MenuNames.Modules.General.ToString()),
                            icon: "glyphicon glyphicon-minus",
                            requiresAuthentication: true
                        ).AddItem(
                            new MenuItemDefinition(MenuNames.Modules.Dashboard.ToString(),
                                L(MenuNames.Modules.Dashboard.ToString()),
                                url: "/Dashboard/_Index",
                                icon: "fa fa-dashboard",
                                requiresAuthentication: true,
                                customData: new MenuCustomData
                                {
                                    Tooltip = "data-placement=right data-toggle=tooltip"
                                }
                            )
                        ).AddItem(
                            new MenuItemDefinition(MenuNames.Modules.Reports.ToString(),
                                L(MenuNames.Modules.Reports.ToString()),
                                url: "/Dashboard/_Index",
                                icon: "fa fa-bar-chart-o",
                                requiresAuthentication: true,
                                customData: new MenuCustomData
                                {
                                    Disabled = true,
                                    Tooltip = "data-placement=right data-toggle=tooltip"
                                }
                            )
                        ).AddItem(
                            new MenuItemDefinition(MenuNames.Modules.Emails.ToString(),
                                L(MenuNames.Modules.Emails.ToString()),
                                url: "/Dashboard/_Index",
                                icon: "fa fa-envelope",
                                requiresAuthentication: true,
                                customData: new MenuCustomData
                                {
                                    Disabled = true,
                                    Tooltip = "data-placement=right data-toggle=tooltip"
                                }
                            )
                        ).AddItem(
                            new MenuItemDefinition(MenuNames.Modules.Tickets.ToString(),
                                L(MenuNames.Modules.Tickets.ToString()),
                                url: "/Dashboard/_Index",
                                icon: "fa fa-envelope",
                                requiresAuthentication: true,
                                customData: new MenuCustomData
                                {
                                    Disabled = true,
                                    Tooltip = "data-placement=right data-toggle=tooltip"
                                }
                            )
                        )
                    )
                    .AddItem(
                        new MenuItemDefinition(MenuNames.Modules.Modules.ToString(),
                            L(MenuNames.Modules.Modules.ToString()),
                            icon: "glyphicon glyphicon-minus",
                             requiresAuthentication: true
                        ).AddItem(
                            new MenuItemDefinition(MenuNames.Modules.Customers.ToString(),
                                L(MenuNames.Modules.Customers.ToString()),
                                url: "/Customers/Index/_Index",
                                icon: "fa fa-users",
                                requiresAuthentication: true,
                                customData: new MenuCustomData
                                {
                                    Disabled = true,
                                    Tooltip = "data-placement=right data-toggle=tooltip"
                                }
                            )
                        )
                    )
                );


            #region CustomersPageMenu

            //context.Manager.Menus.Add(MenuNames.Customers.CustomersPageMenu.ToString(),
            //    new MenuDefinition(MenuNames.Customers.CustomersPageMenu.ToString(), L(""))
            //     .AddItem(
            //           new MenuItemDefinition(MenuNames.Customers.List.ToString(),
            //               L(MenuNames.Customers.List.ToString()),
            //               url: "/Index/_List",
            //               icon: "fa fa-list",
            //               requiresAuthentication: true
            //           )
            //     )
                
            //);

            #endregion

            #endregion

            #region TabsMenu

            #region DashboardPageMenu

            context.Manager.Menus.Add(MenuNames.Dashboard.DashboardPageMenu.ToString(),
                new MenuDefinition(MenuNames.Dashboard.DashboardPageMenu.ToString(), L(""))
               .AddItem(
                   new MenuItemDefinition(MenuNames.Dashboard.UserInfo.ToString(),
                       L(MenuNames.Dashboard.UserInfo.ToString()),
                       url: "/Dashboard/_UserInfo",
                       icon: "fa fa-user",
                       requiresAuthentication: true
                   )
                )
                 .AddItem(
                   new MenuItemDefinition(MenuNames.Dashboard.BrowserInfo.ToString(),
                        L(MenuNames.Dashboard.BrowserInfo.ToString()),
                        url: "/Dashboard/_BrowserInfo",
                        icon: "fa fa-bank",
                        requiresAuthentication: true,
                        customData: new MenuCustomData
                        {
                            Disabled = true
                        }
                   )
                )
            );

            #endregion

            #region ConfigurationPageMenu

            context.Manager.Menus.Add(MenuNames.Configuration.ConfigurationPageMenu.ToString(),
                new MenuDefinition(MenuNames.Configuration.ConfigurationPageMenu.ToString(), L(""))
                .AddItem(
                   new MenuItemDefinition(MenuNames.Configuration.Profile.ToString(),
                       L(MenuNames.Configuration.Profile.ToString()),
                       url: "/Configuration/_Profile",
                       icon: "fa fa-users",
                       requiresAuthentication: true
                   )
                )
                .AddItem(
                   new MenuItemDefinition(MenuNames.Configuration.Billing.ToString(),
                       L(MenuNames.Configuration.Billing.ToString()),
                       url: "/Configuration/_Billing",
                       icon: "fa fa-users",
                       requiresAuthentication: true,
                        customData: new MenuCustomData
                        {
                            Disabled = true
                        }
                   )
                )
                .AddItem(
                   new MenuItemDefinition(MenuNames.Configuration.Company.ToString(),
                       L(MenuNames.Configuration.Company.ToString()),
                       url: "/Configuration/_Company",
                       icon: "fa fa-users",
                       requiresAuthentication: true,
                        customData: new MenuCustomData
                        {
                            Disabled = true
                        }
                   )
                )
                .AddItem(
                   new MenuItemDefinition(MenuNames.Configuration.Users.ToString(),
                       L(MenuNames.Configuration.Users.ToString()),
                       url: "/Configuration/_Users",
                       icon: "fa fa-users",
                       requiresAuthentication: true,
                        customData: new MenuCustomData
                        {
                            Disabled = true
                        }
                   )
                )
                 .AddItem(
                   new MenuItemDefinition(MenuNames.Configuration.Roles.ToString(),
                       L(MenuNames.Configuration.Roles.ToString()),
                       url: "/Configuration/_Roles",
                       icon: "fa fa-cog",
                       requiresAuthentication: true
                   )
                )
                .AddItem(
                   new MenuItemDefinition(MenuNames.Configuration.AuditsLog.ToString(),
                       L(MenuNames.Configuration.AuditsLog.ToString()),
                       url: "/Configuration/_AuditsLog",
                       icon: "fa fa-users",
                       requiresAuthentication: true,
                        customData: new MenuCustomData
                        {
                            Disabled = true
                        }
                   )
                )
            );

            #endregion

            #endregion
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString("MENU." + name, ManagerConsts.LocalizationSourceName);
        }
    }

    public class MenuCustomData
    {
        public string Class { get; set; }
        public string Tooltip { get; set; }
        public bool Disabled { get; set; }
    }

    public static class MenuNames
    {
        public enum User
        {
            UserMenu,
            FullScreen,
            Configuration,
            Logout
        }

        public enum Modules
        {
            ModulesMenu,
            General,
            Dashboard,
            Reports,
            Emails,
            Tickets,
            Modules,
            Customers
        }

        #region Menu names Pages
        
        public enum Dashboard
        {
            DashboardPageMenu,
            UserInfo,
            BrowserInfo
        }

        public enum Configuration
        {
            ConfigurationPageMenu,
            Profile,
            Billing,
            Company,
            Roles,
            Users,
            AuditsLog
        }

        public enum Customers
        {
            CustomersPageMenu,
            Customers,
            List
        }

        #endregion
    }
}
