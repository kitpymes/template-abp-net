using System.Collections.Generic;
using Abp.Localization;
using Abp.Application.Navigation;

namespace Manager.Web.Models.Layout
{
    public class LanguageSelectionViewModel
    {
        public LanguageInfo CurrentLanguage { get; set; }

        public IReadOnlyList<LanguageInfo> Languages { get; set; }
    }
}