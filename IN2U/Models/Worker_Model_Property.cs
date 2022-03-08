using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IN2U.Models
{
    public class Worker_Model_Property
    {

        public int WorkerID { get; set; }
        public string RefUserId { get; set; }

        public Nullable<System.DateTime> RefPaidDate { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

    }

    public class Worker_SMS_Property
    {
        public int WorkerID { get; set; }
        public int ReminderSms { get; set; }
    }
}