using Abp.Application.Navigation;
using Manager.Application.Common.Sessions.Dto;

namespace Manager.Web.Models.Layout
{
    public class UserMenuViewModel
    {
        public UserMenu Menu { get; set; }

        public string AvatarPath { get; set; }

        public GetCurrentLoginInformationsOutput LoginInformations { get; set; }
        public bool IsMultiTenancyEnabled { get; set; }

        public string GetShownLoginName()
        {
            var userName = "<span id=\"HeaderCurrentUserName\">" + LoginInformations.User.UserName + "</span>";

            if (!IsMultiTenancyEnabled)
            {
                return userName;
            }

            return LoginInformations.Tenant == null
                ? userName
                : LoginInformations.Tenant.TenancyName + "\\" + userName;
        }
    }
}