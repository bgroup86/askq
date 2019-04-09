using ASKQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;

namespace ASKQ.Controllers
{
    public class PersonController : ApiController
    {
        [HttpPost]
        [Route("api/person")]
        public void Post([FromBody]Person p)
        {
            try
            {
                p.insert();
            }
            catch (Exception ex)
            {

                throw new Exception("Error in insert" + ex);
            }
        }
        [HttpGet]
        [Route("api/person")]
        public bool Get(string PersonId, string PersonType)
        {
            Person p = new Person();
            bool check = p.checkPersonId(PersonId, PersonType);
            return check;
        }

     
        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
    }
}