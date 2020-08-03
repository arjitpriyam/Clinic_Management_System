using Clinic_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clinic_Management_System.Controllers
{
    [CustomAuthorize(Roles = "Doctor")]

    public class DoctorController : Controller
    {
        // GET: Doctor
        public ActionResult DashBoard()
        {
            int memid = Convert.ToInt32(Session["Memberid"]);
            using (KIITEntities db=new KIITEntities())
            {
                DashView model = new DashView();
                string doceid = db.MemberLogins.Where(a => a.MemberId == memid).Select(a => a.EmailId).SingleOrDefault();
                int docid = db.Doctors.Where(a => a.MemberId == memid).Select(a => a.DoctorId).SingleOrDefault();
                model.App = db.DoctorAppointments.Where(a=> a.DoctorId== docid).Count();
                model.AprApp=db.DoctorAppointments.Where(a=> (a.AppointmentStatus== "Accept") && (a.DoctorId==docid)).Count();
                model.RejApp = db.DoctorAppointments.Where(a => (a.AppointmentStatus == "Reject") && (a.DoctorId == docid)).Count();
                model.ProcApp = db.DoctorAppointments.Where(a => (a.AppointmentStatus == "Requested") && (a.DoctorId == docid)).Count();
                model.Inbox = db.Inboxes.Where(a => a.ToEmailId == doceid).Count();
                model.RInbox= db.Inboxes.Where(a => (a.ToEmailId == doceid) && (a.IsRead==true)).Count(); ;
                model.URInbox=db.Inboxes.Where(a => (a.ToEmailId == doceid) && (a.IsRead == false)).Count(); ;
                return View(model);
            }

        }

        public ActionResult EditProfile()
        {
            using (KIITEntities db = new KIITEntities())
            {
                int memid = Convert.ToInt32(Session["Memberid"]);

                var getdata = db.Doctors.SingleOrDefault(a => a.MemberId == memid);
                DoctorViewModel model = new DoctorViewModel();
                if (getdata != null)
                {
                    
                    model.FirstName = getdata.FirstName;
                    model.LastName=getdata.LastName;
                    model.Gender=getdata.Gender;
                    model.TotalExperience=getdata.TotalExperience;
                    model.SpecializedId=getdata.SpecializedId;
                    model.lstSpec = CommonData.GetSpecs();
                    return View(model);
                }
                else
                {
                    model.lstSpec = CommonData.GetSpecs();
                    return View(model);
                }
            }


        }
        public ActionResult Change_Password()
        {
            return View();
        }

        public ActionResult AppointmentDetails()
        {
            using (KIITEntities db = new KIITEntities())
            {
                int memid = Convert.ToInt32(Session["Memberid"]);
                int doc_id = db.Doctors.Where(a => a.MemberId == memid).Select(a => a.DoctorId).SingleOrDefault();
                var patient = db.DoctorAppointments.Where(a => a.DoctorId == doc_id);
                List<AppointmentViewModel> lst = new List<AppointmentViewModel>();
                foreach (var item in patient)
                {
                    string pat_name = db.Patients.Where(a => a.PatientId == item.PatientId).Select(a => a.FirstName).SingleOrDefault();
                    lst.Add(new AppointmentViewModel
                    {
                        AppId=item.AppointmentId,
                        PName = pat_name,
                        Subject = item.Subject,
                        Description = item.Description,
                        Date = item.AppointmentDate,
                        AppStatus=item.AppointmentStatus
                    });
                }
                AppointmentViewModel model = new AppointmentViewModel();
                model.lst = lst;
                return View(model);
            }
        }

        public JsonResult UpdateStatus(AppointmentViewModel model)
        {
            using(KIITEntities entities=new KIITEntities())
            {
                entities.UpdateAppStatus(model.AppStatus,model.AppId);
            }
            return Json(model);
        }

        public ActionResult View_Message()
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
                        MessageId = item.MessageId,
                        From_Email = item.FromEmailId,
                        To_Email = item.ToEmailId,
                        Subject = item.Subject,
                        MessageDetail = item.MessageDetail,
                        Date = item.MessageDate,
                        isRead = item.IsRead,
                        Check_Email=email
                    });
                }
                MessageViewModel model = new MessageViewModel();
                model.lstMessage = lst;
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult UpdateMessage(MessageViewModel message)
        {
            using (KIITEntities entities = new KIITEntities())
            {
                entities.InsertReplyMessage(message.From_Email,message.To_Email,message.Subject,message.MessageDetail,DateTime.Now,message.MessageId);
                entities.UpdateMessageStatus(message.MessageId);
            }

            return Json(message);
        }

        [HttpPost]
        public ActionResult EditProfile(DoctorViewModel model)
        {
            int memid = Convert.ToInt32(Session["Memberid"]);
            DoctorViewModel model_post = new DoctorViewModel();
            model_post.lstSpec = CommonData.GetSpecs();
            using (KIITEntities db = new KIITEntities())
            {
                var getdata = db.Patients.FirstOrDefault(a => a.MemberId == memid);
                if (getdata != null)
                {
                    db.UpdateDoctor(model.FirstName, model.LastName, model.TotalExperience,model.SpecializedId, model.Gender,memid);
                    ViewBag.message = "Updated";
                }

                else
                {
                    db.InsertDoctor(model.FirstName, model.LastName, model.TotalExperience, model.SpecializedId, model.Gender, memid);
                    ViewBag.message = "Inserted";
                }
            }

            return View(model_post);
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
    }
}