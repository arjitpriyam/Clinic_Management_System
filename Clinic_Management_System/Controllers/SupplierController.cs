using Clinic_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clinic_Management_System.Controllers
{
    [CustomAuthorize(Roles = "Supplier")]
    public class SupplierController : Controller
    {
        // GET: Supplier
        public ActionResult DashBoard()
        {
            using (KIITEntities db = new KIITEntities())
            {
                DashView model = new DashView();
                model.TotOrd = db.PatientOrderDetails.Count();
                model.DelOrd = db.PatientOrderDetails.Where(a => a.OrderStatus == "Delivered").Count();
                model.DisOrd = db.PatientOrderDetails.Where(a => a.OrderStatus == "Dispatched").Count();
                model.ReqOrd = db.PatientOrderDetails.Where(a => a.OrderStatus == "Requested").Count();
                return View(model);
            }
        }

        public ActionResult EditProfile()
        {
            using (KIITEntities db = new KIITEntities())
            {
                int memid = Convert.ToInt32(Session["Memberid"]);

                var getdata = db.Suppliers.SingleOrDefault(a => a.MemberId == memid);

                if (getdata != null)
                {
                    SupplierViewModel model = new SupplierViewModel();
                    model.FirstName = getdata.FirstName;
                    model.LastName = getdata.LastName;
                    model.CompanyName = getdata.CompanyName;
                    model.CompanyAddress = getdata.CompanyAddress;
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

        public ActionResult View_Order_Details()
        {
            int memid = Convert.ToInt32(Session["Memberid"]);

            using (KIITEntities db = new KIITEntities())
            {
                int sid = db.Suppliers.Where(a => a.MemberId == memid).Select(a => a.SupplierId).SingleOrDefault();

                var data = from pod in db.PatientOrderDetails
                           join dd in db.DrugDeliveries on pod.OrderId equals dd.OrderId
                           where dd.SupplierId == sid
                           select new { pod.DrugId, pod.Quantity,pod.OrderedDate,dd.OrderId,pod.OrderStatus,pod.OrderNumber,dd.DeliveredDate,pod.PatientId };

                List<DrugViewModel> lst = new List<DrugViewModel>();
                foreach (var item in data)
                {
                    string pat_name = db.Patients.Where(a => a.PatientId == item.PatientId).Select(a => a.FirstName).FirstOrDefault();
                    string drug_name = db.Drugs.Where(a => a.DrugId == item.DrugId).Select(a => a.DrugName).SingleOrDefault();
                    lst.Add(new DrugViewModel
                    {
                        OrderNumber=item.OrderNumber,
                        PatientName=pat_name,
                        DeliveryDate=Convert.ToDateTime(item.DeliveredDate),
                        DrugName = drug_name,
                        Quantity = item.Quantity,
                        OrderDate=item.OrderedDate,
                        OrderId=item.OrderId,
                        OrderStatus=item.OrderStatus
                    });
                }
                DrugViewModel model = new DrugViewModel();
                model.lstData = lst;
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult UpdateStatus(DrugViewModel drug)
        {
            using (KIITEntities db = new KIITEntities())
            {
                db.UpdateOrderStatus(drug.OrderStatus,drug.OrderId);
                if (drug.OrderStatus == "Delivered")
                {
                    int did = db.DrugDeliveries.Where(a => a.OrderId == drug.OrderId).Select(a => a.DeliveryId).SingleOrDefault();
                    db.UpdateDeliveryDate(did, DateTime.Now);
                }
            }
            return Json(drug);
        }

        [HttpPost]
        public ActionResult EditProfile(SupplierViewModel model)
        {
            int memid = Convert.ToInt32(Session["Memberid"]);
            using (KIITEntities db = new KIITEntities())
            {
                var getdata = db.Suppliers.FirstOrDefault(a => a.MemberId == memid);
                if (getdata != null)
                {
                    db.UpdateSupplier(model.FirstName, model.LastName, model.CompanyName, model.CompanyAddress,memid);
                    ViewBag.message = "Updated";
                }

                else
                {
                    db.InsertSupplier(model.FirstName, model.LastName, model.CompanyName, model.CompanyAddress, memid);
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

        //[HttpPost]
        //public ActionResult View_Order_Details(DrugViewModel model)
        //{
        //    int memid = Convert.ToInt32(Session["Memberid"]);
        //    using (KIITEntities db = new KIITEntities())
        //    {

        //    }

        //    return View();
        //}
    }
}