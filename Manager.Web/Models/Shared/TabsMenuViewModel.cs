using Abp.Application.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manager.Web.Models.Shared
{
    public class TabsMenuViewModel
    {
        public UserMenu Menu { get; set; }

        public string ActiveMenuItemName { get; set; }
    }
}