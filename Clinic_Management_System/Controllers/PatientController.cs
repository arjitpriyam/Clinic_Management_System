using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Clinic_Management_System.Models;

namespace Clinic_Management_System.Controllers
{
    [CustomAuthorize(Roles = "Patient")]
    public class PatientController : Controller
    {
        // GET: Patient
        public ActionResult DashBoard()
        {
            int memid = Convert.ToInt32(Session["Memberid"]);

            using (KIITEntities db = new KIITEntities())
            {
                DashView model = new DashView();
                int pid = db.Patients.Where(a => a.MemberId == memid).Select(a => a.PatientId).SingleOrDefault();
                model.App = db.DoctorAppointments.Where(a => a.PatientId == pid).Count();
                model.AprApp=db.DoctorAppointments.Where(a=> (a.PatientId==pid) && (a.AppointmentStatus=="Accept")).Count();
                model.RejApp = db.DoctorAppointments.Where(a => (a.PatientId == pid) && (a.AppointmentStatus == "Reject")).Count();
                model.ProcApp = db.DoctorAppointments.Where(a => (a.PatientId == pid) && (a.AppointmentStatus == "Requested")).Count();
                model.TotOrd = db.PatientOrderDetails.Where(a => a.PatientId == pid).Count();
                model.DelOrd = db.PatientOrderDetails.Where(a => (a.PatientId == pid) && (a.OrderStatus=="Delivered")).Count();
                model.ReqOrd = db.PatientOrderDetails.Where(a => (a.PatientId == pid) && (a.OrderStatus == "Requested") || (a.OrderStatus == "Dispatched")).Count();
                return View(model);
            }
        }

        public ActionResult EditProfile()
        {
            using (KIITEntities db = new KIITEntities())
            {
                int memid =Convert.ToInt32(Session["Memberid"]);

                var getdata = db.Patients.SingleOrDefault(a => a.MemberId == memid);

                if (getdata != null)
                {
                    ProfileViewModel model = new ProfileViewModel();
                    model.FirstName = getdata.FirstName;
                    model.LastName = getdata.LastName;
                    model.DateOfBirth =Convert.ToDateTime(getdata.DateOfBirth);
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
        public ActionResult Doctor_Appointment()
        {
            DoctorAppointmentViewModel model = new DoctorAppointmentViewModel();
            model.lstDocs = CommonData.GetDocs();

            return View(model);
        }
        public ActionResult View_App_Details()
        {
            using (KIITEntities db = new KIITEntities())
            {
                int memid = Convert.ToInt32(Session["Memberid"]);
                var p = db.Patients.Where(a => a.MemberId == memid).Select(a => new { a.FirstName, a.PatientId }).SingleOrDefault();
                var da = db.DoctorAppointments.Where(a => a.PatientId == p.PatientId);

                List<DoctorAppointmentViewModel> lst = new List<DoctorAppointmentViewModel>();
                foreach (var item in da)
                {
                    var d = db.Doctors.Where(a => a.DoctorId == item.DoctorId).SingleOrDefault();
                    lst.Add(new DoctorAppointmentViewModel
                    {
                        PatientName = p.FirstName,
                        DoctorName = d.FirstName,
                        Subject = item.Subject,
                        Description = item.Description,
                        AppointmentDate = item.AppointmentDate,
                        AppointmentStatus = item.AppointmentStatus
                    });
                }
                DoctorAppointmentViewModel model = new DoctorAppointmentViewModel();
                model.lstApp = lst;
                return View(model);
            }
        }
        public ActionResult Send_Message()
        {
            MessageViewModel model = new MessageViewModel();
            model.lstDocId = CommonData.GetDocsId();

            return View(model);
        }
        public ActionResult View_Messages()
        {
            int memid = Convert.ToInt32(Session["Memberid"]);
            
            using (KIITEntities db = new KIITEntities())
            {
                string email = db.MemberLogins.Where(a => a.MemberId == memid).Select(a => a.EmailId).SingleOrDefault();
                var sent_data = db.Inboxes.Where(a => (a.FromEmailId == email) || (a.ToEmailId==email));
                List<MessageViewModel> lst = new List<MessageViewModel>();
                foreach (var item in sent_data)
                {
                    lst.Add(new MessageViewModel
                    {
                        From_Email=item.FromEmailId,
                        To_Email = item.ToEmailId,
                        Subject=item.Subject,
                        MessageDetail=item.MessageDetail,
                        Date=item.MessageDate
                    }) ;
                }
                MessageViewModel model = new MessageViewModel();
                model.lstMessage = lst;
                return View(model);
            }
        }
        public ActionResult Raise_Drug_Request()
        {
            DrugViewModel model = new DrugViewModel();
            model.lstDrugs = CommonData.GetDrugs();
            return View(model);
        }
        public ActionResult View_Order_Details()
        {
            int memid = Convert.ToInt32(Session["Memberid"]);

            using (KIITEntities db = new KIITEntities())
            {
                var data = db.PatientOrderDetails.Select(a => new { a.DrugId, a.OrderNumber, a.Quantity, a.OrderedDate, a.OrderStatus });
                List<DrugViewModel> lst = new List<DrugViewModel>();
                foreach (var item in data)
                {
                    string drug_name = db.Drugs.Where(a => a.DrugId == item.DrugId).Select(a => a.DrugName).SingleOrDefault();
                    lst.Add(new DrugViewModel
                    {
                        OrderNumber = item.OrderNumber,
                        DrugName = drug_name,
                        Quantity = item.Quantity,
                        OrderDate = item.OrderedDate,
                        OrderStatus = item.OrderStatus
                    });
                }
                DrugViewModel model = new DrugViewModel();
                model.lstData = lst;
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult EditProfile(ProfileViewModel model)
        {
            int memid = Convert.ToInt32(Session["Memberid"]);
            using (KIITEntities db = new KIITEntities())
            {
                var getdata = db.Patients.FirstOrDefault(a => a.MemberId == memid);
                if (getdata != null)
                {
                    db.UpdatePatient(model.FirstName, model.LastName, model.DateOfBirth, model.Gender, model.Address,memid );
                    ViewBag.message = "Updated";
                }

                else
                {
                    db.InsertPatient(model.FirstName, model.LastName, model.DateOfBirth, model.Gender, model.Address, memid);
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
        public ActionResult Doctor_Appointment(DoctorAppointmentViewModel model)
        {
            int memid = Convert.ToInt32(Session["Memberid"]);
            DoctorAppointmentViewModel model_post = new DoctorAppointmentViewModel();
            model_post.lstDocs = CommonData.GetDocs();
            if (model.PatientName != null && model.Subject != null && model.DoctorId != 0)
            {
                KIITEntities db = new KIITEntities();
                int p_id = db.Patients.Where(a => a.MemberId == memid).Select(a => a.PatientId).SingleOrDefault();
                db.InsertAppointment(p_id, model.DoctorId, model.Subject,model.Description,model.AppointmentDate);
                return View(model_post);
            }
            else
            {
                return View(model_post);
            }
        }

        [HttpPost]
        public ActionResult Send_Message(MessageViewModel model)
        {
            int memid = Convert.ToInt32(Session["Memberid"]);
            MessageViewModel p_model = new MessageViewModel();
            p_model.lstDocId = CommonData.GetDocsId();
            using (KIITEntities db = new KIITEntities())
            {
                string email = db.MemberLogins.Where(a => a.MemberId == memid).Select(a => a.EmailId).SingleOrDefault();
                db.InsertMessage(email, model.To_Email, model.Subject, model.MessageDetail, DateTime.Now,model.ReplyId,0);
            }
            return View(p_model);

        }

        [HttpPost]
        public ActionResult Raise_Drug_Request(DrugViewModel model)
        {
            int memid = Convert.ToInt32(Session["Memberid"]);
            DrugViewModel model_post = new DrugViewModel();
            model_post.lstDrugs = CommonData.GetDrugs();
            int day = DateTime.Now.Day;
            int hrs = DateTime.Now.Hour;
            int min = DateTime.Now.Minute;
            int sec = DateTime.Now.Second;
            string odrno = string.Format("{0}{1}{2}{3}",day, hrs, min, sec);
            using (KIITEntities db = new KIITEntities())
            {
                int p_id = db.Patients.Where(a => a.MemberId == memid).Select(a => a.PatientId).SingleOrDefault();
                db.OrderDrug(p_id,model.DrugId, Convert.ToInt32(odrno), model.Quantity,DateTime.Now);
            }
            return View(model_post);

        }
    }
}