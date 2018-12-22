using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class CapitalController : Controller
    {
        // GET: Capital
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult InvestmentForm()
        {
            return View();
        }
        public ActionResult FinancingForm()
        {
            return View();
        }
        public ActionResult StockForm()
        {
            return View();
        }
    }
}