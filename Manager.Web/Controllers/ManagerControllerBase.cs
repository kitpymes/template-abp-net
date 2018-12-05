using Abp.IdentityFramework;
using Abp.UI;
using Abp.Web.Mvc.Controllers;
using Manager.Application;
using Manager.Core;
using Manager.Web.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;

namespace Manager.Web.Controllers
{
    /// <summary>
    /// Derive all Controllers from this class.
    /// </summary>
    public abstract class ManagerControllerBase : AbpController
    {
        protected ManagerControllerBase()
        {
            LocalizationSourceName = ManagerConsts.LocalizationSourceName;
        }

        ////protected virtual void CheckModelState()
        ////{
        ////    if (!ModelState.IsValid)
        ////    {
        ////        var modelErrors = new List<string>();
        ////        foreach (var modelState in ModelState.Values)
        ////        {
        ////            foreach (var modelError in modelState.Errors)
        ////            {
        ////                modelErrors.Add(modelError.ErrorMessage);
        ////            }
        ////        }

        ////        throw new UserFriendlyException(string.Join(" ", modelErrors));
        ////    }
        ////}

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        protected virtual void CheckResultState(ResultType resultType)
        {
            switch (resultType)
            {
                case ResultType.Success:
                    //"Don't call this method with a success result!"
                    break;
                case ResultType.Error:
                    throw new UserFriendlyException(L(ValidationHelper.Msg.ErrorMessageUserDefault));
                case ResultType.EmailAddressNotFindInSystem:
                    throw new UserFriendlyException(L("ACCOUNT.EmailAddressNotFindInSystem"));
                case ResultType.WaitingForActivationEmail:
                    throw new UserFriendlyException(L("ACCOUNT.WaitingForActivationEmail"));
                case ResultType.WaitingForActivationMessage:
                    throw new UserFriendlyException(L("ACCOUNT.WaitingForActivationMessage"));
                case ResultType.ResetCodeExpired:
                    throw new UserFriendlyException(L("ACCOUNT.ResetCodeExpired"));
                case ResultType.InvalidPasswordResetCode:
                    throw new UserFriendlyException(L("ACCOUNT.InvalidPasswordResetCode"));
                default:
                    Logger.Warn($"Unhandled 'CheckResultState' fail reason: {resultType}");
                    break;
            }

        }
    }
}