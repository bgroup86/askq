using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

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
        public void insert(Person p)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                if (p.Type == "Student")
                {
                    string qry = $"INSERT INTO ASKQstudent(Id, FirstName, LastName, gender, Email, Userpassword, img) VALUES('{p.Id}', '{p.FirstName}', '{p.LastName}', '{p.Gender}', '{p.Email}', '{p.Userpassword}', '{p.Img}')";
                    string connectionString = "ASKQConnection";
                    con = connect(connectionString);
                    cmd = new SqlCommand(qry, con);
                }
                else
                {
                    string qry = $"INSERT INTO ASKQlecturer(Id, FirstName, LastName, gender, Email, Userpassword, img) VALUES('{p.Id}', '{p.FirstName}', '{p.LastName}', '{p.Gender}', '{p.Email}', '{p.Userpassword}', '{p.Img}')";
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
                    string qry = $"select * from ASKQstudent where id='{idP}'";
                    string connectionString = "ASKQConnection";
                    con = connect(connectionString);
                    cmd = new SqlCommand(qry, con);
                }
                else
                {
                    string qry = $"select * from ASKQlecturer where id='{idP}'";
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
        public string Login(string PersonId, string userPassword)
        {
            SqlConnection con;
            SqlCommand cmd;
            string qry = $"select * from ASKQstudent where Id = '{PersonId}' and Userpassword = '{userPassword}'";
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
                string userID = "";
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    userID = Convert.ToString(dr["Id"]);
                }
                if (userID != "")
                {
                    return userID;
                }
                else
                {
                    SqlConnection con2;
                    SqlCommand cmd2;
                    string qry2 = $"select * from ASKQlecturer where Id = '{PersonId}' and Userpassword = '{userPassword}'";
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
                            userID = Convert.ToString(dr2["Id"]);
                        }
                            return userID;
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
    }
}