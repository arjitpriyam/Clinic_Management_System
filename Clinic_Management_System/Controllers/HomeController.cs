using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Clinic_Management_System.Models;

namespace Clinic_Management_System.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Register()
        {
            MemberViewModel model = new MemberViewModel();
            model.lstRoles = CommonData.GetRoles();

            return View(model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            using (KIITEntities db = new KIITEntities())
            {
                var checkuser = (from m in db.MemberLogins
                                 join r in db.RoleDetails on m.RoleId equals r.RoleId
                                 where m.EmailId == model.Username && m.Password == model.PassWord
                                 select new { m.EmailId, r.RoleName, m.MemberId }).FirstOrDefault();

                if (checkuser != null)
                {
                    FormsAuthentication.SetAuthCookie(checkuser.EmailId, false);
                    var authTicket = new FormsAuthenticationTicket(1,  // version
                        checkuser.EmailId, //username
                        DateTime.Now,// current date and time
                        DateTime.Now.AddMinutes(1),// time out
                        false,
                        checkuser.RoleName //userdata , store a role
                        );
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(authCookie);
                    Session["Memberid"] = checkuser.MemberId;
                    switch (checkuser.RoleName)
                    {
                        case "Patient":
                            return RedirectToAction("DashBoard", "Patient");
                        case "Doctor":
                            return RedirectToAction("DashBoard", "Doctor");
                        case "Supplier":
                            return RedirectToAction("DashBoard", "Supplier");
                        default:
                            break;

                    }
                }
                else
                {
                    ViewBag.message = "Invalid Credentials";
                }

            }
            return View();
        }

        [HttpPost]
        public ActionResult Register(MemberViewModel model)
        {
            MemberViewModel model_post = new MemberViewModel();
            model_post.lstRoles = CommonData.GetRoles();
            if (model.EmailId != null && model.Password!=null && model.RoleId!=0)
            {
                KIITEntities db = new KIITEntities();
                db.InsertMember(model.EmailId, model.Password, model.RoleId);
                return View(model_post);
            }          
            else 
            {
                return View(model_post);
            }
        }
        

    }
}