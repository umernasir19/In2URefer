using IN2U.Entity;
using IN2U.Models;
//using IN2U.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;

namespace IN2U.Controllers
{

  
    [RoutePrefix("Api/Referrer")]
    public class RererrerController : ApiController
    {
        IN2UEntities _db;
        string URL = WebConfigurationManager.AppSettings["URL"];
        string TokenValue = WebConfigurationManager.AppSettings["Token"];
        public bool CrossDomain(string objreq)
        {
            var req = Request;
            var flag = objreq;
            if (req.Headers.Contains("Token"))
            {
                string token = req.Headers.GetValues("Token").First();
                if (token == TokenValue)
                {
                    return true;
                }
            }
            if (URL.Contains(objreq))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        [HttpPost]
        [Route("Register")]
        public IHttpActionResult Register(ReferrerInfo Referrer)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
           
            string ipAddress = context.Request.UserHostAddress;
            var Badreq = new APIResponse() { StatusCode = "502", Status = false, ResponseObject = ipAddress, Message = "Cross Domanin Not Allowed" };
            string flag = ipAddress;// .Host; 
            if (!CrossDomain(flag)) return Content(HttpStatusCode.BadGateway, Badreq);
            try
            {

                var response = new APIResponse();
                _db = new IN2UEntities();
                //_db.Database.Connection.Open();

                var existingcheck = _db.ReferrerInfoes.Where(p => p.Phone == Referrer.Phone).ToList();
                if (existingcheck.Count > 0)
                {

                        response = new APIResponse() { StatusCode = "409", Status = false, ResponseObject = Referrer, Message = "Phone Number/Email Already Exist" };
                        return Content(HttpStatusCode.Conflict, response);
                  
                }
                if (Referrer.Email.Length > 0)
                {
                    existingcheck = _db.ReferrerInfoes.Where(p => p.Email == Referrer.Email).ToList();
                    if (existingcheck.Count > 0)
                    {
                        response = new APIResponse() { StatusCode = "409", Status = false, ResponseObject = Referrer, Message = "Phone Number/Email Already Exist" };
                        return Content(HttpStatusCode.Conflict, response);
                    }

                }
                Referrer.DateCreated = DateTime.Now;
                Referrer.DateChanged = null;
                _db.ReferrerInfoes.Add(Referrer);
                _db.SaveChanges();
                response = new APIResponse() { StatusCode = "200", Status = true, ResponseObject = Referrer, Message = "Row Inserted" };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errortype = ex.GetType().Name;
                string ErrorMSG = "";
                if (errortype == "DbEntityValidationException")
                {
                    DbEntityValidationException entityerror = (DbEntityValidationException)ex;
                    foreach (var eve in entityerror.EntityValidationErrors)
                    {
                        //Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        //    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            ErrorMSG = string.Concat(ErrorMSG, " Property " + ve.PropertyName + " has following Error " + ve.ErrorMessage + "\n");

                        }
                    }
                    var response = new APIResponse() { StatusCode = "500", Status = false, ResponseObject = Referrer, Message = "Error " + ErrorMSG };
                    return Content(HttpStatusCode.InternalServerError, response);
                }
                else
                {
                    var response = new APIResponse() { StatusCode = "500", Status = false, ResponseObject = Referrer, Message = "Error" + ex.Message };
                    return Content(HttpStatusCode.InternalServerError, response);
                }


            }
        }


        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login(Login_Property Login)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.UserHostAddress;
            var Badreq = new APIResponse() { StatusCode = "502", Status = false, ResponseObject = ipAddress, Message = "Cross Domanin Not Allowed" };
            string flag = ipAddress;// .Host; 
            if (!CrossDomain(flag)) return Content(HttpStatusCode.BadGateway, Badreq);

            if (ModelState.IsValid)
            {
                _db = new IN2UEntities();
                var userexist = _db.ReferrerInfoes.Where(p => p.Phone == Login.Phone).FirstOrDefault();
                if (userexist != null)
                {
                    var data = _db.ReferrerInfoes.Where(p => p.Phone == Login.Phone && p.Password == Login.Password).Select(x => new IN2UReferrerInfo
                    {

                        RefUserId = x.RefUserId,
                        Email = x.Email,
                        //Password = x.Password,
                        Phone = x.Phone,
                        HubSpotId = x.HubSpotId,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        UserGroup = x.UserGroup,
                        HubSpotVid = x.HubSpotVid,
                        InHubSpot = false,
                        DateCreated = x.DateCreated,
                        DateChanged = x.DateChanged,
                        Address1 = x.Address1,
                        Address2 = x.Address2,
                        City = x.City,
                        State = x.State,
                        ZipCode = x.ZipCode

                    }).FirstOrDefault();
                    if (data != null)
                    {
                        var existinhubspot = _db.VwHS_Contacts.Where(p => p.MobilePhone == data.Phone).FirstOrDefault();
                        if (existinhubspot != null)
                        {
                            data.InHubSpot = true;
                        }

                        var response = new APIResponse() { StatusCode = "200", Status = true, ResponseObject = data, Message = "" };
                        return Ok(response);
                    }
                    else
                    {
                        var response = new APIResponse() { StatusCode = "401", Status = false, ResponseObject = data, Message = "Invalid Credentials" };
                        return Content(HttpStatusCode.Unauthorized, response);
                    }
                }
                else
                {
                    var response = new APIResponse() { StatusCode = "422", Status = false, ResponseObject = Login, Message = "No User Found" };
                    return Content((HttpStatusCode)422, response);
                }
               

                

            }
            else
            {
                var response = new APIResponse() { StatusCode = "400", Status = false, ResponseObject = Login, Message = "Invalid Data Format" };
                return Content(HttpStatusCode.BadRequest, response);
            }

        }





        [HttpPost]
        [Route("UpdateUser")]
        public IHttpActionResult UpdateUser(ReferrerInfo UpdateUser)
         {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.UserHostAddress;
            var Badreq = new APIResponse() { StatusCode = "502", Status = false, ResponseObject = ipAddress, Message = "Cross Domanin Not Allowed" };
            string flag = ipAddress;// .Host; 
            if (!CrossDomain(flag)) return Content(HttpStatusCode.BadGateway, Badreq);
            try
            {

                var response = new APIResponse();
                _db = new IN2UEntities();
                //_db.Database.Connection.Open();
                
                var userexist = _db.ReferrerInfoes.Where(p => p.Phone == UpdateUser.Phone && p.Phone != null || p.Email == UpdateUser.Email && p.Email != null || p.RefUserId == UpdateUser.RefUserId).FirstOrDefault();
                if (userexist.RefUserId > 0)
                {
                    var existingemail = _db.ReferrerInfoes.Where(p => p.Email == UpdateUser.Email && p.Email != null &&p.RefUserId!=userexist.RefUserId).FirstOrDefault();
                    if (existingemail != null)
                    {
                        if (userexist.RefUserId != existingemail.RefUserId)
                        {
                            //already email exist
                            response = new APIResponse() { StatusCode = "409", Status = false, ResponseObject = UpdateUser, Message = "Another User Exist With this Email/Phone" };
                            return Content(HttpStatusCode.Conflict, response);
                        }
                        else
                        {
                            userexist.Email = UpdateUser.Email;
                            //userexist.Phone = UpdateUser.Phone;
                            userexist.Password = ((UpdateUser.Password != null && UpdateUser.Password.Count() > 0) ? UpdateUser.Password : userexist.Password);
                            // userexist.HubSpotId = UpdateUser.HubSpotId;
                            userexist.FirstName = UpdateUser.FirstName;
                            userexist.LastName = UpdateUser.LastName;
                            //userexist.UserGroup = UpdateUser.UserGroup;
                            //userexist.HubSpotVid = UpdateUser.HubSpotVid;
                            userexist.DateChanged = DateTime.Now;
                            _db.ReferrerInfoes.Add(userexist);
                            _db.Entry(userexist).State = System.Data.Entity.EntityState.Modified;
                            _db.SaveChanges();

                            response = new APIResponse() { StatusCode = "200", Status = true, ResponseObject = UpdateUser, Message = "Updated" };
                            return Ok(response);
                        }
                    }
                    else
                    {
                        //userexist.Email = (UpdateUser.Email!=null ? UpdateUser.Email : userexist.Email);
                        //userexist.Phone = UpdateUser.Phone.Count()>0? UpdateUser.Phone : userexist.Phone;
                        
                        userexist.Password = ((UpdateUser.Password!=null && UpdateUser.Password.Count()>0) ?UpdateUser.Password:userexist.Password);
                       // userexist.HubSpotId = UpdateUser.HubSpotId;
                        userexist.FirstName = UpdateUser.FirstName;
                        userexist.LastName   = UpdateUser.LastName;
                      //  userexist.UserGroup = UpdateUser.UserGroup;
                      //  userexist.HubSpotVid = UpdateUser.HubSpotVid;
                        userexist.DateChanged = DateTime.Now;
                       

                        _db.ReferrerInfoes.Add(userexist);
                        _db.Entry(userexist).State = System.Data.Entity.EntityState.Modified;
                        _db.SaveChanges();

                        response = new APIResponse() { StatusCode = "200", Status = true, ResponseObject = UpdateUser, Message = "Updated" };
                        return Ok(response);
                    }


                }
                else
                {
                    response = new APIResponse() { StatusCode = "422", Status = false, ResponseObject = UpdateUser, Message = "NO User Exist With this Email/Phone" };
                    return Content((HttpStatusCode)422, response);
                }

            }
            catch (Exception ex)
            {
                var response = new APIResponse() { StatusCode = "500", Status = false, ResponseObject = UpdateUser, Message = "Error" + ex.Message };
                return Content(HttpStatusCode.InternalServerError, response);
            }
        }



        [HttpPost]
        [Route("ForgotPassword")]
        public IHttpActionResult ForgotPassword(ReferrerInfo UpdateUser)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.UserHostAddress;
            var Badreq = new APIResponse() { StatusCode = "502", Status = false, ResponseObject = ipAddress, Message = "Cross Domanin Not Allowed" };
            string flag = ipAddress;// .Host; 
            if (!CrossDomain(flag)) return Content(HttpStatusCode.BadGateway, Badreq);
            try
            {

                var response = new APIResponse();
                _db = new IN2UEntities();
                //_db.Database.Connection.Open();

                var userexist = _db.ReferrerInfoes.Where(p => p.Phone == UpdateUser.Phone).FirstOrDefault();
                if (userexist.RefUserId > 0)
                {

                    userexist.Password = UpdateUser.Password;
                    _db.ReferrerInfoes.Add(userexist);
                    _db.Entry(userexist).State = System.Data.Entity.EntityState.Modified;
                    _db.SaveChanges();

                    response = new APIResponse() { StatusCode = "200", Status = true, ResponseObject = UpdateUser, Message = "Password Changed" };
                    return Ok(response);



                }
                else
                {
                    response = new APIResponse() { StatusCode = "422", Status = false, ResponseObject = UpdateUser, Message = "NO User Exist With this Phone" };
                    return Content((HttpStatusCode)422, response);
                }

            }
            catch (Exception ex)
            {
                var response = new APIResponse() { StatusCode = "500", Status = false, ResponseObject = UpdateUser, Message = "Error" + ex.Message };
                return Content(HttpStatusCode.InternalServerError, response);
            }
        }


        [HttpPost]
        [Route("ReferFriends")]
        public IHttpActionResult ReferFriends(ReferFriendModel ReferFriend)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.UserHostAddress;
            var Badreq = new APIResponse() { StatusCode = "502", Status = false, ResponseObject = ipAddress, Message = "Cross Domanin Not Allowed" };
            string flag = ipAddress;// .Host; 
            if (!CrossDomain(flag)) return Content(HttpStatusCode.BadGateway, Badreq);
            try
            {

                var response = new APIResponse();
                _db = new IN2UEntities();
                var existflag = _db.VwHS_Contacts.Where(p => p.MobilePhone == ReferFriend.Phone).FirstOrDefault();
                if (existflag == null)
                {

                    var workerduplicate = _db.WorkerInfoes.Where(p => p.RefUserId == ReferFriend.RefererID && p.Phone == ReferFriend.Phone).FirstOrDefault();
                    if (workerduplicate != null)
                    {
                        response = new APIResponse() { StatusCode = "422", Status = false, ResponseObject = ReferFriend, Message = "Referee already exist With This Phone" };
                        return Content(HttpStatusCode.Conflict, response);
                    }

                    WorkerInfo worker = new WorkerInfo();
                    worker.RefUserId = ReferFriend.RefererID;
                    worker.ContactFirstName = ReferFriend.FirstName;
                    worker.ContactLastName = ReferFriend.LastName;
                    //worker.ContactVid = ReferFriend.ContactID;
                    worker.Email = ReferFriend.Email;
                    worker.Phone = ReferFriend.Phone;
                    worker.DateCreated = DateTime.Now;

                    worker.Promo = ReferFriend.Promo;
                    worker.ReminderSms = 0;
                    _db.WorkerInfoes.Add(worker);
                    _db.SaveChanges();
                    response = new APIResponse() { StatusCode = "200", Status = true, ResponseObject = worker, Message = "Worker Added Successfully" };
                    return Ok(response);
                }
                else
                {
                    response = new APIResponse() { StatusCode = "422", Status = false, ResponseObject = ReferFriend, Message = "Referee already exist " };
                    return Content(HttpStatusCode.Conflict, response);
                }

            }
            catch (Exception ex)
            {
                var response = new APIResponse() { StatusCode = "500", Status = false, ResponseObject = ReferFriend, Message = "Error" + ex.Message };
                return Content(HttpStatusCode.InternalServerError, response);
            }
        }


        [HttpGet]
        [Route("GetRefreeByRefererID")]
        public IHttpActionResult GetRefreeByRefererID(string id)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.UserHostAddress;
            var Badreq = new APIResponse() { StatusCode = "502", Status = false, ResponseObject = ipAddress, Message = "Cross Domanin Not Allowed" };
            string flag = ipAddress;// .Host; 
            if (!CrossDomain(flag)) return Content(HttpStatusCode.BadGateway, Badreq);
            try
            {
               
                //var flag = Request.Headers;
                _db = new IN2UEntities();
                var data = _db.WorkerInfoes.Where(p => p.RefUserId == id).ToList();
                if (data.Count > 0)
                {
                    var response = new APIResponse() { StatusCode = "200", Status = true, ResponseObject = data, Message = "" };
                    return Content(HttpStatusCode.OK, response);
                }
                else
                {
                    var response = new APIResponse() { StatusCode = "422", Status = false, ResponseObject = data, Message = "NO User Found" };
                    return Content((HttpStatusCode)422, response);
                }
            }
            catch (Exception ex)
            {
                var response = new APIResponse() { StatusCode = "500", Status = false, ResponseObject = "", Message = ex.Message };
                return Content(HttpStatusCode.BadGateway, response);
            }
        }


        [HttpPost]
        [Route("GetPaymentStatus")]
        public IHttpActionResult GetPaymentStatus(Payment_Status_Model PaymentStatus)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.UserHostAddress;
            var Badreq = new APIResponse() { StatusCode = "502", Status = false, ResponseObject = ipAddress, Message = "Cross Domanin Not Allowed" };
            string flag = ipAddress;// .Host; 
            if (!CrossDomain(flag)) return Content(HttpStatusCode.BadGateway, Badreq);
            try
            {
                _db = new IN2UEntities();
                if (PaymentStatus.Status != null)
                {
                    if (PaymentStatus.Status.Length > 0)
                    {
                        var existflag = _db.WorkerInfoes.Where(p => p.RefUserId == PaymentStatus.RefUserId).ToList();
                        if (existflag.Count > 0)
                        {
                            existflag = existflag.Where(p => p.RefPaidStatus == PaymentStatus.Status).ToList();
                            var response = new APIResponse() { StatusCode = "200", Status = true, ResponseObject = existflag, Message = "" };
                            return Ok(response);
                        }
                        else
                        {
                            var response = new APIResponse() { StatusCode = "409", Status = false, ResponseObject = PaymentStatus, Message = "No User Found" };
                            return Content(HttpStatusCode.Conflict, response);
                        }

                    }
                    else
                    {
                        var existflag = _db.WorkerInfoes.Where(p => p.RefUserId == PaymentStatus.RefUserId).ToList();
                        if (existflag.Count > 0)
                        {

                            var response = new APIResponse() { StatusCode = "200", Status = true, ResponseObject = existflag, Message = "" };
                            return Ok(response);
                        }
                        else
                        {
                            var response = new APIResponse() { StatusCode = "409", Status = false, ResponseObject = PaymentStatus, Message = "No User Found" };
                            return Content(HttpStatusCode.Conflict, response);
                        }

                    }
                }
                else
                {
                    var existflag = _db.WorkerInfoes.Where(p => p.RefUserId == PaymentStatus.RefUserId).ToList();
                    if (existflag.Count > 0)
                    {

                        var response = new APIResponse() { StatusCode = "200", Status = true, ResponseObject = existflag, Message = "" };
                        return Ok(response);
                    }
                    else
                    {
                        var response = new APIResponse() { StatusCode = "409", Status = false, ResponseObject = PaymentStatus, Message = "No User Found" };
                        return Content(HttpStatusCode.Conflict, response);
                    }
                }
            }
            catch (Exception ex)
            {
                var response = new APIResponse() { StatusCode = "500", Status = false, ResponseObject = "", Message = ex.Message };
                return Content(HttpStatusCode.BadGateway, response);
            }
        }



        [HttpPost]
        [Route("UpdateReferrerAddress")]
        public IHttpActionResult UpdateReferrerAddress(ReferrerInfo UpdateReferrer)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.UserHostAddress;
            var Badreq = new APIResponse() { StatusCode = "502", Status = false, ResponseObject = ipAddress, Message = "Cross Domanin Not Allowed" };
            string flag = ipAddress;// .Host; 
            if (!CrossDomain(flag)) return Content(HttpStatusCode.BadGateway, Badreq);
            try
            {

                var response = new APIResponse();
                _db = new IN2UEntities();
                //_db.Database.Connection.Open();
                //for Update of Payment Status
                var exist = _db.ReferrerInfoes.Where(p => p.RefUserId == UpdateReferrer.RefUserId).FirstOrDefault();
                if (exist != null)
                {
                    //user found
                    exist.Address1 = UpdateReferrer.Address1;
                    exist.Address2 = UpdateReferrer.Address2;
                    exist.State = UpdateReferrer.State;
                    exist.City = UpdateReferrer.City;
                    exist.ZipCode = UpdateReferrer.ZipCode;
                    exist.DateChanged = DateTime.Now;



                    _db.ReferrerInfoes.Add(exist);
                    _db.Entry(exist).State = System.Data.Entity.EntityState.Modified;
                    _db.SaveChanges();
                    response = new APIResponse() { StatusCode = "200", Status = true, ResponseObject = exist, Message = " Updated" };
                    return Ok(response);
                }
                else
                {
                    response = new APIResponse() { StatusCode = "422", Status = false, ResponseObject = UpdateReferrer, Message = "No user Found With these Parameters" };
                    return Content((HttpStatusCode)422, response);
                }

            }
            catch (Exception ex)
            {
                var response = new APIResponse() { StatusCode = "500", Status = false, ResponseObject = UpdateReferrer, Message = "Error" + ex.Message };
                return Content(HttpStatusCode.InternalServerError, response);
            }
        }


        [HttpPost]
        [Route("ResetPassword")]
        public IHttpActionResult ResetPassword(ResetPassword_Property UpdatePassword)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.UserHostAddress;
            var Badreq = new APIResponse() { StatusCode = "502", Status = false, ResponseObject = ipAddress, Message = "Cross Domanin Not Allowed" };
            string flag = ipAddress;// .Host; 
            if (!CrossDomain(flag)) return Content(HttpStatusCode.BadGateway, Badreq);
            if (ModelState.IsValid)
            {
                try
                {

                    var response = new APIResponse();
                    _db = new IN2UEntities();
                    var exist = _db.ReferrerInfoes.Where(p => p.RefUserId == UpdatePassword.RefUserId && p.Password == UpdatePassword.CurrentPassword).FirstOrDefault();
                    if (exist != null)
                    {
                        //user found
                        exist.Password = UpdatePassword.NewPassword;
                        _db.ReferrerInfoes.Add(exist);
                        _db.Entry(exist).State = System.Data.Entity.EntityState.Modified;
                        _db.SaveChanges();
                        response = new APIResponse() { StatusCode = "200", Status = true, ResponseObject = exist, Message = " Updated" };
                        return Ok(response);
                    }
                    else
                    {
                        response = new APIResponse() { StatusCode = "422", Status = false, ResponseObject = UpdatePassword, Message = "Either Current Password Is Not Correct Or No User Found" };
                        return Content((HttpStatusCode)422, response);
                    }

                }
                catch (Exception ex)
                {
                    var response = new APIResponse() { StatusCode = "500", Status = false, ResponseObject = UpdatePassword, Message = "Error" + ex.Message };
                    return Content(HttpStatusCode.InternalServerError, response);
                }
            }
            else
            {
                var response = new APIResponse() { StatusCode = "400", Status = false, ResponseObject = UpdatePassword, Message = "Validation Error In Password or New Password Or User ID" };
                return Content(HttpStatusCode.BadRequest, response);
            }
        }

        [HttpGet]
        [Route("GetReferrerByPhoneNumber")]
        public IHttpActionResult GetReferrerByPhoneNumber(string Phone)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.UserHostAddress;
            var Badreq = new APIResponse() { StatusCode = "502", Status = false, ResponseObject = ipAddress, Message = "Cross Domanin Not Allowed" };
            string flag = ipAddress;// .Host; 
            if (!CrossDomain(flag)) return Content(HttpStatusCode.BadGateway, Badreq);
            try
            {
                _db = new IN2UEntities();
                var data = _db.ReferrerInfoes.Where(p => p.Phone == Phone).ToList();
                if (data.Count > 0)
                {
                    var response = new APIResponse() { StatusCode = "200", Status = true, ResponseObject = data, Message = "" };
                    return Ok(response);
                }
                else
                {
                    var response = new APIResponse() { StatusCode = "422", Status = false, ResponseObject = data, Message = "NO User Found" };
                    return Content((HttpStatusCode)422, response);
                }
            }
            catch (Exception ex)
            {
                var response = new APIResponse() { StatusCode = "500", Status = false, ResponseObject = "", Message = ex.Message };
                return Ok(response);
            }
        }


        [HttpPost]
        [Route("UpdateWorkerSMSCount")]
        public IHttpActionResult UpdateWorkerSMSCount(Worker_SMS_Property obj)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.UserHostAddress;
            var Badreq = new APIResponse() { StatusCode = "502", Status = false, ResponseObject = ipAddress, Message = "Cross Domanin Not Allowed" };
            string flag = ipAddress;// .Host; 
            if (!CrossDomain(flag)) return Content(HttpStatusCode.BadGateway, Badreq);
            try
            {
                var response = new APIResponse();
                _db = new IN2UEntities();
                var worker = _db.WorkerInfoes.Where(p => p.WorkerID == obj.WorkerID).FirstOrDefault();
                if (worker != null)
                {
                    worker.ReminderSms = obj.ReminderSms;
                    _db.WorkerInfoes.Add(worker);
                    _db.Entry(worker).State = System.Data.Entity.EntityState.Modified;
                    _db.SaveChanges();
                    response = new APIResponse() { StatusCode = "200", Status = true, ResponseObject = worker, Message = " Updated" };
                    return Ok(response);
                }
                else
                {
                    response = new APIResponse() { StatusCode = "422", Status = false, ResponseObject = obj, Message = "No User Found" };
                    return Content((HttpStatusCode)422, response);
                }
            }
            catch(Exception ex)
            {
                var response = new APIResponse() { StatusCode = "500", Status = false, ResponseObject = obj, Message = "Error" + ex.Message };
                return Content(HttpStatusCode.InternalServerError, response);
            }
        
        }
    }
}
