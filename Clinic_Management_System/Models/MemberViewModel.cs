using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clinic_Management_System.Models
{
    public class MemberViewModel
    {
        public int MemberId { get; set; }

        [Required(ErrorMessage = "Enter your Email")]
        public string EmailId { get; set; }
        [RegularExpression("^(?=.*[0-9]+.*)(?=.*[a-zA-Z]+.*)[0-9a-zA-Z]{6,}$", ErrorMessage = "Enter in the required form")]
        public string Password { get; set; }

        public int RoleId { get; set; }

        public List<SelectListItem> lstRoles { get; set; }


    }
}