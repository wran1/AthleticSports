using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Services.SysServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    /// <summary>
    /// 调查征集
    /// </summary>
    public class ResearchController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Details()
        {
            return View();
        }
        public ActionResult Details_a()
        {
            return View();
        }
        public ActionResult Details_b()
        {
            return View();
        }
        public ActionResult Details_c()
        {
            return View();
        }
        public ActionResult Details_d()
        {
            return View();
        }
        public ActionResult Details_e()
        {
            return View();
        }
        public ActionResult Details_f()
        {
            return View();
        }
        public ActionResult Details_g()
        {
            return View();
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="researchhtml"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> SendResearchEmail(string researchhtml, string title)
        {
            var emailService = new EmailService();
            try
            {
                await
                    emailService.SendAsync(new IdentityMessage
                    {
                        Body = researchhtml,
                        Subject = title,
                        Destination = "tradeteda@foxmail.com",
                    });
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(true);
        }
    }
}