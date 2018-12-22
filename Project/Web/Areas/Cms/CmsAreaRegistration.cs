using System.Web.Mvc;

namespace Web.Areas.Cms
{
    public class CmsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Cms";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Cms_default",
                "Cms/{controller}/{action}/{id}",
                new { Controller = "Index", action = "Index", id = UrlParameter.Optional },
                new[] { "Web.Areas.Cms.Controllers" }
            );
        }
    }
}