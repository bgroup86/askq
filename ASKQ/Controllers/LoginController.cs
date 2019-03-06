using ASKQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ASKQ.Controllers
{
    public class LoginController : ApiController
    {
        [HttpGet]
        [Route("api/login")]
        public string GET(string PersonId, string userPassword)
        {
            try
            {
                Person p = new Person();
                string idPerson = p.Login(PersonId, userPassword);
                return idPerson;
            }
            catch (Exception ex)
            {

                throw new Exception("Your details are incorrect" + ex);

            }
        }
    }
}