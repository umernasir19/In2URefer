using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IN2U.Models
{
    public class IN2UReferrerInfo
    {
        //public int ReferID { get; set; }
        public int RefUserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string HubSpotId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserGroup { get; set; }
        public Nullable<int> HubSpotVid { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<System.DateTime> DateChanged { get; set; }
    }
}