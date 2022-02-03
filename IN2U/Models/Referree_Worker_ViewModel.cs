using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IN2U.Models
{
    public class Referree_Worker_ViewModel
    {
        public int RefUserId { get; set; }
        //public string Email { get; set; }
        //public string Phone { get; set; }
        public string Password { get; set; }
        public string HubSpotId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserGroup { get; set; }
        public Nullable<int> HubSpotVid { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<System.DateTime> DateChanged { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        public int WorkerID { get; set; }
        //public string RefUserId { get; set; }
        public Nullable<int> ContactVid { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string InvitationMethod { get; set; }
        public Nullable<int> ContactEmployeeNo { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ReferredCompany { get; set; }
        public Nullable<System.DateTime> ReferredDate { get; set; }
        public string ReferralStatus { get; set; }
        public Nullable<System.DateTime> WorkStartDate { get; set; }
        public Nullable<System.DateTime> WorkEndDate { get; set; }
        public string WorkStatus { get; set; }
        public string TerminateReason { get; set; }
        //public Nullable<System.DateTime> DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public string RefPaidStatus { get; set; }
        public Nullable<decimal> RefPaidAmount { get; set; }
        public Nullable<System.DateTime> RefPaidDate { get; set; }
        public string RefPaidMethod { get; set; }
        public string Promo { get; set; }
    }
}