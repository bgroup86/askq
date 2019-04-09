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
    public class LessonController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<Lesson> Get(string id,int courseId)
        {
            try
            {
                List<Lesson> ls = new List<Lesson>();
                Lesson l = new Lesson();
                ls = l.GetLesson(id, courseId);
                return ls;

            }
            catch (Exception ex)
            {
                throw new Exception("error with getting the Courses" + ex);
            }
        }


        // POST api/<controller>
        public void Post([FromBody]Lesson l)
        {
            try
            {
                string path = Path.Combine(HostingEnvironment.MapPath("~/uploadedFiles"), l.CourseId.ToString());
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                l.OpenNewTopic();
            }
            catch (Exception ex)
            {

                throw new Exception("Error in Add a New Course" + ex);
            }
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}