using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Clinic_Management_System.Models;

namespace Clinic_Management_System.Models
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Please Enter Subject")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please Enter Subject")]
        public string LastName { get; set; }
        [CustomValidateDOB(ErrorMessage ="Enter correct Date of Birth")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Please Select Gender")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Please Enter Address")]
        public string Address { get; set; }
        public int MemberId { get; set; }
        public List<ProfileViewModel> PatientModel { get; set; }
    }
}