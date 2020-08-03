using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clinic_Management_System.Models
{
    public class DoctorViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TotalExperience { get; set; }
        public string Gender { get; set; }
        public int SpecializedId { get; set; }
        public List<SelectListItem> lstSpec { get; set; }
        public List<DoctorViewModel> DoctorModel { get; set; }
    }
}