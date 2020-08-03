using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Clinic_Management_System.Models;

namespace Clinic_Management_System.Models
{
    public class DoctorAppointmentViewModel
    {
        [Required(ErrorMessage = "Please Enter Name")]
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        [Required(ErrorMessage = "Please Enter Subject")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Please Enter Description")]
        public string Description { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentStatus { get; set; }
        public int DoctorId { get; set; }
        public List<SelectListItem> lstDocs { get; set; }

        public List<DoctorAppointmentViewModel> lstApp { get; set; }






    }
}