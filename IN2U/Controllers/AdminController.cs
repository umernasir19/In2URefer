using IN2U.Entity;
using IN2U.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IN2U.Controllers
{
    [RoutePrefix("Api/Admin")]
    public class AdminController : ApiController
    {
        IN2UEntities _db;

        [HttpGet]
        [Route("GetAllReferrer")]
        public IHttpActionResult GetAllReferrer()
        {
            try
            {
                _db = new IN2UEntities();
                var data = _db.Database.SqlQuery<Referree_Worker_ViewModel>("select * from ReferrerInfo A inner join WorkerInfo B on A.RefUSERID=B.RefUserId")
                            .ToList();
                var response = new APIResponse() { StatusCode = "200", Status = true, ResponseObject = data, Message = "" };
                return Ok(response);
            }
            catch(Exception ex)
            {
                var response = new APIResponse() { StatusCode = "500", Status = false, ResponseObject = "", Message = ex.Message };
                return Ok(response);
            }
        }



        [HttpGet]
        [Route("GetReferrerById")]
        public IHttpActionResult GetReferrerById(int id)
        {
            try
            {
                _db = new IN2UEntities();
                var data = _db.ReferrerInfoes.Where(p=>p.RefUserId==id).ToList();
                if (data.Count > 0)
                {
                    var response = new APIResponse() { StatusCode = "200", Status = true, ResponseObject = data, Message = "" };
                    return Ok(response);
                }
                else
                {
                    var response = new APIResponse() { StatusCode = "200", Status = true, ResponseObject = data, Message = "NO User Found" };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                var response = new APIResponse() { StatusCode = "500", Status = false, ResponseObject = "", Message = ex.Message };
                return Ok(response);
            }
        }




        [HttpPost]
        [Route("UpdateRefreePayDate")]
        public IHttpActionResult UpdateRefreePayDate(Worker_Model_Property UpdateWorker)
        {
            try
            {

                var response = new APIResponse();
                _db = new IN2UEntities();
                //_db.Database.Connection.Open();
                //for Update of Payment Status
                var exist = _db.WorkerInfoes.Where(p => p.WorkerID == UpdateWorker.WorkerID && p.RefUserId == UpdateWorker.RefUserId).FirstOrDefault();
                if (exist != null)
                {
                    //user found
                    exist.RefPaidDate = UpdateWorker.RefPaidDate;
                    _db.WorkerInfoes.Add(exist);
                    _db.Entry(exist).State = System.Data.Entity.EntityState.Modified;
                    _db.SaveChanges();
                    response = new APIResponse() { StatusCode = "200", Status = true, ResponseObject = exist, Message = "Payment Date Updated" };
                    return Ok(response);
                }
                else
                {
                    response = new APIResponse() { StatusCode = "404", Status = false, ResponseObject = UpdateWorker, Message = "No user Found With these Parameters" };
                    return Content(HttpStatusCode.NotFound, response);
                }
               
            }
            catch (Exception ex)
            {
                var response = new APIResponse() { StatusCode = "500", Status = false, ResponseObject = UpdateWorker, Message = "Error" + ex.Message };
                return Content(HttpStatusCode.InternalServerError, response);
            }
        }


    }
}
