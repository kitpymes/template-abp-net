using System.Web.Mvc;
using Abp.Application.Navigation;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.Threading;
using Manager.Web.Models.Layout;
using Manager.Application.Common.Sessions;
using Abp.Runtime.Session;
using System.IO;
using System;
using System.Configuration;

namespace Manager.Web.Controllers
{
    public class LayoutController : ManagerControllerBase
    {
        private readonly IUserNavigationManager _userNavigationManager;
        private readonly LanguageManager _languageManager;
        private readonly ISessionAppService _sessionAppService;
        private readonly IMultiTenancyConfig _multiTenancyConfig;

        public LayoutController(
            IUserNavigationManager userNavigationManager,
            LanguageManager languageManager,
            ISessionAppService sessionAppService, 
            IMultiTenancyConfig multiTenancyConfig)
        {
            _userNavigationManager = userNavigationManager;
            _languageManager = languageManager;
            _sessionAppService = sessionAppService;
            _multiTenancyConfig = multiTenancyConfig;
        }

        [ChildActionOnly]
        public PartialViewResult UserMenu()
        {
            return PartialView("_UserMenu", new UserMenuViewModel
            {
                Menu = AsyncHelper.RunSync(() => _userNavigationManager
                    .GetMenuAsync(MenuNames.User.UserMenu.ToString(), AbpSession.ToUserIdentifier())),

                LoginInformations = AbpSession.UserId.HasValue
                    ? AsyncHelper.RunSync(() => _sessionAppService.GetCurrentLoginInformations())
                    : null,

                AvatarPath = GetProfileImage(),

                IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled
            });
        }

        [ChildActionOnly]
        public PartialViewResult LanguageSelection()
        {
            return PartialView("_LanguageSelection", new LanguageSelectionViewModel
            {
                CurrentLanguage = _languageManager.CurrentLanguage,
                Languages = _languageManager.GetLanguages(),
                
            });
        }

        [ChildActionOnly]
        public PartialViewResult ModulesMenu(string activeMenu = "")
        {
            return PartialView("_ModulesMenu", new ModulesMenuViewModel
            {
                Menu = AsyncHelper.RunSync(() => _userNavigationManager
                    .GetMenuAsync(MenuNames.Modules.ModulesMenu.ToString(), AbpSession.ToUserIdentifier())),

                ActiveMenuItemName = activeMenu
            });
        }

        public string GetProfileImage()
        {
            var relativePath = ConfigurationManager.AppSettings["RELATIVEPATH_UPLOADS"].ToString();
            var avatarPath = "/user-" + AbpSession.UserId + "/avatar/";
            var defaultsAvatarPath = "/Content/images/users/profile.jpg";

            var uploadsPath = relativePath + avatarPath;
            var directoryPathServer = Server.MapPath("~" + uploadsPath);
            
            if (Directory.Exists(directoryPathServer)
                && (Directory.GetFiles(directoryPathServer, string.Format("*.{0}", "jpg")).Length > 0))
            {
                defaultsAvatarPath = uploadsPath + "profile.jpg";
            }

            return defaultsAvatarPath;
        }
    }
}