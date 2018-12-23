using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices.ISettingServices
{
    /// <summary>
    /// 配置参数设置服务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISettingService<T> where T : class
    {
        /// <summary>
        /// 将参数保存到数据库
        /// </summary>
        /// <param name="typedSetting"></param>
        /// <returns></returns>
        Task<int> SetTypedSetting(T typedSetting);

        /// <summary>
        /// 从数据库获取系统参数
        /// </summary>
        /// <param name="typedSetting"></param>
        /// <returns></returns>
        T GetTypedSetting(T typedSetting);
    }
}
