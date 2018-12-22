using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class ListController : Controller
    {
        // GET: List
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ArticleList()
        {
            return View();
        }
        public ActionResult ArticleCont()
        {
            return View();
        }
    }
}