//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IN2U.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class WorkerInfo
    {
        public int WorkerID { get; set; }
        public string RefUserId { get; set; }
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
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public string RefPaidStatus { get; set; }
        public Nullable<decimal> RefPaidAmount { get; set; }
        public Nullable<System.DateTime> RefPaidDate { get; set; }
        public string RefPaidMethod { get; set; }
        public string Promo { get; set; }
        public Nullable<int> ReminderSms { get; set; }
    }
}
