using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IN2U.Models
{
    public class Login_Property
    {
        [Required(ErrorMessage ="Please Enter Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please Enter Phone")]
        public string Phone { get; set; }
    }
}