using ASKQ.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASKQ.Controllers;

namespace ASKQ.Models
{
    public class YesNoQuestion
    {

        public int ID { get; set; }
        public int RealTimeQuestionId { get; set; }
        public string Content { get; set; }
        public string UploadDate { get; set; }
        public string FileLink { get; set; }
        public int NoCounter { get; set; }
        public int YesCounter { get; set; }

        public YesNoQuestion() { }

        public YesNoQuestion(int RealTimeQuestionid, string content, string uploadDate, string fileLink, int noCounter, int yesCounter)
        {
            RealTimeQuestionId = RealTimeQuestionid;
            Content = content;
            UploadDate = uploadDate;
            FileLink = fileLink;
            NoCounter = noCounter;
            YesCounter = yesCounter;
        }

        public void insert()
        {
            DBservices dbs = new DBservices();
            dbs.insert(this);
        }

        public YesNoQuestion NumAnswers(int id)
        {
            DBservices dbs = new DBservices();
            return dbs.NumAnswers("ASKQConnection", "YesNoQuestion", id);
        }
    }
}
