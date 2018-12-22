using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using IServices;
using IServices.Infrastructure;
using IServices.IMessage;
using IServices.ISysServices;
using Models.SysModels;
//using ifunction.JPush.V3;

namespace Services
{
    //Todo： 添加微信消息推送,消息内容【描述+链接】
    public class MessageCenterService : IMessageCenterService
    {
        //private readonly IJpushService _iJpushService;

        private readonly ISignalMessenger _iSignalMessenger;

        private readonly ISysUserService _iSysUserService;


        public MessageCenterService(
            //IJpushService iJpushService, 
            ISignalMessenger iSignalMessenger,
            ISysUserService iSysUserService)
        {
            //_iJpushService = iJpushService;

            _iSignalMessenger = iSignalMessenger;

            _iSysUserService = iSysUserService;

        }


        public void Send(string title, string message, string userId)
        {
            //Todo：可同时发送给多个用户

            //web端
            _iSignalMessenger.SendMessage(userId, "<a title=\"消息\" data-ajax=\"true\" data-ajax-mode=\"replace\" data-ajax-update=\"#Main\" href=\"/Platform/MyMessage\">您有一条新的消息：" + title + "</a>");

        }

        public void Send(string title, string message)
        {
            //web端
            _iSignalMessenger.SendAll("<a title=\"消息\" data-ajax=\"true\" data-ajax-mode=\"replace\" data-ajax-update=\"#Main\" href=\"/Platform/MyMessage\">您有一条新的消息：" + title + "</a>");
        }

        public void Send(SysBroadcast sysBroadcast)
        {
            //web消息
            _iSignalMessenger.SendMessage(sysBroadcast.AddresseeId, "<a title=\"消息\" data-ajax=\"true\" data-ajax-mode=\"replace\" data-ajax-update=\"#Main\" href=\"/Platform/MyMessage\">您有一条新的消息：" + sysBroadcast.Title + "</a>");

        }

    }

}
