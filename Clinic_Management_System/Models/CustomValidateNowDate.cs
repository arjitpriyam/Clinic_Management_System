using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Clinic_Management_System.Models
{
    public class CustomValidateNowDate : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dt = Convert.ToDateTime(value);
            if (DateTime.Now < dt)
                return true;
            else
                return false;
        }
    }
}