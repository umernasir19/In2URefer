using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IN2U.Models
{
    public class APIResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public object ResponseObject { get; set; }

        public string StatusCode { get; set; }
    }
}