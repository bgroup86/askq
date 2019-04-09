using ASKQ.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;

namespace ASKQ.Controllers
{
    public class CourseController : ApiController
    {
        // GET api/<controller>/5
        public IEnumerable<Course> Get(string id)
        {
            try
            {
                List<Course> lc = new List<Course>();
                Course c = new Course();
                lc = c.GetCourse(id);
                return lc;

            }
            catch (Exception ex)
            {
                throw new Exception("error with getting the Courses" + ex);
            }
        }

        // POST api/<controller>
        public void Post([FromBody]Course c)
        {
            try
            {
                c.OpenNewCourse();

            }
            catch (Exception ex)
            {

                throw new Exception("Error in Add a New Course" + ex);
            }
        }

    }
}