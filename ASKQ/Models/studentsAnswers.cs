using ASKQ.Models.DAL;
using System.Collections.Generic;

namespace ASKQ.Models
{
    public class studentsAnswers
    {
        public int ID { get; set; }
        public string AnswerContent { get; set; }
        public int QuestionId { get; set; }
        public int LikeCounter { get; set; }

    public studentsAnswers() { }

        public studentsAnswers(string answerContent, int questionId, int likeCounter)
        {
            AnswerContent = answerContent;
            QuestionId = questionId;
            this.LikeCounter = likeCounter;
        }

        public void insert()
        {
            DBservices dbs = new DBservices();
            dbs.insertAnswer(this);
        }

        public List<studentsAnswers> ReadList(int id)
        {
            DBservices dbs = new DBservices();
            List<studentsAnswers> sa = dbs.AList("ASKQConnection", "studentsAnswers", id);
            return sa;
        }

    }
}