using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using Manager.Application.Configuration.MultiTenancy;
using Manager.Core.Common.Authorization;

namespace Manager.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Tenants)]
    public class TenantsController_NOVA : ManagerControllerBase
    {
        private readonly ITenantAppService _tenantAppService;

        public TenantsController_NOVA(ITenantAppService tenantAppService)
        {
            _tenantAppService = tenantAppService;
        }

        public ActionResult Index()
        {
            var output = _tenantAppService.GetTenants();
            return View(output);
        }
    }
}