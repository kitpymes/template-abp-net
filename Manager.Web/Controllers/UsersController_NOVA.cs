using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using Manager.Core.Common.Authorization;
using Manager.Application.Configuration.Users;

namespace Manager.Web.Controllers
{
    //[AbpMvcAuthorize(PermissionNames.Pages_Users)]
    public class UsersController_NOVA : ManagerControllerBase
    {
        private readonly IUserAppService _userAppService;

        public UsersController_NOVA(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        public async Task<ActionResult> Index()
        {
            var output = await _userAppService.GetUsers();
            return View(output);
        }
    }
}