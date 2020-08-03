using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Clinic_Management_System.Models;

namespace Clinic_Management_System.Models
{
    public class AppointmentViewModel
    {
        public int AppId { get; set; }
        public string PName { get; set; }
        public string DName { get; set; }
        public string Subject { get; set; }
        public string Description    { get; set; }
        public DateTime Date { get; set; }
        public string AppStatus { get; set; }

        public List<AppointmentViewModel> lst { get; set; }

    }
}