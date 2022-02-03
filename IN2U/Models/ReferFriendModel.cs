using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IN2U.Models
{
    public class ReferFriendModel
    {
        public int ContactID { get; set; }
        public int Vid { get; set; }
        public string Promo { get; set; }
        public string RefererID { get; set; }
        public string UserType { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string MobilePhone { get; set; }
        public string CreateDate { get; set; }
        public string LastModifiedDate { get; set; }
    }
}