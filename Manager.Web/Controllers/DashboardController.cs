using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using Manager.Web.Models.Shared;
using Abp.Threading;
using Abp.Application.Navigation;
using Abp.Runtime.Session;

namespace Manager.Web.Controllers
{
    [AbpMvcAuthorize]
    public class DashboardController : ManagerControllerBase
    {
        private readonly IUserNavigationManager _userNavigationManager;
        public DashboardController(IUserNavigationManager userNavigationManager)
        {
            _userNavigationManager = userNavigationManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult _Index(string activeMenu = "")
        {
            return PartialView(new TabsMenuViewModel
            {
                Menu = AsyncHelper.RunSync(() => _userNavigationManager
                    .GetMenuAsync(MenuNames.Dashboard.DashboardPageMenu.ToString(), AbpSession.ToUserIdentifier())),

                ActiveMenuItemName = string.IsNullOrEmpty(activeMenu)
                ? MenuNames.Dashboard.UserInfo.ToString()
                : activeMenu
            });
        }

        public PartialViewResult _UserInfo()
        {
            return PartialView();
        }

        public PartialViewResult _BrowserInfo()
        {
            return PartialView();
        }
    }
}