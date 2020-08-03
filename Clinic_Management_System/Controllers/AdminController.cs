using Clinic_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Clinic_Management_System.Controllers
{

    //[CustomAuthorize(Roles = "Admin")]

    public class AdminController : Controller
    {
        public ActionResult Login()
        {
            return View();
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
                
                if (checkuser != null && checkuser.RoleName=="Admin")
                {
                    FormsAuthentication.SetAuthCookie(checkuser.EmailId, false);
                    var authTicket = new FormsAuthenticationTicket(2,  // version
                        checkuser.EmailId, //username
                        DateTime.Now,// current date and time
                        DateTime.Now.AddMinutes(10),// time out
                        false,
                        "Admin" //userdata , store a role
                        );
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(authCookie);
                    Session["Memberid"] = checkuser.MemberId;
                    return RedirectToAction("DashBoard", "Admin");
                }
                else
                {
                    ViewBag.message = "Invalid Credentials";
                }
            }
            return View();
        }
        // GET: Admin
        public ActionResult DashBoard()
        {
            using (KIITEntities db = new KIITEntities())
            {
                DashView model = new DashView();
                model.Patn = db.Patients.Count();
                model.Docs = db.Doctors.Count();
                model.Sups = db.Suppliers.Count();
                model.TotOrd = db.PatientOrderDetails.Count();
                model.DelOrd = db.PatientOrderDetails.Where(a => a.OrderStatus == "Delivered").Count();
                model.DisOrd = db.PatientOrderDetails.Where(a => a.OrderStatus == "Dispatched").Count();
                model.ReqOrd = db.PatientOrderDetails.Where(a => a.OrderStatus == "Requested").Count();
                model.AssOrd = db.DrugDeliveries.Count();
                model.NAssOrd = db.PatientOrderDetails.Count()-(db.DrugDeliveries.Count()) ;
                return View(model);
            }
        }

        public ActionResult EditProfile()
        {
            using (KIITEntities db = new KIITEntities())
            {
                int memid = Convert.ToInt32(Session["Memberid"]);
                ProfileViewModel model = new ProfileViewModel();
                var getdata = db.Admins.SingleOrDefault(a => a.MemberId == memid);

                if (getdata != null)
                {
                    model.FirstName = getdata.FirstName;
                    model.LastName = getdata.LastName;
                    model.Gender = getdata.Gender;
                    model.Address = getdata.Address;
                    return View(model);
                }
                else
                {
                    return View();
                }
            }
        }
        public ActionResult Change_Password()
        {
            return View();
        }
        public ActionResult Remove_Drugs()
        {
            int memid = Convert.ToInt32(Session["Memberid"]);

            using (KIITEntities db = new KIITEntities())
            {
                var data = db.Drugs;
                List<DrugModel> lst = new List<DrugModel>();
                foreach (var item in data)
                {
                    lst.Add(new DrugModel
                    {
                        DrugId = item.DrugId,
                        DrugName = item.DrugName,
                        Man_Date = item.ManufactureDate,
                        Exp_Date = item.ExpiredDate,
                        UsedFor = item.UsedFor,
                        SideEffects = item.SideEffects,
                        Quantity = item.TotalQuantityAvailable,
                    });
                }
                DrugModel model = new DrugModel();
                model.lstDrugs = lst;
                return View(model);
            }
        }
        public ActionResult View_Order_Details()
        {
            string sname;
            int memid = Convert.ToInt32(Session["Memberid"]);

            using (KIITEntities db = new KIITEntities())
            {
                var data = db.PatientOrderDetails;
                List<DrugViewModel> lst = new List<DrugViewModel>();
                foreach (var item in data)
                {
                    string drug_Name = db.Drugs.Where(a => a.DrugId == item.DrugId).Select(a => a.DrugName).SingleOrDefault();
                    int sid = db.DrugDeliveries.Where(a => a.OrderId == item.OrderId).Select(a => a.SupplierId).SingleOrDefault();
                    if (sid > 0)
                    { sname = db.Suppliers.Where(a => a.SupplierId == sid).Select(a => a.FirstName).SingleOrDefault(); }
                    else
                        sname = null;

                    lst.Add(new DrugViewModel
                    {
                        OrderId = item.OrderId,
                        DrugId=item.DrugId,
                        OrderNumber = item.OrderNumber,
                        DrugName = drug_Name,
                        Quantity = item.Quantity,
                        OrderDate = item.OrderedDate,
                        SupplierName = sname
                    }) ;
                }

                DrugViewModel model = new DrugViewModel();
                model.lstShowDrugs = lst;
                model.lstSupplier = CommonData.GetSuppliers();
                return View(model);
            }
        }
   
        public ActionResult JS_Drugs()
        {
            int memid = Convert.ToInt32(Session["Memberid"]);

            using (KIITEntities db = new KIITEntities())
            {
                var data = db.Drugs;
                List<DrugModel> lst = new List<DrugModel>();
                foreach (var item in data)
                {
                    lst.Add(new DrugModel
                    {
                        DrugId = item.DrugId,
                        DrugName = item.DrugName,
                        Man_Date = item.ManufactureDate,
                        Exp_Date = item.ExpiredDate,
                        UsedFor = item.UsedFor,
                        SideEffects = item.SideEffects,
                        Quantity = item.TotalQuantityAvailable,

                    });
                }
                DrugModel model = new DrugModel();
                model.lstDrugs = lst;
                return View(model);
            }
        }
     
        [HttpPost]
        public ActionResult EditProfile(ProfileViewModel model)
        {
            int memid = Convert.ToInt32(Session["Memberid"]);
            using (KIITEntities db = new KIITEntities())
            {
                var getdata = db.Admins.FirstOrDefault(a => a.MemberId == memid);
                if (getdata != null)
                {
                    db.UpdateAdmin(model.FirstName, model.LastName, model.Address, model.Gender, memid);
                    ViewBag.message = "Updated";
                }

                else
                {
                    db.InsertAdmin(model.FirstName, model.LastName, model.Address, model.Gender, memid);
                    ViewBag.message = "Inserted";
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult Change_Password(LoginViewModel model)
        {
            int memid = Convert.ToInt32(Session["Memberid"]);
            using (KIITEntities db = new KIITEntities())
            {
                var getdata = db.MemberLogins.FirstOrDefault(a => a.MemberId == memid);

                if (model.Old_Password == getdata.Password)
                {
                    db.UpdatePassword(model.PassWord, memid);
                    ViewBag.message = "Password Updated";
                }
                else
                {
                    ViewBag.message = "Wrong Password Entered";
                }
            }

            return View();
        }

        [HttpPost]
        public JsonResult InsertDrug(DrugModel drug)
        {
            using (KIITEntities entities = new KIITEntities())
            {
                entities.InsertDrug(drug.DrugName,drug.Man_Date,drug.Exp_Date,drug.UsedFor,drug.SideEffects,drug.Quantity);
            }

            return Json(drug);
        }

        [HttpPost]
        public JsonResult DeleteDrug(DrugModel drug)
        {
            using (KIITEntities entities = new KIITEntities())
            {
                entities.RemoveDrug(drug.DrugId);
            }

            return Json(drug);
        }

        [HttpPost]
        public ActionResult UpdateDrug(DrugModel drug)
        {
            using (KIITEntities entities = new KIITEntities())
            {
                entities.UpdateDrug(drug.DrugId,drug.DrugName,drug.Man_Date,drug.Exp_Date,drug.UsedFor,drug.SideEffects,drug.Quantity);
            }

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult InsertSupplier(DrugViewModel drug)
        {
            using (KIITEntities entities = new KIITEntities())
            {
                var quan = (from d in entities.Drugs where d.DrugId == drug.DrugId select d.TotalQuantityAvailable).FirstOrDefault();
                if (drug.Quantity < quan)
                {
                    entities.InsertDelivery(drug.OrderId, drug.SupplierId);
                    entities.UpdateDQuan(quan-drug.Quantity,drug.DrugId);
                }
                else
                {
                    throw new Exception("abc");
                }
            }

            return Json(drug);
        }
    }
}