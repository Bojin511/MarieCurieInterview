using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarieCurieInterview.Controllers
{
    public class HelperServiceController : Controller
    {
        //
        // GET: /HelperService/

        public ActionResult Index()
        {
            var repo = new MarieCurie.Interview.Assets.HelperServiceRepository();
            var helperServiceList = repo.Get();

            return View(helperServiceList);
        }

    }
}
