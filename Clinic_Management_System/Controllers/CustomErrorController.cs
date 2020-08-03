using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clinic_Management_System.Controllers
{
    public class CustomErrorController : Controller
    {
        // GET: CustomError
        // GET: CustomError
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PageNotFound()
        {
            return View();
        }
        public ActionResult Unauthorized()
        {
            return View();
        }
    }
}