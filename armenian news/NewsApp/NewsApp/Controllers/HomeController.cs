using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsApp.Models;
using PagedList;

namespace NewsApp.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index(int? page)
        {
            return RedirectToAction("Popular", "News", null);
        }

        
    }
}
