using Abp.Authorization;
using Abp.Web.Mvc.Authorization;
using Manager.Core.Common.Authorization;
using Manager.Web.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Runtime.Session;
using Abp.Threading;
using Abp.Application.Navigation;
using Manager.Web.Controllers;

//namespace Manager.Web.Controllers
namespace Manager.Web.Areas.Customers.Controllers
{
    //[AbpMvcAuthorize(PermissionNames.Pages_Customers)]
    public class IndexController : ManagerControllerBase
    {
        private readonly IUserNavigationManager _userNavigationManager;
        public IndexController(IUserNavigationManager userNavigationManager)
        {
            _userNavigationManager = userNavigationManager;
        }

        // GET: Customers
        public PartialViewResult _Index(string activeMenu = "")
        {
            return PartialView("~/Areas/Customers/Views/_Index.cshtml", new TabsMenuViewModel
            {
                Menu = AsyncHelper.RunSync(() => _userNavigationManager
                    .GetMenuAsync(MenuNames.Customers.CustomersPageMenu.ToString(), AbpSession.ToUserIdentifier())),

                ActiveMenuItemName = string.IsNullOrEmpty(activeMenu)
                ? MenuNames.Customers.List.ToString()
                : activeMenu
            });
        }

        public PartialViewResult _AddressType()
        {
            return PartialView("~/Areas/Customers/Views/_AddressType.cshtml");
        }

        public PartialViewResult _EmailType()
        {
            return PartialView("~/Areas/Customers/Views/_EmailType.cshtml");
        }

        public PartialViewResult _TelephoneType()
        {
            return PartialView("~/Areas/Customers/Views/_TelephoneType.cshtml");
        }

        //[AbpMvcAuthorize(PermissionNames.Pages_Customers_Create)]
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //[AbpMvcAuthorize(PermissionNames.Pages_Customers_Edit)]
        //public ActionResult Edit()
        //{
        //    return View();
        //}

        //[AbpMvcAuthorize(PermissionNames.Pages_Customers_Delete)]
        //public ActionResult Delete()
        //{
        //    return View();
        //}
    }
}