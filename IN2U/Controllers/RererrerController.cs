using IN2U.Entity;
using IN2U.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IN2U.Controllers
{
    [RoutePrefix("Api/Referrer")]
    public class RererrerController : ApiController
    {
        IN2UEntities _db;

        


        [HttpPost]
        [Route("Register")]
        public IHttpActionResult Register(ReferrerInfo Referrer)
        {
            try
            {

                var response = new APIResponse();
                _db = new IN2UEntities();
                //_db.Database.Connection.Open();

                var existingcheck = _db.ReferrerInfoes.Where(p => p.Phone == Referrer.Phone ||p.Email==Referrer.Email).ToList();
                if (existingcheck.Count > 0)
                {
                    response = new APIResponse() { StatusCode = "409", Status = false, ResponseObject = Referrer, Message = "Phone Number/Email Already Exist" };
                    return Content(HttpStatusCode.Conflict, response);
                }
                Referrer.DateCreated = DateTime.Now;
                Referrer.DateChanged = null;
                _db.ReferrerInfoes.Add(Referrer);
                _db.SaveChanges();
                response = new APIResponse() { StatusCode = "200",Status = true, ResponseObject = Referrer, Message = "Row Inserted" };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errortype = ex.GetType().Name;
                string ErrorMSG = "";
               if(errortype== "DbEntityValidationException")
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
            if (ModelState.IsValid)
            {
                _db = new IN2UEntities();
                var data = _db.ReferrerInfoes.Where(p => p.Phone == Login.Phone && p.Password == Login.Password).Select(x => new IN2UReferrerInfo
                {
                    
                    RefUserId = x.RefUserId,
                    Email = x.Email,
                    //Password = x.Password,
                    Phone= x.Phone,
                    HubSpotId = x.HubSpotId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserGroup = x.UserGroup,
                    HubSpotVid = x.HubSpotVid,
                    DateCreated = x.DateCreated,
                    DateChanged = x.DateChanged

                }).ToList();
                if (data.Count > 0)
                {
                    var response = new APIResponse() { StatusCode = "200", Status = true, ResponseObject = data, Message = "" };
                    return Ok(response);
                }
                else
                {
                    var response = new APIResponse() { StatusCode = "404", Status = false, ResponseObject = data, Message = "No User Found" };
                    return Content(HttpStatusCode.NotFound, response);
                }

            }
            else
            {
                var response = new APIResponse() { StatusCode = "400", Status = false, ResponseObject = Login, Message = "Invalid Data Format" };
                return Content(HttpStatusCode.BadRequest,response);
            }

        }





        [HttpPost]
        [Route("UpdateUser")]
        public IHttpActionResult UpdateUser(ReferrerInfo UpdateUser)
        {
            try
            {

                var response = new APIResponse();
                _db = new IN2UEntities();
                //_db.Database.Connection.Open();

                var userexist = _db.ReferrerInfoes.Where(p => p.Phone == UpdateUser.Phone || p.Email == UpdateUser.Email).FirstOrDefault();
                if (userexist.RefUserId>0)
                {
                    var existingemail = _db.ReferrerInfoes.Where(p => p.HubSpotId == UpdateUser.HubSpotId || p.Email == UpdateUser.Email).FirstOrDefault();
                    if (userexist.RefUserId != existingemail.RefUserId)
                    {
                        //already email/Phone exist
                        response = new APIResponse() { StatusCode = "409", Status = false, ResponseObject = UpdateUser, Message = "User Exist With this Email/Phone" };
                        return Content(HttpStatusCode.Conflict, response);
                    }
                    else
                    {

                        _db.ReferrerInfoes.Add(UpdateUser);
                        _db.Entry(UpdateUser).State = System.Data.Entity.EntityState.Modified;
                        _db.SaveChanges();

                        response = new APIResponse() { StatusCode = "200", Status = false, ResponseObject = UpdateUser, Message = "NO User Exist With this Email/Phone" };
                        return Ok(response);
                    }
                    

                }
                else
                {
                    response = new APIResponse() { StatusCode = "404", Status = false, ResponseObject = UpdateUser, Message = "NO User Exist With this Email/Phone" };
                    return Content(HttpStatusCode.NotFound, response);
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

                        response = new APIResponse() { StatusCode = "200", Status = false, ResponseObject = UpdateUser, Message = "Password Changed" };
                        return Ok(response);
                  


                }
                else
                {
                    response = new APIResponse() { StatusCode = "404", Status = false, ResponseObject = UpdateUser, Message = "NO User Exist With this Phone" };
                    return Content(HttpStatusCode.NotFound, response);
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
            try
            {

                var response = new APIResponse();
                _db = new IN2UEntities();
                var existflag = _db.VwHS_Contacts.Where(p => p.MobilePhone == ReferFriend.MobilePhone).FirstOrDefault();
                if (existflag == null)
                {
                    

                    WorkerInfo worker = new WorkerInfo();
                    worker.RefUserId = ReferFriend.RefererID;
                    worker.ContactFirstName = ReferFriend.FirstName;
                    worker.ContactLastName = ReferFriend.LastName;
                    //worker.ContactVid = ReferFriend.ContactID;
                    worker.Email = ReferFriend.Email;
                    worker.Phone = ReferFriend.MobilePhone;
                    
                   
                    worker.Promo = ReferFriend.Promo;

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
            try
            {
                _db = new IN2UEntities();
                var data = _db.WorkerInfoes.Where(p => p.RefUserId == id).ToList();
                if (data.Count > 0)
                {
                   var response = new APIResponse() { StatusCode = "200", Status = false, ResponseObject = data, Message = "" };
                    return Content(HttpStatusCode.OK, response);
                }
                else
                {
                    var response = new APIResponse() { StatusCode = "404", Status = false, ResponseObject = data, Message = "NO User Found" };
                    return Content(HttpStatusCode.NotFound, response);
                }
            }
            catch (Exception ex)
            {
                var response = new APIResponse() { StatusCode = "500", Status = false, ResponseObject = "", Message = ex.Message };
                return Content(HttpStatusCode.BadGateway, response);
            }
        }


        [HttpGet]
        [Route("GetPaymentStatus")]
        public IHttpActionResult GetPaymentStatus(Payment_Status_Model PaymentStatus)
        {
            try
            {
                _db = new IN2UEntities();
                if (PaymentStatus.Status.Length > 0)
                {
                    var existflag = _db.WorkerInfoes.Where(p => p.RefUserId == PaymentStatus.RefUserId).ToList();
                    if (existflag.Count>0)
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




                    _db.ReferrerInfoes.Add(exist);
                    _db.Entry(exist).State = System.Data.Entity.EntityState.Modified;
                    _db.SaveChanges();
                    response = new APIResponse() { StatusCode = "200", Status = false, ResponseObject = exist, Message = " Updated" };
                    return Ok(response);
                }
                else
                {
                    response = new APIResponse() { StatusCode = "404", Status = false, ResponseObject = UpdateReferrer, Message = "No user Found With these Parameters" };
                    return Content(HttpStatusCode.NotFound, response);
                }

            }
            catch (Exception ex)
            {
                var response = new APIResponse() { StatusCode = "500", Status = false, ResponseObject = UpdateReferrer, Message = "Error" + ex.Message };
                return Content(HttpStatusCode.InternalServerError, response);
            }
        }
    }
}
