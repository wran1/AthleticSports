using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Services.SysServices
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // 在此处插入电子邮件服务可发送电子邮件。

            var email = new MailMessage()
            {
                To = { message.Destination },
                IsBodyHtml = true,
                Body = message.Body,
                Subject = message.Subject
            };
            //实例化smtp客服端对象，用来发送电子邮件    
            var stmp = new SmtpClient();

            //发送邮件    
            stmp.Send(email);
            // stmp.SendMailAsync(email);

            return Task.FromResult(0);
        }
    }
}
