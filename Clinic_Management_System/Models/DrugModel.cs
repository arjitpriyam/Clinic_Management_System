using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinic_Management_System.Models
{
    public class DrugModel
    {
        public int DrugId { get; set; }
        public string DrugName { get; set; }
        public DateTime Man_Date { get; set; }
        public DateTime Exp_Date { get; set; }
        public string UsedFor { get; set; }
        public string SideEffects { get; set; }
        public int Quantity { get; set; }
        public Boolean isDeleted { get; set; }
        public List<DrugModel> lstDrugs { get; set; }
    }
}