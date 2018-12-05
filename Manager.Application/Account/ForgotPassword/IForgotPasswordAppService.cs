using Abp.Application.Services;
using Manager.Application.Account.ForgotPassword.Dto;
using System;
using System.Threading.Tasks;

namespace Manager.Application.Account.ForgotPassword
{
    public interface IForgotPasswordAppService : IApplicationService
    {
        Task<ResultType> ForgotPasswordLinkAsync(ForgotPasswordLinkDtoInput input);

        Task<Tuple<ResultType, ForgotPasswordDtoOutput>> ForgotPasswordAsync(ForgotPasswordDtoInput input);
    }
}
