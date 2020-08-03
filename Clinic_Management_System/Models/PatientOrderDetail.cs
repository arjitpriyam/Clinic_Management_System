//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Clinic_Management_System.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PatientOrderDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PatientOrderDetail()
        {
            this.DrugDeliveries = new HashSet<DrugDelivery>();
        }
    
        public int OrderId { get; set; }
        public int PatientId { get; set; }
        public int DrugId { get; set; }
        public int OrderNumber { get; set; }
        public int Quantity { get; set; }
        public System.DateTime OrderedDate { get; set; }
        public string OrderStatus { get; set; }
    
        public virtual Drug Drug { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DrugDelivery> DrugDeliveries { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
