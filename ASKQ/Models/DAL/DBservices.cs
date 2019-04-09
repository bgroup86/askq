using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using ASKQ.Models;

namespace ASKQ.Models.DAL
{
    public class DBservices
    {
        public SqlConnection connect(string conString)
        {
            // read the connection string from the configuration file
            string cStr = WebConfigurationManager.ConnectionStrings[conString].ConnectionString;
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }
        private SqlCommand CreateCommand(string CommandSTR, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand(); // create the command object
            cmd.Connection = con;              // assign the connection to the command object
            cmd.CommandText = CommandSTR;      // can be Select, Insert, Update, Delete 
            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds
            cmd.CommandType = System.Data.CommandType.Text; // the type of the command, can also be stored procedure
            return cmd;
        }

        internal void numAnswers(string v, int yesCounter, int noCounter)
        {
            throw new NotImplementedException();
        }

        public void insert(Person p)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                if (p.Type == "Student")
                {
                    string qry = $"INSERT INTO Student(Id, FirstName, LastName, gender, Email, Userpassword, img) VALUES('{p.Id}', '{p.FirstName}', '{p.LastName}', '{p.Gender}', '{p.Email}', '{p.Userpassword}', '{p.Img}')";
                    string connectionString = "ASKQConnection";
                    con = connect(connectionString);
                    cmd = new SqlCommand(qry, con);
                }
                else
                {
                    string qry = $"INSERT INTO Lecturer(Id, FirstName, LastName, gender, Email, Userpassword, img) VALUES('{p.Id}', '{p.FirstName}', '{p.LastName}', '{p.Gender}', '{p.Email}', '{p.Userpassword}', '{p.Img}')";
                    string connectionString = "ASKQConnection";
                    con = connect(connectionString);
                    cmd = new SqlCommand(qry, con);
                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            try
            {
                cmd.ExecuteNonQuery(); // execute the command
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                // close the db connection
                con.Close();
            }

        }

        public bool checkPersonId(string idP, string typeP)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                if (typeP == "Student")
                {
                    string qry = $"select * from Student where id='{idP}'";
                    string connectionString = "ASKQConnection";
                    con = connect(connectionString);
                    cmd = new SqlCommand(qry, con);
                }
                else
                {
                    string qry = $"select * from Lecturer where id='{idP}'";
                    string connectionString = "ASKQConnection";
                    con = connect(connectionString);
                    cmd = new SqlCommand(qry, con);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            try
            {
                cmd.ExecuteNonQuery(); // execute the command
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                string id = "";
                while (dr.Read())
                {
                    id = Convert.ToString(dr["Id"]);

                }
                if (id == "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        public Person Login(string PersonId, string userPassword)
        {
            SqlConnection con;
            SqlCommand cmd;
            string qry = $"select * from Student where Id = '{PersonId}' and Userpassword = '{userPassword}'";
            Person p = new Person();
            try
            {
                string connectionString = "ASKQConnection";
                con = connect(connectionString);
                cmd = new SqlCommand(qry, con);

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            try
            {
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    p.Id = Convert.ToString(dr["Id"]);
                    p.FirstName = Convert.ToString(dr["FirstName"]);
                    p.LastName = Convert.ToString(dr["LastName"]);
                    p.Gender = Convert.ToString(dr["gender"]);
                    p.Email = Convert.ToString(dr["Email"]);
                    p.Userpassword = Convert.ToString(dr["Userpassword"]);
                    p.Img = Convert.ToString(dr["img"]);
                    p.Type = "Student";

                }
                if (p.Id != null)
                {
                    return p;
                }
                else
                {
                    SqlConnection con2;
                    SqlCommand cmd2;
                    string qry2 = $"select * from Lecturer where Id = '{PersonId}' and Userpassword = '{userPassword}'";
                    try
                    {
                        string connectionString = "ASKQConnection2";
                        con2 = connect(connectionString);
                        cmd2 = new SqlCommand(qry2, con2);

                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
                    try
                    {
                        SqlDataReader dr2 = cmd2.ExecuteReader(CommandBehavior.CloseConnection);

                        while (dr2.Read())
                        {
                            p.Id = Convert.ToString(dr2["Id"]);
                            p.FirstName = Convert.ToString(dr2["FirstName"]);
                            p.LastName = Convert.ToString(dr2["LastName"]);
                            p.Gender = Convert.ToString(dr2["gender"]);
                            p.Email = Convert.ToString(dr2["Email"]);
                            p.Userpassword = Convert.ToString(dr2["Userpassword"]);
                            p.Img = Convert.ToString(dr2["img"]);
                            p.Type = "Lecturer";
                        }
                        return p;
                    }
                    catch (Exception ex)
                    {

                        // write to log
                        throw (ex);
                    }
                    finally
                    {
                        // close the db connection
                        con2.Close();
                    }
                }
            }
            catch (Exception ex)
            {

                // write to log
                throw (ex);
            }
            finally
            {
                // close the db connection
                con.Close();
            }
        }

        public void OpenNewCourse(Course c)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                string qry = $"INSERT INTO Course(courseName, info, courseYear, Id) VALUES('{c.CourseName}', '{c.Info}', '{c.CourseYear}', '{c.Id}')";
                string connectionString = "ASKQConnection";
                con = connect(connectionString);
                cmd = new SqlCommand(qry, con);

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            try
            {
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                // close the db connection
                con.Close();
            }

        }

        public List<Course> GetCourse(string id)
        {
            SqlConnection con;
            SqlCommand cmd;
            String qry = $"select * from Course where Id={id}";
            List<Course> lc = new List<Course>();
            try
            {
                string connectionString = "ASKQConnection";
                con = connect(connectionString);
                cmd = new SqlCommand(qry, con);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            try
            {
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    Course c = new Course();
                    c.CourseId = Convert.ToInt32(dr["courseId"]);
                    c.CourseName = Convert.ToString(dr["courseName"]);
                    c.Info = Convert.ToString(dr["info"]);
                    c.CourseYear = Convert.ToInt32(dr["courseYear"]);
                    lc.Add(c);
                }
                return lc;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                // close the db connection
                con.Close();
            }
        }

        public void OpenNewTopic(Lesson l)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                string qry = $"INSERT INTO Lesson(lessonName, info,Id, courseId, isActive,timeStampLesson) VALUES('{l.LessonName}', '{l.Info}','{l.Id}', '{l.CourseId}', '{l.IsActive}', '{l.TimeStampLesson}')";
                string connectionString = "ASKQConnection";
                con = connect(connectionString);
                cmd = new SqlCommand(qry, con);

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            try
            {
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                // close the db connection
                con.Close();
            }

        }

        public List<Lesson> GetLesson(string id, int idCourse)
        {
            SqlConnection con;
            SqlCommand cmd;
            String qry = $"select C.courseId, l.lessonId, L.lessonName, L.info,L.isActive,L.timeStampLesson from Lesson L join Course C on L.courseId=C.courseId where C.Id='{id}' and C.courseId={idCourse}";
            try
            {
                string connectionString = "ASKQConnection2";
                con = connect(connectionString);
                cmd = new SqlCommand(qry, con);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            try
            {
                SqlDataReader dr2 = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<Lesson> ls = new List<Lesson>();
                while (dr2.Read())
                {
                    Lesson l = new Lesson();
                    l.LessonId = Convert.ToInt32(dr2["lessonId"]);
                    l.LessonName = Convert.ToString(dr2["lessonName"]);
                    l.Info = Convert.ToString(dr2["info"]);
                    l.IsActive = Convert.ToBoolean(dr2["isActive"]);
                    l.TimeStampLesson = Convert.ToInt32(dr2["timeStampLesson"]);
                    ls.Add(l);
                }
                return ls;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                // close the db connection
                con.Close();
            }
        }

        public void AddNewFile(AddFile f)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                string qry = $"INSERT INTO Files(lessonId, courseId, fileName, fileDescription,path,typeFile) VALUES('{f.LessonId}', '{f.CourseId}', '{f.FileName}', '{f.FileDescription}','{f.Path}','{f.TypeFile}')";
                string connectionString = "ASKQConnection";
                con = connect(connectionString);
                cmd = new SqlCommand(qry, con);

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            try
            {
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                // close the db connection
                con.Close();
            }


        }

        public List<AddFile> GetFiles(int lessonId, int courseId)
        {
            SqlConnection con;
            SqlCommand cmd;
            String qry = $"select * from Files f where f.courseId={courseId} and f.lessonId={lessonId}";
            try
            {
                string connectionString = "ASKQConnection";
                con = connect(connectionString);
                cmd = new SqlCommand(qry, con);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            try
            {
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                List<AddFile> af = new List<AddFile>();
                while (dr.Read())
                {
                    AddFile f = new AddFile();
                    f.CourseId = courseId;
                    f.LessonId = lessonId;
                    f.Idfile = Convert.ToInt32(dr["idFile"]);
                    f.FileName = Convert.ToString(dr["fileName"]);
                    f.FileDescription = Convert.ToString(dr["fileDescription"]);
                    f.Path = Convert.ToString(dr["path"]);
                    f.TypeFile = Convert.ToString(dr["typeFile"]);

                    af.Add(f);
                }
                return af;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                // close the db connection
                con.Close();
            }
        }

        public void insert(RealTimeQuestion q)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {

                string qry = $"INSERT INTO RealTimeQuestion(content, uploadDate, fileLink, likeCounter, lessonId, courseId) VALUES('{q.Content}', '{q.UploadDate}', '{q.FileLink}',{q.likeCounter},{q.lessonId},{q.courseId})";
                string connectionString = "ASKQConnection";
                con = connect(connectionString);
                cmd = new SqlCommand(qry, con);

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            try
            {
                cmd.ExecuteNonQuery(); // execute the command
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                // close the db connection
                con.Close();
            }

        }

        public void insert(YesNoQuestion q)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {

                string qry = $"INSERT INTO YesNoQuestion(RealTimeQuestionId, content, uploadDate, fileLink, yesAmount, noAmount) VALUES({q.RealTimeQuestionId}, '{q.Content}', '{q.UploadDate}', '{q.FileLink}',{q.YesCounter}, {q.NoCounter})";
                string connectionString = "ASKQConnection";
                con = connect(connectionString);
                cmd = new SqlCommand(qry, con);

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            try
            {
                cmd.ExecuteNonQuery(); // execute the command
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                // close the db connection
                con.Close();
            }

        }
        public List<RealTimeQuestion> QList(string conString, string tableName, int lessonId, int courseId)
        {
            SqlConnection con = null;
            List<RealTimeQuestion> lq = new List<RealTimeQuestion>();
            try
            {
                con = connect(conString); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM " + tableName + " where isDeleted=0 AND isAnswered=0 and lessonId="+ lessonId + " and courseId="+ courseId;
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    RealTimeQuestion q = new RealTimeQuestion();
                    q.ID = Convert.ToInt32(dr["questionId"]);
                    q.Content = (string)dr["content"];
                    q.likeCounter = Convert.ToInt32(dr["likeCounter"]);

                    lq.Add(q);
                }

                return lq;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        //---------------------------------------------------------------------------------
        // get numbers of yes\no from the DB - dataReader
        //---------------------------------------------------------------------------------

        public YesNoQuestion NumAnswers(string conString, string tableName, int id)
        {
            SqlConnection con = null;
            YesNoQuestion a = new YesNoQuestion();

            try
            {
                con = connect(conString); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM " + tableName + " where RealTimeQuestionId=" + id;
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    a.ID = Convert.ToInt32(dr["questionId"]);
                    a.YesCounter = Convert.ToInt32(dr["yesAmount"]);
                    a.NoCounter = Convert.ToInt32(dr["noAmount"]);
                }

                return a;

            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

            public int UpdateQuestion(int r, string field)
            {
                SqlConnection con;
                SqlCommand cmd;

                try
                {
                    con = connect("ASKQConnection"); // create the connection
                }
                catch (Exception ex)
                {
                    // write to log
                    throw (ex);
                }

                String cStr = "UPDATE RealTimeQuestion SET " + field + "= 1 WHERE questionId = " + r;

                cmd = CreateCommand(cStr, con);             // create the command

                try
                {
                    int numEffected = cmd.ExecuteNonQuery(); // execute the command
                    return numEffected;
                }
                catch (Exception ex)
                {
                    return 0;
                    // write to log
                    throw (ex);
                }

                finally
                {
                    if (con != null)
                    {
                        // close the db connection
                        con.Close();
                    }
                }
            }

        public int UpdateAnswer (int r)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("ASKQConnection"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            String cStr = "UPDATE studentsAnswers SET theBest= 1 WHERE answerId = " + r;

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                return 0;
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        public RealTimeQuestion forYesNoQuestion(string conString, string tableName, int id)
        {
            SqlConnection con = null;
            RealTimeQuestion r = new RealTimeQuestion();

            try
            {
                con = connect(conString); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM " + tableName + " where questionId=" + id;
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    r.ID = Convert.ToInt32(dr["questionId"]);
                    r.Content = (string)dr["content"];
                }

                return r;

            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        public void insertAnswer(studentsAnswers a)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {

                string qry = $"INSERT INTO studentsAnswers(content, questionId, likeCounter) VALUES('{a.AnswerContent}', {a.QuestionId}, {a.LikeCounter})";
                string connectionString = "ASKQConnection";
                con = connect(connectionString);
                cmd = new SqlCommand(qry, con);

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            try
            {
                cmd.ExecuteNonQuery(); // execute the command
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                // close the db connection
                con.Close();
            }

        }

        public List<studentsAnswers> AList(string conString, string tableName, int id)
        {
            SqlConnection con = null;
            List<studentsAnswers> al = new List<studentsAnswers>();
            try
            {
                con = connect(conString); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM " + tableName + " where questionId=" + id;
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    studentsAnswers a = new studentsAnswers();

                    a.ID = Convert.ToInt32(dr["answerId"]);
                    a.AnswerContent = (string)dr["content"];
                    a.QuestionId = Convert.ToInt32(dr["questionId"]);
                    a.LikeCounter = Convert.ToInt32(dr["likeCounter"]);

                    al.Add(a);
                }

                return al;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        public void insertAnswerAndQuestion(QuestionAndAnswer qa)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {

                string qry = $"INSERT INTO studentsAnswers(questionId, questionContent, answerId, answerContant) VALUES({qa.QuestionId}, '{qa.QuestionContent}', {qa.AnswerId} , '{qa.AnswerContent}')";
                string connectionString = "ASKQConnection";
                con = connect(connectionString);
                cmd = new SqlCommand(qry, con);

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            try
            {
                cmd.ExecuteNonQuery(); // execute the command
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                // close the db connection
                con.Close();
            }

        }

    }
}