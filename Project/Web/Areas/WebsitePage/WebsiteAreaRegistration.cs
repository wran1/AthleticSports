using System.Web.Mvc;

namespace Web.Areas.WebsitePage
{
    public class WebsitePageAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "WebsitePage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "WebsitePage",
                "WebsitePage/{controller}/{action}/{id}",
                new { Controller = "Index", action = "Index", id = UrlParameter.Optional },
                new[] { "Web.Areas.WebsitePage.Controllers" }
            );
        }
    }
}