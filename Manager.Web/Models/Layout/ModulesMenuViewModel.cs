using Abp.Application.Navigation;

namespace Manager.Web.Models.Layout
{
    public class ModulesMenuViewModel
    {
        public UserMenu Menu { get; set; }

        public string ActiveMenuItemName { get; set; }
    }
}