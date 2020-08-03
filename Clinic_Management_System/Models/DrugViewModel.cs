using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clinic_Management_System.Models
{
    public class DrugViewModel
    {
        public int OrderNumber { get; set; }
        public string PatientName { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int OrderId { get; set; }
        public int DrugId { get; set; }
        public string DrugName { get; set; }
        public int Quantity { get; set; }
        public List<SelectListItem> lstDrugs { get; set; }
        public List<DrugViewModel> lstShowDrugs { get; set; }
        public List<SelectListItem> lstSupplier { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string OrderStatus { get; set; }
        public List<DrugViewModel> lstData { get; set; }

    }
}