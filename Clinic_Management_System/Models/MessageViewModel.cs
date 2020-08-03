using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Clinic_Management_System.Models;

namespace Clinic_Management_System.Models
{
    public class MessageViewModel
    {
        public int MessageId { get; set; }
        public string Check_Email { get; set; }
        public string From_Email { get; set; }
        [Required(ErrorMessage = "Please Enter Email Address")]
        public string To_Email { get; set; }
        [Required(ErrorMessage = "Please Enter Subject")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Please Enter Message")]
        public string MessageDetail { get; set; }
        [CustomValidateNowDate(ErrorMessage ="Enter the correct date")]
        public DateTime Date { get; set; }
        public int ReplyId { get; set; }
        public bool isRead { get; set; }

        public List<SelectListItem> lstDocId { get; set; }
        public List<MessageViewModel> lstMessage { get; set; }
    }
}