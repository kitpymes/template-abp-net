using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.UI;
using Manager.Application.Account.ForgotPassword.Dto;
using Manager.Core.Common.Mail;
using Manager.Core.Configuration.Users;
using Microsoft.AspNet.Identity;
using System;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Manager.Core.Configuration.MultiTenancy;

namespace Manager.Application.Account.ForgotPassword
{
    public class ForgotPasswordAppService : ManagerAppServiceBase, IForgotPasswordAppService
    {
        private readonly IEmailManager _emailManager;

        public ForgotPasswordAppService(IPermissionManager permissionManager, IEmailManager emailManager)
        {
             _emailManager = emailManager;
        }

        /// <summary>
        /// Genera un código que se envia al email del usuario para establecer la contraeña.
        /// </summary>
        /// <param name="emailAddress">Email del usuario.</param>
        public async Task<ResultType> ForgotPasswordLinkAsync(ForgotPasswordLinkDtoInput input)
        {
            try
            {
                User user = null;

                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete, AbpDataFilters.MayHaveTenant))
                {
                    user = await UserManager.FindByEmailAsync(input.EmailAddress);
                }

                if (user == null)
                    return ResultType.EmailAddressNotFindInSystem;

                if (!user.IsEmailConfirmed)
                    return ResultType.WaitingForActivationEmail;

                if (!user.IsActive)
                    return ResultType.WaitingForActivationMessage;

                user.SetNewPasswordResetCode();

                await SendEmailTokenLinkPasswordReset(user);

                return ResultType.Success;
            }
            catch (Exception ex)
            {
                Logger.Error(typeof(ForgotPasswordAppService).ToString(), ex);
                return ResultType.Error;
            }
        }

        /// <summary>
        /// Se cambia la contraseña del usuario.
        /// </summary>
        /// <param name="input">Datos del usuario.</param>
        /// <returns>El usuario.</returns>
        public async Task<Tuple<ResultType, ForgotPasswordDtoOutput>> ForgotPasswordAsync(ForgotPasswordDtoInput input)
        {
            try
            {
                User user = null;

                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete, AbpDataFilters.MayHaveTenant))
                {
                    user = await UserManager.FindByIdAsync(input.UserId);
                }

                if (user == null)
                    return Tuple.Create(ResultType.EmailAddressNotFindInSystem, new ForgotPasswordDtoOutput());

                if (user.LastModificationTime.HasValue && DateTime.Now > user.LastModificationTime.Value.AddHours(1))
                    return Tuple.Create(ResultType.ResetCodeExpired, new ForgotPasswordDtoOutput());

                if (user.PasswordResetCode != input.PasswordResetCode)
                    return Tuple.Create(ResultType.InvalidPasswordResetCode, new ForgotPasswordDtoOutput());

                user.Password = new PasswordHasher().HashPassword(input.Password);

                user.PasswordResetCode = null;

                Tenant tenant = user.TenantId.HasValue 
                    ? await TenantManager.FindByIdAsync(user.TenantId.Value) : null;

                return Tuple.Create(ResultType.Success, 
                    new ForgotPasswordDtoOutput
                    {
                        Dto = user.MapTo<ForgotPasswordDto>(),
                        TenantName = tenant?.Name
                    });
            }
            catch (Exception ex)
            {
                Logger.Error(typeof(ForgotPasswordAppService).ToString(), ex);
                return Tuple.Create(ResultType.Error, new ForgotPasswordDtoOutput());
            }
        }

        #region Private Methods

        /// <summary>
        /// Envia un email para establecer una nueva contraseña.
        /// </summary>
        /// <param name="user">Datos del usuario.</param>
        /// <returns></returns>
        private async Task SendEmailTokenLinkPasswordReset(User user)
        {
            var mailBuilder = new StringBuilder();

            mailBuilder.Append(
                @"<!DOCTYPE html>
                <html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">
                <head>
                    <meta charset=""utf-8"" />
                    <title>{TEXT_HTML_TITLE}</title>
                    <style>
                        body {
                            font-family: Verdana, Geneva, 'DejaVu Sans', sans-serif;
                            font-size: 12px;
                        }
                    </style>
                </head>
                <body>

                    <table style=""width:100%;height:100%;background-color:#eeeeee"">
	                    <tr>
		                    <td bgcolor=""#EEEEEE"">
			                    <table width=""600"" border=""0"" cellspacing=""0"" cellpadding=""0"" align=""center"">
				                    <tr>
					                    <td height=""10""></td>
				                    </tr>
				                    <tr>
					                    <td>
						                    <table width=""600"" height=""100"" border=""0"" cellspacing=""0"" cellpadding=""0"">
							                    <tr>
								                    <td bgcolor=""white""  align=""center"" valign=""middle"" style=""font-family:Arial,Helvetica,sans-serif;font-size:15px;"">
									                    <a href=""{SERVER}"" title=""Mondo Empleos"" target=""_blank"">
										                    <img style=""display:block;text-align:center"" src=""http://mondoempleos.com/Content/img/logo.png"" alt=""Mondo Empleos"" width=""75"" height=""45"" border=""0"">
									                    </a>
								                    </td>
								                    <td bgcolor=""#303a3c"" align=""center"" valign=""middle"" 
									                    style=""font-family:Arial,Helvetica,sans-serif;color:#fff;font-size:18px;height:60px;"">
									                    <strong>{TEXT_WELCOME}</strong>
								                    </td>
							                    </tr>
							                    <tr>
								                    <td height=""10""></td>
							                    </tr>
						                    </table>
					                    </td>
				                    </tr>
				                    <tr>
					                    <td bgcolor=""#FFFFFF"" height=""10""></td>
				                    </tr>
				                    <tr>
					                    <td bgcolor=""#FFFFFF"" align=""center"" valign=""top"" style=""font-family:Arial,Helvetica,sans-serif"">
						                    <table width=""600"" border=""0"" cellspacing=""0"" cellpadding=""0"">
							                    <tr>
								                    <td width=""30"">&nbsp;</td>
								                    <td align=""left"" valign=""top"" style=""font-family:Arial,Helvetica,sans-serif;color:#555555;font-size:14px"">
									                    <br/>
									                    <span style=""color:#22aae4;font-size:23px"">{TEXT_DESCRIPTION}</span>
									                    <br/><br/>
									                    <span style=""font-size:14px;font-family:Arial,Helvetica,sans-serif;color:#666666;line-height:1.4"">
										                    <p><a href=""{SERVER}/Account/ForgotPassword?userId={USER_ID}&resetCode={RESET_CODE}"">{SERVER}/Account/ForgotPassword?userId={USER_ID}&amp;resetCode={RESET_CODE}</a></p>
										                    <p>&nbsp;</p>
										                    <p><a href=""{SERVER}"">{SERVER}</a></p> 
									                    </span>
									                    <br/>
									                    <br/>
		
								                    </td>
								                    <td width=""30"">&nbsp;</td>
							                    </tr>
							                    <tr>
                                                    <td height=""10"" bgcolor=""#ffffff""></td>
										        </tr>
						                    </table>
					                    </td>
				                    </tr>
			                    </table>
		                    </td>
	                    </tr>
	                    <tr>
		                    <td height=""40"">&nbsp;</td>
                        </tr>
                    </table>
                </body>
                </html>");

            mailBuilder.Replace("{SERVER}", ConfigurationManager.AppSettings["SERVER"].ToString());
            mailBuilder.Replace("{TEXT_HTML_TITLE}", L("Mail.ForgotPasword.HtmlTitle"));
            mailBuilder.Replace("{TEXT_WELCOME}", L("Mail.ForgotPasword.BodyTitle"));
            mailBuilder.Replace("{TEXT_DESCRIPTION}", L("Mail.ForgotPasword.BodyDescription"));
            mailBuilder.Replace("{USER_ID}", user.Id.ToString());
            mailBuilder.Replace("{RESET_CODE}", user.PasswordResetCode);

            var mail = new MailMessage();
            mail.To.Add(user.EmailAddress);
            mail.IsBodyHtml = true;
            mail.Subject = L("Mail.ForgotPasword.Subject");
            mail.SubjectEncoding = Encoding.UTF8;
            mail.Body = mailBuilder.ToString();
            mail.BodyEncoding = Encoding.UTF8;

            await _emailManager.SendEmailAsync(mail);
        }

        #endregion
    }
}
