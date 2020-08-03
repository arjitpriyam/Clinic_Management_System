using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Clinic_Management_System.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Enter your name")]
        public string Username { get; set; }
        [RegularExpression("^(?=.*[0-9]+.*)(?=.*[a-zA-Z]+.*)[0-9a-zA-Z]{6,}$", ErrorMessage = "Enter in the required form")]
        public string PassWord { get; set; }
        [Compare("PassWord", ErrorMessage = "Password do not match")]
        public string Confirm_Password { get; set; }
        public string Old_Password { get; set; }

        public List<LoginViewModel> User { get; set; }
    }
}