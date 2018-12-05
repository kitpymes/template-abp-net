using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using Manager.Core.Common.Authorization;
using Abp.Application.Navigation;
using Abp.Threading;
using System.Collections.Generic;
using Manager.Core.Configuration.Roles;
using System.Collections.Specialized;
using System.Web;
using System;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Abp.Runtime.Session;
using Manager.Web.Models.Shared;
using Manager.Core.Configuration.Users;
using System.Configuration;
using Manager.Web.Models.Upload;
using Abp.Localization;
using Abp.UI;
using Manager.Core.Configuration.MultiTenancy;
using Manager.Web.Models;
using Manager.Application.Configuration.Profile;
using Manager.Application.Configuration.Profile.Dto;
using Abp.AutoMapper;
using Manager.Application.Configuration.MultiTenancy;
using Manager.Application.Configuration.Roles;
using Abp.Extensions;
using Manager.Application.Configuration.Roles.Dto;
using Abp.Web.Models;
using Manager.Web.Models.Configuration.Profile;
using Manager.Web.Models.Configuration.Roles;

namespace Manager.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Configuration)]
    public class ConfigurationController : ManagerControllerBase
    {
        private readonly IProfileAppService _profileAppService;
        private readonly IUserNavigationManager _userNavigationManager;
        private readonly IRoleAppService _roleAppService;
        private readonly ITenantAppService _tenantAppService;
        private readonly UserManager _userManager;
        private readonly LanguageManager _languageManager;

        public ConfigurationController(
            IProfileAppService profileAppService, 
            IUserNavigationManager userNavigationManager, 
            IRoleAppService roleAppService,
            ITenantAppService tenantAppService,
            UserManager userManager,
            LanguageManager languageManager)
        {
            _profileAppService = profileAppService;
            _userNavigationManager = userNavigationManager;
            _roleAppService = roleAppService;
            _tenantAppService = tenantAppService;
            _userManager = userManager;
            _languageManager = languageManager;
        }

        public PartialViewResult _Index(string activeMenu = "")
        {
            return PartialView(new TabsMenuViewModel
            {
                Menu = AsyncHelper.RunSync(() => _userNavigationManager
                    .GetMenuAsync(MenuNames.Configuration.ConfigurationPageMenu.ToString(), AbpSession.ToUserIdentifier())),

                ActiveMenuItemName = string.IsNullOrEmpty(activeMenu)
                ? MenuNames.Configuration.Profile.ToString()
                : activeMenu
            });
        }

        #region Profile

        public PartialViewResult _Profile()
        {
            try
            {
                User user = AsyncHelper.RunSync(() =>
               _userManager.GetUserByIdAsync(AbpSession.UserId.Value));

                return PartialView(new ProfileViewModel
                {
                    EmailAddress = user.EmailAddress,
                    Name = user.Name,
                    Password = "",
                    PasswordRepeat = "",
                    Surname = user.Surname,
                    Avatar = new UploadConfigViewModel
                    {
                        UserId = AbpSession.UserId.Value,
                        StaticName = "profile",
                        StaticExtension = "jpg",
                        Languaje = _languageManager.CurrentLanguage.Name,
                        HasInitialPreviewAsData = true,
                        FileExtensions = new List<string> { "jpg", "gif", "png", "jpeg" },
                        MaxFileSize = 101400,
                        UploadsPath = ConfigurationManager.AppSettings["RELATIVEPATH_UPLOADS"].ToString() + "/user-" + AbpSession.UserId + "/avatar/",
                        DefaultPreviewContent = "<img class='img-responsive' src='/Content/images/users/profile.jpg' alt='Avatar'>",
                        MaxFileCount = 1
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.Error(typeof(ConfigurationController).ToString(), ex);
                throw new UserFriendlyException(L(ValidationHelper.Msg.ErrorMessageUserDefault));
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateProfile(ProfileViewModel model)
        {
            try
            {
                CheckResultState(await _profileAppService
                    .UpdateProfileAsync(model.MapTo<UpdateProfileDtoInput>()));

                return Json(new 
                {
                    Message = L("COMMON.MSG.ChangesSuccessfullySaved")
                });
            }
            catch (UserFriendlyException ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(typeof(ConfigurationController).ToString(), ex);
                throw new UserFriendlyException(L(ValidationHelper.Msg.ErrorMessageUserDefault));
            }
        }

        #endregion

        #region Billing

        public PartialViewResult _Billing()
        {
            return PartialView();
        }

        #endregion

        #region Companies

        public PartialViewResult _Company()
        {
            return PartialView();
        }

        public JsonResult GetAllCompanies()
        {
            try
            {
                Tenant filter = GetFilterTenant();

                var output = _tenantAppService.GetTenants().Items.Where(c =>
                    (String.IsNullOrEmpty(filter.Name) || c.Name.ToLower().Contains(filter.Name.ToLower()))
                    && (String.IsNullOrEmpty(filter.TenancyName) || c.TenancyName.ToLower().Contains(filter.TenancyName.ToLower()))
                    && (!filter.IsActive || c.IsActive == filter.IsActive)
                    && (!filter.EditionId.HasValue || c.EditionId == filter.EditionId)
                    && (!filter.IsDeleted || c.IsDeleted == filter.IsDeleted)
                );

                return Json(new 
                {
                    Success = true,
                    Error = false,
                    Msg = "",
                    Result = output.ToArray()
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(typeof(ConfigurationController)).Error(ex);
                return Json(new 
                {
                    Success = false,
                    Error = true,
                    Msg = "Ocurrio un error inesperado, por favor intente mas tarde.",
                    Result = new List<Tenant>().ToArray()
                }, JsonRequestBehavior.AllowGet);
            }
        }

        private Tenant GetFilterTenant()
        {
            NameValueCollection filter = HttpUtility.ParseQueryString(Request.QueryString.ToString());

            return new Tenant
            {
                Name = filter["Name"],
                TenancyName = filter["TenancyName"],
                EditionId = filter["EditionId"] == "0" ? new int?() : int.Parse(filter["EditionId"]),
                IsActive = String.IsNullOrEmpty(filter["IsActive"]) ? false : bool.Parse(filter["IsActive"]),
                IsDeleted = String.IsNullOrEmpty(filter["IsDeleted"]) ? false : bool.Parse(filter["IsDeleted"])
                //Country = (filter["Country"] == "0") ? (Country?)null : (Country)int.Parse(filter["Country"]),
            };
        }

        #endregion

        #region Users

        //[AbpMvcAuthorize(PermissionNames.Pages_Configuration_Users)]
        public PartialViewResult _Users()
        {
            return PartialView();
        }

        #endregion

        #region Roles

        [AbpMvcAuthorize(PermissionNames.Pages_Configuration_Roles)]
        public PartialViewResult _Roles()
        {
            ViewBag.CurrentLanguageName = _languageManager.CurrentLanguage.Name;
            return PartialView();
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Configuration_Roles)]
        public async Task<JsonResult> GetAllRoles()
        {
            try
            {
                

                var IsMaster = User.IsInRole(StaticRoleNames.Host.Master.ToString());

                var result = await _roleAppService.GetRolesAsync(IsMaster);

                CheckResultState(result.Item1);

                RoleViewModel filter = GetFilterRole();

                var output = result.Item2.Items.Where(c =>
                    (filter.Name.IsNullOrWhiteSpace() || c.Name.ToLower().Contains(filter.Name.ToLower()))
                    && (filter.DisplayName.IsNullOrWhiteSpace() || c.DisplayName.ToLower().Contains(filter.DisplayName.ToLower()))
                    && (!filter.IsStatic || c.IsStatic == filter.IsStatic)
                    && (!filter.IsDefault || c.IsDefault == filter.IsDefault)
                    && (!filter.IsDeleted || c.IsDeleted == filter.IsDeleted) 
                    //(String.IsNullOrEmpty(filter.Address) || c.Address.Contains(filter.Address)) &&
                    //(!filter.Married.HasValue || c.Married == filter.Married) &&
                    //(!filter.Country.HasValue || c.Country == filter.Country)
                );

                return Json(new AjaxResponse
                {
                    Result = output.ToArray()
                }, JsonRequestBehavior.AllowGet);
            }
            catch (UserFriendlyException ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(typeof(ConfigurationController).ToString(), ex);
                throw new UserFriendlyException(L(ValidationHelper.Msg.ErrorMessageUserDefault));
            }
        }
        
        [AbpMvcAuthorize(PermissionNames.Pages_Configuration_Roles_Create)]
        public async Task<JsonResult> CreateRole(CreateRoleViewModel model)
        {
            try
            {
                CheckResultState(await _roleAppService.CreateRoleAsync(
                    model.MapTo<CreateRoleDtoInput>()));

                return Json(new
                {
                    Message = L("COMMON.MSG.ChangesSuccessfullySaved")
                });
            }
            catch (UserFriendlyException ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(typeof(ConfigurationController).ToString(), ex);
                throw new UserFriendlyException(L(ValidationHelper.Msg.ErrorMessageUserDefault));
            }
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Configuration_Roles_Edit)]
        public async Task<JsonResult> UpdateRole(UpdateRoleViewModel model)
        {
            try
            {
                CheckResultState(await _roleAppService.UpdateRoleAsync(
                    model.MapTo<UpdateRoleDtoInput>()));

                return Json(new
                {
                    Message = L("COMMON.MSG.ChangesSuccessfullySaved")
                });
            }
            catch (UserFriendlyException ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(typeof(ConfigurationController).ToString(), ex);
                throw new UserFriendlyException(L(ValidationHelper.Msg.ErrorMessageUserDefault));
            }
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Configuration_Roles_Delete)]
        public async Task<JsonResult> DeleteRole(Role model)
        {
            try
            {
                CheckResultState(await _roleAppService.DeleteRoleAsync(model.Id));

                return Json(new
                {
                    Message = L("COMMON.MSG.ChangesSuccessfullySaved")
                });
            }
            catch (UserFriendlyException ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(typeof(ConfigurationController).ToString(), ex);
                throw new UserFriendlyException(L(ValidationHelper.Msg.ErrorMessageUserDefault));
            }
        }

        private RoleViewModel GetFilterRole()
        {
            NameValueCollection filter = HttpUtility.ParseQueryString(Request.QueryString.ToString());

            return new RoleViewModel
            {
                Name = filter["Name"],
                DisplayName = filter["DisplayName"],
                IsStatic = String.IsNullOrEmpty(filter["IsStatic"]) ? false : bool.Parse(filter["IsStatic"]),
                IsDefault = String.IsNullOrEmpty(filter["IsDefault"]) ? false : bool.Parse(filter["IsDefault"]),
                IsDeleted = String.IsNullOrEmpty(filter["IsDeleted"]) ? false : bool.Parse(filter["IsDeleted"])
                //Country = (filter["Country"] == "0") ? (Country?)null : (Country)int.Parse(filter["Country"]),
            };
        }

        #endregion

        #region Logs

        public PartialViewResult _AuditsLog()
        {
            return PartialView();
        }

        #endregion
    }
}