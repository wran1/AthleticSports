using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using IServices.ISettingServices;
using IServices.ISysServices;
using Models.SysModels;

namespace Web.App_Start
{
    /// <summary>
    /// 加载全站系统参数配置
    /// </summary>
    public static class SettingsLoader
    {
        /// <summary>
        /// 获取账户系统参数
        /// </summary>
        /// <returns></returns>
        public static AccountConfiguration GetAccountConfiguration()
        {
            var accountSettingServics = DependencyResolver.Current.GetService<IAccountSettingServics>();
            var cachedSettings = HttpContext.Current.Cache.Get("AccountConfiguration") as AccountConfiguration;
            if (cachedSettings == null)
            {
                cachedSettings = new AccountConfiguration();
                //加载数据库存储的设置
                cachedSettings = accountSettingServics.GetTypedSetting(cachedSettings); 
                HttpContext.Current.Cache.Add("AccountConfiguration", cachedSettings, null, Cache.NoAbsoluteExpiration, TimeSpan.FromDays(1), CacheItemPriority.Normal, null);
            }
            return cachedSettings;

        }
        /// <summary>
        /// 设置账户系统参数
        /// </summary>
        /// <param name="accountConfiguration"></param>
        /// <returns></returns>
        public static async Task<int> SetAccountConfiguration(AccountConfiguration accountConfiguration)
        {
            var accountSettingServics = DependencyResolver.Current.GetService<IAccountSettingServics>();
            var result = await accountSettingServics.SetTypedSetting(accountConfiguration);
            if (result > 0) { 
                var cachedSettings = HttpContext.Current.Cache.Get("AccountConfiguration") as AccountConfiguration;
                if (cachedSettings == null)
                {
                    HttpContext.Current.Cache.Add("AccountConfiguration", cachedSettings, null, Cache.NoAbsoluteExpiration, TimeSpan.FromDays(1), CacheItemPriority.Normal, null);
                }
                else
                {
                    HttpContext.Current.Cache["AccountConfiguration"] = accountConfiguration;
                }
            }
            return result;

        }



    }


}