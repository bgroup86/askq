using ASKQ.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ASKQ.Controllers
{
    public class YesNoQuestionController : ApiController
    {
        [HttpPost]
        public void Post([FromBody]YesNoQuestion q)
        {
            try
            {
                q.insert();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in insert" + ex);
            }
        }

        [HttpGet]
        public YesNoQuestion YesOrNo(int id)
        {
            YesNoQuestion a = new YesNoQuestion();
            return a.NumAnswers(id);
        }

    }
}
