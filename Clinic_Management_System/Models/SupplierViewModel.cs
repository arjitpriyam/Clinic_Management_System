using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinic_Management_System.Models
{
    public class SupplierViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public int MemberId { get; set; }
        public List<SupplierViewModel> SupplierModel { get; set; }
    }
}