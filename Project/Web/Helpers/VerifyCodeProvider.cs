using IServices.ISysServices;
using Microsoft.AspNet.Identity;
using Models.SysModels;
using Services.SysServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Web.Helpers
{
    public class Result4CodeVerify
    {
        private Dictionary<string, string> _erroMessage = new Dictionary<string, string>();
        public bool Success { get; set; }

        public Dictionary<string, string> ErroMessage
        {
            get { return _erroMessage; }
        }

        public Result4CodeVerify()
        {
            Success = true;
        }

        public void AddModelError(string key, string message)
        {
            _erroMessage.Add(key, message);
        }
    }

    /// <summary>
    /// 邮件验证
    /// </summary>
    public static class EmailVerifyCodeProvider
    {
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="verifyCode"></param>
        /// <param name="destination"></param>
        /// <param name="codeMessage"></param>
        public static async Task<Result4CodeVerify> SendCode(string verifyCode, string destination, string codeMessage)
        {
            var result = new Result4CodeVerify();
            var verifyCodeService = DependencyResolver.Current.GetService<IVerifyCodeService>();
            var emailService = new EmailService();
            var datetimeLocal = DateTime.UtcNow.AddHours(8);
            var today = datetimeLocal.Date;
            if (verifyCodeService.GetAll().Count(a => a.Destination == destination && a.VerifyCodeUsageType == VerifyCodeUsageType.验证码 && DbFunctions.DiffDays(a.CreateUtcDateTime, datetimeLocal) == 0) >= 5)
            {
                result.Success = false;
                result.AddModelError("", "获取验证码失败，每个邮箱每天至多能获取5次");
            }
            else
            {
                try
                {
                    await
                        emailService.SendAsync(new IdentityMessage
                        {
                            Body =
                                codeMessage + "[" + verifyCode + "]，请在60分钟内完成验证",
                            Subject = "泰达贸促网验证码",
                            Destination = destination
                        });
                }
                catch (Exception e)
                {
                    result.Success = false;
                    result.AddModelError("", "邮件发送失败，邮箱服务器不可达或邮箱地址不正确");
                }
                if (result.Success)
                {
                    verifyCodeService.Add(new VerifyCode
                    {
                        AbsoluteExpirationUtcDateTime = datetimeLocal.Add(TimeSpan.FromMinutes(60)),
                        Destination = destination,
                        VerifyProvider = VerifyProvider.Email,
                        VerifyCodeUsageType = VerifyCodeUsageType.验证码,
                        Code = verifyCode
                    });
                    await verifyCodeService.CommitAsync();
                }
            }
            return result;
        }

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="verifyCode"></param>
        /// <param name="slidingExpiration"></param>
        /// <returns></returns>
        public static async Task<Result4CodeVerify> CodeVerify(string destination, string verifyCode, TimeSpan slidingExpiration)
        {
            var result = new Result4CodeVerify();
            if (verifyCode == "999999" && ConfigurationManager.AppSettings["TestingPipe"] == "On")
            {
                return result;
            }
            var verifyCodeService = DependencyResolver.Current.GetService<IVerifyCodeService>();
            var now = DateTime.UtcNow.AddHours(8);
            var item =
                verifyCodeService.GetAll().FirstOrDefault(
                    a =>
                        !a.Deleted && a.Destination == destination && a.VerifyCodeUsageType == VerifyCodeUsageType.验证码 &&
                        a.AbsoluteExpirationUtcDateTime > now && a.Code == verifyCode);
            if (item != null)
            {
                if (slidingExpiration > TimeSpan.Zero)
                {
                    item.AbsoluteExpirationUtcDateTime += slidingExpiration;
                }
                else
                {
                    item.Deleted = true;
                }
                verifyCodeService.Save(item.Id, item);
                await verifyCodeService.CommitAsync();
            }
            else
            {
                result.Success = false;
                result.AddModelError("", "验证码错误或已过期");
            }
            return result;
        }
    }
}