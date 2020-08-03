using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinic_Management_System.Models
{
    public class DashView
    {
        public int Docs { get; set; }
        public int Patn { get; set; }
        public int Sups { get; set; }
        public int TotOrd { get; set; }
        public int DelOrd { get; set; }
        public int DisOrd { get; set; }
        public int ReqOrd{ get; set; }
        public int AssOrd { get; set; }
        public int NAssOrd { get; set; }
        public int App { get; set; }
        public int Inbox { get; set; }
        public int AprApp { get; set; }
        public int RejApp { get; set; }
        public int ProcApp { get; set; }
        public int RInbox { get; set; }
        public int URInbox { get; set; }


    }
}