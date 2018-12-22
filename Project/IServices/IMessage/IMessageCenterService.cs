using Models.SysModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices.IMessage
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISignalMessenger
    {
        /// <summary>
        /// 给某个用户弹出右下角消息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="message"></param>
        void SendMessage(string userId, string message);
        /// <summary>
        /// 给全体用户弹出右下角消息
        /// </summary>
        /// <param name="message"></param>
        void SendAll(string message);

        /// <summary>
        /// 发送给指定用户
        /// </summary>
        /// <param name="message"></param>
        Task<int> SendSysBroadcast(SysBroadcast message);

        /// <summary>
        /// 发送有模块权限的人
        /// </summary>
        /// <param name="area"></param>
        /// <param name="message"></param>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        Task<int> SendSysBroadcastByAction(string action, string controller, string area, SysBroadcast message);
    }
    public interface IMessageCenterService
    {
        void Send(string title, string message, string userId);
        void Send(string title, string message);
        void Send(SysBroadcast sysBroadcast);
    }
}
